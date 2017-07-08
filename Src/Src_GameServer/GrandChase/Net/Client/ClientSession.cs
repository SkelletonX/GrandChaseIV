using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrandChase.IO.Packet;
using System.Net.Sockets;
using Common;
using GrandChase.IO;
using System.Net;
using GrandChase.Security;
using GrandChase.Function;
using Manager.Factories;
using Manager;

namespace GrandChase.Net.Client
{
    public class ClientSession : Session
    {
        // Security START
        public byte[] CRYPT_KEY { get; set; }
        public byte[] CRYPT_HMAC { get; set; }
        public byte[] CRYPT_PREFIX = new byte[2];
        public int CRYPT_COUNT;
        // Security END

        public User MyUser = new User();
        public GCCommon MyCommon = new GCCommon();
        public Inventory MyInventory = new Inventory();
        public Character MyCharacter = new Character();
        public Pet MyPet = new Pet();
        public Square square = new Square();
        public Shop MyShop = new Shop();
        public Gacha GachaList = new Gacha();
        public Quests MyQuests = new Quests();
        public Guild MyGuilds = new Guild();
        public Agit MyAgit = new Agit();
        public Heroico heroico = new Heroico();
        public Manufacture manufacture = new Manufacture();

        public int ServerType;
        public int LoginUID;
        public string Login;
        public string Nick;
        public int GamePoint = 0;
        public int VirtualCash = 0;
        public int CurrentChar;
        public int BonusLife = 0;
        public int isBan = 0;
        public int AuthLevel = 0;
        public int UserAuthLevel = 3;
        public int Online;
        public bool PCBang = false;
        public byte PCBangType = 0;
        public bool IsHost = true;
        public bool IsLive = true;
        public Channel CurrentChannel { get; set; }
        public Room CurrentRoom { get; set; }

        // Option
        public bool InviteDeny;

        public int LastHeartBeat { get; set; }
        public uint IP { get; set; }
        public ushort Port { get; set; }

        public ClientSession(Socket pSocket) : base(pSocket)
        {
            IP = BitConverter.ToUInt32(IPAddress.Parse(GetIP()).GetAddressBytes(), 0);

            InitiateReceive(2, true);

            CurrentChannel = null;
            CurrentRoom = null;

            CRYPT_KEY = CryptoGenerators.GenerateKey();
            CRYPT_HMAC = CryptoGenerators.GenerateKey();
            byte[] TEMP_PREFIX = CryptoGenerators.GeneratePrefix(); 
            LogFactory.GetLog("KEY").LogHex("", CRYPT_KEY);
            LogFactory.GetLog("HMAC").LogHex("", CRYPT_HMAC);
            using (OutPacket oPacket = new OutPacket(GameOpcodes.SET_SECURITY_KEY_NOT))
            {
                oPacket.WriteBytes(TEMP_PREFIX);
                oPacket.WriteInt((int)8);
                oPacket.WriteBytes(CRYPT_HMAC);
                oPacket.WriteInt((int)8);
                oPacket.WriteBytes(CRYPT_KEY);
                oPacket.WriteHexString("00 00 00 01 00 00 00 00 00 00 00 00");

                oPacket.Assemble(CryptoConstants.GC_DES_KEY, CryptoConstants.GC_HMAC_KEY, CRYPT_PREFIX, ++CRYPT_COUNT);
                Send(oPacket);
            }

            // Prefix
            CRYPT_PREFIX = TEMP_PREFIX;
        }

        public string GetIP()
        {
            if( _socket == null ) return "0.0.0.0";

            IPEndPoint remoteIpEndPoint = _socket.RemoteEndPoint as IPEndPoint;
            return ( remoteIpEndPoint.Address.ToString() );
        }

        public override void OnPacket( InPacket iPacket )
        {
            try
            {
                iPacket.Decrypt(CRYPT_KEY);

                GameOpcodes uOpcode = (GameOpcodes)iPacket.ReadUShort();
                int uSize = iPacket.ReadInt();
                bool isCompress = iPacket.ReadBool();
                int cSize = 0;
                if (isCompress == true)
                {
                    cSize = iPacket.ReadInt();
                    LogFactory.GetLog("Main").LogWarning("[{0}] Pacote comprimido {1}({2})", Login, (int)uOpcode, uOpcode.ToString());
                } else
                {
                    LogFactory.GetLog("Main").LogInfo("[{0}] Packet {1}({2})", Login, (int)uOpcode, uOpcode.ToString());
                }

                LogFactory.GetLog("Main").LogHex("Pacote: ", iPacket.ToArray());

                switch ( uOpcode )
                {
                    case GameOpcodes.HEART_BIT_NOT: // 0
                        OnHeartBeatNot();
                        break;
                    case GameOpcodes.EVENT_VERIFY_ACCOUNT_REQ: // 2
                        MyUser.OnLogin(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_GET_FULL_SP_INFO_REQ: // 423 0x1A7
                        MyCommon.OnGetFullSPInfo(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_PET_COSTUM_LIST_REQ: // 517 0x205
                        MyCommon.OnPetCostumList(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_INVEN_BUFF_ITEM_LIST_REQ: // 1226 0x04CA
                        MyCommon.OnInvenBuffItemList(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_DEPOT_INFO_REQ: // 1340 0x053C
                        MyCommon.OnDepotInfo(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_STAT_CLIENT_INFO: // 226 0x00E2
                        MyCommon.OnStatClientInfo(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_COST_RATE_FOR_GAMBLE_BUY_REQ: // 871 0x0367
                        MyCommon.OnCostRateForGambleBuy(this);
                        break;
                    case GameOpcodes.EVENT_ENTER_AGIT_REQ://AGITT
                        MyAgit.EnterAgit(this);
                        break;
                    case GameOpcodes.EVENT_AGIT_LOADING_COMPLETE_REQ://AGITT
                        MyAgit.AgitLoadComplete(this);
                        break;
                    case GameOpcodes.EVENT_REGISTER_NICKNAME_REQ: // 134 0x0086
                        MyUser.OnRegisterNick(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_SET_IDLE_STATE_REQ: // 835 0x0343
                        MyCommon.OnSetIDLE(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_CHAR_SELECT_JOIN_REQ: // 1555 0x0613
                        MyUser.OnCharSelectJoin(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_CHOICE_BOX_LIST_REQ: // 1012 0x03F4
                        MyCommon.OnChoiceBoxList(this);
                        break;
                    case GameOpcodes.EVENT_EXP_POTION_LIST_REQ: // 1338 0x053A
                        MyCommon.OnExpPotionList(this);
                        break;
                    case GameOpcodes.EVENT_AGIT_STORE_CATALOG_REQ: // 1114 0x054A
                        MyCommon.OnAgitStoreCatalog(this);
                        break;
                    case GameOpcodes.EVENT_AGIT_STORE_MATERIAL_REQ: // 1116 0x045C
                        MyCommon.OnAgitStoreMaterial(this);
                        break;
                    case GameOpcodes.EVENT_AGIT_MAP_CATALOGUE_REQ: // 1106 0x0452
                        MyCommon.OnAgitMapCatalogue(this);
                        break;
                    case GameOpcodes.EVENT_FAIRY_TREE_LV_TABLE_REQ: // 1184 0x04A0
                        MyCommon.OnFaityTreeLvTable(this);
                        break;
                    case GameOpcodes.EVENT_INVITE_DENY_NOT: // 348 0x015C
                        InviteDeny = iPacket.ReadBool();
                        break;
                    case GameOpcodes.EVENT_GET_USER_DONATION_INFO_REQ: // 523 0x020B
                        MyCommon.OnGetUserDonationInfo(this);
                        break;
                    case GameOpcodes.EVENT_RECOMMEND_FULL_INFO_REQ: // 567 0x0237
                        MyCommon.OnRecommentUser(this);
                        break;
                    case GameOpcodes.EVENT_USER_BINGO_DATA_REQ: // 654 0x28E
                        MyCommon.OnUserBingoData(this);
                        break;
                    case GameOpcodes.EVENT_CHANNEL_LIST_REQ: // 14 0x0E
                        MyCommon.OnChannelList(this,iPacket);
                        break;
                    case GameOpcodes.EVENT_DONATION_INFO_REQ: // 525 0x020D
                        MyCommon.OnDonationInfo(this);
                        break;
                    case GameOpcodes.EVENT_ENTER_CHANNEL_REQ: // 12 0x0C
                        MyUser.OnEnterChannel(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_LEAVE_CHANNEL_NOT: // 26
                        MyUser.OnLeaveChannel(this);
                        break;
                    case GameOpcodes.EVENT_CREATE_ROOM_REQ: // 24 방 만들기
                        MyUser.OnCreateRoom(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_CHANGE_ROOM_INFO_REQ: // 28 0x1C 방의 게임모드 정보 바뀜
                        CurrentRoom.OnChangeRoomInfo(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_CHANGE_ROOMUSER_INFO_REQ: // 40 0x28 방의 유저 정보 바뀜
                        CurrentRoom.OnChangeUserInfo(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_LEAVE_ROOM_REQ: // 33 0x21 방에서 나가기 요청
                        CurrentRoom.OnLeaveRoom(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_ROOM_LIST_REQ: // 16 0x10 방 목록 요청
                        CurrentChannel.OnRoomList(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_JOIN_ROOM_REQ: // 20 방 입장 요청
                        CurrentChannel.OnJoinRoom(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_START_GAME_REQ: // 36 게임 시작 요청
                        CurrentRoom.OnGameStart(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_RELAY_LOADING_STATE: // 842 게임로딩중 로딩진행도
                        CurrentRoom.OnLoadState(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_LOAD_COMPLETE_NOT: // 39 로딩 끝났다는거 전송 (게임으로 들여보내짐)
                        CurrentRoom.OnLoadComplete(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_STAGE_LOAD_COMPLETE_NOT: // 927 스테이지 로딩이 끝났다
                        CurrentRoom.OnStageLoadComplete(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_ROOM_MEMBER_PING_INFO_REQ: // 837 게임 멤버 핑 정보 요청 (게임시작 할때 쓰나봄)
                        CurrentRoom.OnPingInfo(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_GET_ROOMUSER_IDLE_STATE_REQ: // 833 자리비움 상태 요청
                        CurrentRoom.OnIdleInfo(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_END_GAME_REQ: // 게임 종료
                        CurrentRoom.OnGameEnd(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_CHAT_REQ: // 6 채팅!
                        CurrentChannel.OnChat(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_LEAVE_GAME_REQ: // 44 게임중 나가기
                        CurrentRoom.OnLeaveGame(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_EQUIP_ITEM_REQ: // 62 장비 변경 요청
                        MyCharacter.OnEquipItem(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_CHANGE_LOOK_EQUIP_REQ: // 860 방에서 장비변경 요청 (이게 선처리 된 후 EVENT_EQUIP_ITEM_REQ)
                        MyCharacter.OnChangeEquipInRoom(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_SET_PRESS_STATE_REQ: // 방에서 다른 용무중
                        CurrentRoom.OnSetPressState(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_SKILL_TRAINING_REQ: // 스킬 배우기
                        MyCharacter.OnTrainSkill(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_SET_SKILL_REQ: // 스킬 장착 요청
                        MyCharacter.OnSetSkill(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_SERVER_LIST_REQ:
                        MyUser.SendServerList(this);
                        break;
                    case GameOpcodes.EVENT_SQUARE_LIST_REQ:
                        square.SquareList(this);
                        break;
                    case GameOpcodes.EVENT_ENTER_SQUARE_REQ:
                        square.enterSquare(this);
                        break;
                    case GameOpcodes.EVENT_LEAVE_SQUARE_REQ:
                        square.LeaveSquare(this);
                        break;
                    case GameOpcodes.EVENT_CASHBACK_EXTRA_RATIO_INFO_REQ:
                        MyShop.CashRatio(this);
                        break;
                    case GameOpcodes.EVENT_PACKAGE_INFO_REQ:
                        MyShop.packageInfo(this);
                        break;
                    case GameOpcodes.EVENT_PACKAGE_INFO_DETAIL_REQ:
                        MyShop.packageInfoDetail(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_ITEM_BUY_CHECK_REQ:
                        MyShop.CheckItem(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_BUY_VIRTUAL_CASH_REQ:
                        MyShop.BuyVP(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_BUY_FOR_GP_REQ:
                        MyShop.BuyGP(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_CHARISMAS_EVENT_INFO_REQ:
                        MyCommon.Event_Gifts(this);
                        break;
                    case GameOpcodes.EVENT_CHECK_ROOM_USERLIST_NOT:
                        CurrentRoom.checkRoomUser(this);
                        break;
                    case GameOpcodes.EVENT_STAT_LOADING_TIME_NOT:
                        CurrentRoom.statLoadingTime(this,iPacket);
                        break;
                    case GameOpcodes.EVENT_SPECIAL_REWARD_REQ:
                        CurrentRoom.RewardItem(this,iPacket);
                        break;
                    case GameOpcodes.EVENT_DEPOT_CHAR_TAB_INFO_REQ:
                        GachaList.Depot_Char_tab(this,iPacket);
                        break;
                    case GameOpcodes.EVENT_GACHA_REWARD_LIST_REQ:
                        GachaList.Gacha_Reward_List(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_GACHA_SET_REWARD_LIST_REQ:
                        GachaList.Gacha_SET_Reward_List(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_GACHA_LEVEL_ACTION_REQ:
                        GachaList.Gacha_Action(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_REMOVE_MISSION_REQ:
                        MyQuests.RemoveMission(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_REGIST_MISSION_REQ:
                        MyQuests.RegisterMission(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_COMPLETE_MISSION_REQ:
                        MyQuests.CompleteMission(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_CREATE_PET_REQ:
                        MyPet.create_pet(this, iPacket);
                        break;
                    case GameOpcodes.EVENT_HERO_ITEM_CATALOG_REQ:
                        heroico.Hero_Item_Catalog(this);
                        break;
                    case GameOpcodes.EVENT_HERO_ITEM_MATERIAL_REQ:
                        heroico.Hero_Item_Material(this);
                        break;
                    case GameOpcodes.EVENT_MANUFACTURES3_CATALOG_REQ:
                        manufacture.Manufacture_Catalog(this);
                        break;
                    case GameOpcodes.EVENT_SPECIFIC_ITEM_BREAKUP_INFO_REQ:
                        manufacture.SpecificItemBreakup(this);
                        break;
                    case GameOpcodes.EVENT_MANUFACTURES3_MATERIAL_REQ:
                        manufacture.ManuFactureMaterial(this);
                        break;
                    case GameOpcodes.EVENT_DUNGEON_REWARD_EXP_REQ:
                        CurrentRoom.RewardEXP(this,iPacket);
                        break;
                    case GameOpcodes.EVENT_USER_LIST_REQ:
                        MyUser.ListUsers(this);
                        break;

                    default:
                        {
                            LogFactory.GetLog("Main").LogWarning("pacote indefinido foi recebida. Opcode: {0}({1})", (int)uOpcode, uOpcode.ToString());
                            LogFactory.GetLog("Main").LogHex("Pacote: ", iPacket.ToArray());
                            break;
                        }
                }
            }
            catch( Exception e )
            {
                LogFactory.GetLog("Main").LogError( e.ToString() );
                Close();
            }
        }



        public override void OnDisconnect()
        {
            LogFactory.GetLog("Main").LogInfo("A conexão de soquete foi perdida. ID: {0}", Login);

            User.StatusOnline(null, Login, 0, 1);


            // 방에 입장중이면 방에서 나간다.
            if( CurrentRoom != null )
                CurrentRoom.ProcessLeaveRoom(this);

            // 채널에 입장중이면 나간다.
            if (CurrentChannel != null)
                MyUser.OnLeaveChannel(this);


            TSingleton<ClientHolder>.Instance.DestoryAccount(this);
        }

        public void OnHeartBeatNot()
        {
            User.StatusOnline(null, Login, 1, 1);
            LastHeartBeat = Environment.TickCount;
        }
    }
}
