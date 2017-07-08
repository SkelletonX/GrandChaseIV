using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrandChase.IO.Packet;
using GrandChase.Net.Client;
using GrandChase.Data;
using GrandChase.Security;
using GrandChase.IO;
using GrandChase.Function;
using GrandChase.Utilities;
using Manager.Factories;
using Manager;

namespace GrandChase.Function
{
    public class Gacha
    {
        /*
         * helm 0
         * upper 1
         * lower 2
         * Weapon 3
         * gloves 8
         * shoes 9
         * Circlet 10
         * Wings 12
         * Mask 11
         * cloak 13
         * Stompers 14
         * Shields 15
         */

        public int CharIndex;
        public int Gem;
        public int Pet = 547540;

        public int Helm;//Helm
        public int HelmID = 1;//Helm
        public int UpperArmor;//Upper Armor
        public int UpperArmorID = 2;//Upper Armor
        public int LowerArmor;//Lower Armor
        public int LowerArmorID = 3;//Lower Armor
        public int Gloves;//Gloves
        public int GlovesID = 4;//Gloves
        public int Shoes;//Shoes
        public int ShoesID = 5;//Shoes
        public int Cloak;//Cloak
        public int CloakID = 6;//Cloak
        //ACESSORIOS
        public int Circlet;//Circlet
        public int CircletID = 892910;//Circlet
        public int Wings;//Wings
        public int WingsID = 892920;//Wings
        public int Stompers;//Stompers
        public int StompersID = 892930;//Stompers
        public int Shields;//Shields
        public int ShieldsID = 892940;//Shields
        public int Mask;//Mask
        public int MaskID = 893100;//Mask
        public int[] Weapon = new int[3];
        
        Random RandomItems = new Random();
        Random RandomUID = new Random();

        public int loadweapon(int classe)
        {
            Weapon[0] = 892300;
            Weapon[1] = 892300;
            Weapon[2] = 892300;
            Weapon[3] = 892300;
            return Weapon[classe];
        }

        public int SortedItem;

        Random randomFail = new Random();
        public static int[] ItemsObter = 
        {
            287060, //
            627290 //PEDRA  de Fortificaçao
        };        

        public void Depot_Char_tab(ClientSession cs,InPacket ip)
        {
            ushort test = ip.ReadUShort();
            CharIndex = ip.ReadInt();
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_DEPOT_CHAR_TAB_INFO_ACK))
            {
                LogFactory.GetLog("Gacha").LogInfo("CHARACTER: " + CharIndex);
                oPacket.WriteInt(5);
                oPacket.WriteByte(0);
                oPacket.WriteByte((byte)CharIndex);
                oPacket.WriteHexString("01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
                LogFactory.GetLog("DEPOT_CHAR").LogHex("",ip.ToArray());
            }
        }

        public void Gacha_Reward_List(ClientSession cs, InPacket ip)
        {
            int u1 = ip.ReadInt();
            Gem = ip.ReadInt();

            Weapon[0] = 892300;
            Helm = 546000;//Helm
            UpperArmor = 546010;//Upper Armor
            LowerArmor = 546020;//Lower Armor
            Gloves = 546030;//Gloves
            Shoes = 546040;//Shoes
            Cloak = 546050;//Cloak
            Circlet = 547240;//Circlet
            Wings = 547260;//Wings
            Stompers = 547280;//Stompers
            Shields = 547270;//Shields
            Mask = 547250;//Mask

            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_GACHA_REWARD_LIST_ACK))
            {
                LogFactory.GetLog("Gacha").LogInfo("GEMA: "+Gem);
                oPacket.WriteInt(u1);
                oPacket.WriteInt(Gem);
                oPacket.WriteHexString("01 00 00 00 02 00 00 00 00");
                oPacket.WriteInt(6);//Length items
                oPacket.WriteInt(Helm);
                oPacket.WriteInt(UpperArmor);
                oPacket.WriteInt(LowerArmor);
                oPacket.WriteInt(Gloves);
                oPacket.WriteInt(Shoes);
                oPacket.WriteInt(Cloak);
                oPacket.WriteInt(1);
                oPacket.WriteInt(5);//Length Acessorios
                oPacket.WriteInt(Circlet);
                oPacket.WriteInt(Wings);
                oPacket.WriteInt(Stompers);
                oPacket.WriteInt(Shields);
                oPacket.WriteInt(Mask);
                oPacket.WriteHexString("00 00 00 06 00 00 00 1E 00 00 00 02 00 00 00 28 00 00 00 02 00 00 00 32 00 00 00 03 00 00 00 3C 00 00 00 03 00 00 00 46 00 00 00 04 00 00 00 50 00 00 00 05 00 00 00 00");
                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public void Gacha_SET_Reward_List(ClientSession cs, InPacket ip)
        {
            int unk1 = ip.ReadInt();
            int itemID = ip.ReadInt();
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_GACHA_SET_REWARD_LIST_ACK))
            {
                oPacket.WriteInt(unk1);
                oPacket.WriteInt(itemID);
                oPacket.WriteHexString("01 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 01");
                oPacket.WriteInt(Weapon[0]);//Weapon
                oPacket.WriteHexString("00 00 00 01 00 00 00 01");
                oPacket.WriteInt(Pet);//PET
                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }
        
        public void Gacha_Action(ClientSession cs, InPacket ip)
        {           
            int CharID = ip.ReadInt();
            int Set =ip.ReadInt();
            int Level = ip.ReadInt();
            int Gema = ip.ReadInt();

            int rndItems = RandomItems.Next(30);
            int rndUID = RandomItems.Next(873604976);

            if (Level == 30)
            {
                percent30(rndItems, Set);
            }
            if (Level == 40)
            {
                percent30(rndItems, Set);
            }
            if (Level == 50)
            {
                percent30(rndItems, Set);
            }
            if (Level == 60)
            {
                percent30(rndItems, Set);
            }
            if (Level == 70)
            {
                percent50(rndItems, Set);
            }
            if (Level == 80)
            {
                percent50(rndItems, Set);
            }

            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_GACHA_ACTION_ACK))
            {
                oPacket.WriteInt(0);
                oPacket.WriteInt(CharID);
                oPacket.WriteInt(Gema);
                oPacket.WriteHexString("00 00 00 01 34 12 27 47 00 00 00 03 00 00 00 03 00 00 FF FF 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0F 00 00 00 00 00 00 00 00 00 01 8E 8E 00 00 00 01 34 12 24 65 00 00 00 3A 00 00 00 3A 00 02 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 1E 00 00 00 01");
                oPacket.WriteInt(SortedItem);//00 09 92 5A
                oPacket.WriteInt(1);
                oPacket.WriteInt(rndUID);//34 12 27 70
                oPacket.WriteHexString("00 00 00 02 00 00 00 02");
                oPacket.WriteInt(Level);
                oPacket.WriteHexString("00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 00 00 00 00 00 00");
                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
                LogFactory.GetLog("GACHA ACTION").LogHex("",ip.ToArray());
            }
        }

        public void percent30(int rndItems, int Set)
        {
            int Result = randomFail.Next(ItemsObter.Length);
            if (Set == 0)
            {
                if (rndItems < 5)
                {
                    if (HelmID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Helm;
                        Helm = 0;
                    }
                }

                if (rndItems == 5)
                {
                    if (HelmID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Helm;
                        Helm = 0;
                    }
                }
                if (rndItems > 5 && rndItems < 10)
                {
                    SortedItem = Result;
                }
                if (rndItems == 10)
                {
                    if (UpperArmorID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = UpperArmor;
                        UpperArmorID = 0;
                    }
                }
                if (rndItems > 10 && rndItems < 11)
                {
                    SortedItem = Result;
                }
                if (rndItems > 15 && rndItems < 20)
                {
                    if (LowerArmorID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = LowerArmor;
                        LowerArmorID = 0;
                    }
                }
                if (rndItems == 15)
                {
                    if (LowerArmorID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = LowerArmor;
                        LowerArmorID = 0;
                    }
                }
                if (rndItems > 15 && rndItems < 20)
                {
                    SortedItem = Result;
                }
                if (rndItems == 20)
                {
                    if (LowerArmorID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = LowerArmor;
                        LowerArmorID = 0;
                    }
                }
                if (rndItems > 20 && rndItems < 25)
                {
                    SortedItem = Result;
                }
                if (rndItems == 25)
                {
                    if (GlovesID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Gloves;
                        GlovesID = 0;
                    }
                }
                if (rndItems > 25 && rndItems < 30)
                {
                    SortedItem = Result;
                }
            }
            if (Set == 1)
            {
                if (rndItems < 5)
                {
                    if (CircletID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Circlet;
                        CircletID = 0;
                    }
                }

                if (rndItems == 5)
                {
                    if (CircletID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Circlet;
                        CircletID = 0;
                    }
                }
                if (rndItems > 5 && rndItems < 10)
                {
                    SortedItem = Result;
                }
                if (rndItems == 10)
                {
                    if (WingsID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Wings;
                        WingsID = 0;
                    }
                }
                if (rndItems > 10 && rndItems < 15)
                {
                    SortedItem = Result;
                }
                if (rndItems == 15)
                {
                    SortedItem = Stompers;
                }
                if (rndItems > 15 && rndItems < 20)
                {
                    SortedItem = Result;
                }
                if (rndItems == 20)
                {
                    if (ShieldsID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Shields;
                        ShieldsID = 0;
                    }
                }
                if (rndItems > 20 && rndItems < 25)
                {
                    SortedItem = Result;
                }
                if (rndItems == 25)
                {
                    if (MaskID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Mask;
                        MaskID = 0;
                    }
                }
                {
                    if (MaskID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Mask;
                        MaskID = 0;
                    }
                }

                if (rndItems == 30)
                {
                    if (MaskID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Mask;
                        MaskID = 0;
                    }
                }
            }
        }
        

        public void percent50(int rndItems,int Set)
        {
            int Result = randomFail.Next(ItemsObter.Length);
            if (Set == 0)
            {
                if (rndItems < 5)
                {
                    SortedItem = Result;
                }

                if (rndItems == 5)
                {
                    if (HelmID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Helm;
                        Helm = 0;
                    }
                }
                if (rndItems > 5 && rndItems < 10)
                {
                    SortedItem = Result;
                }
                if (rndItems == 10)
                {
                    if (UpperArmorID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = UpperArmor;
                        UpperArmorID = 0;
                    }
                }
                if (rndItems > 10 && rndItems < 15)
                {
                    SortedItem = Result;
                }
                if (rndItems == 15)
                {
                    if (LowerArmorID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = LowerArmor;
                        LowerArmorID = 0;
                    }
                }
                if (rndItems > 15 && rndItems < 20)
                {
                    SortedItem = Result;
                }
                if (rndItems == 20)
                {
                    if (LowerArmorID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = LowerArmor;
                        LowerArmorID = 0;
                    }
                }
                if (rndItems > 20 && rndItems < 25)
                {
                    SortedItem = Result;
                }
                if (rndItems == 25)
                {
                    if (GlovesID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Gloves;
                        GlovesID = 0;
                    }
                }
                if (rndItems > 25 && rndItems < 30)
                {
                    SortedItem = Result;
                }
            }
            if (Set == 1)
            {
                if (rndItems < 5)
                {
                    SortedItem = Result;
                }

                if (rndItems == 5)
                {
                    if (CircletID== 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Circlet;
                        CircletID = 0;
                    }
                }
                if (rndItems > 5 && rndItems < 10)
                {
                    if (StompersID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Stompers;
                        StompersID = 0;
                    }
                }
                if (rndItems == 10)
                {
                    if (WingsID== 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Wings;
                        WingsID = 0;
                    }
                }
                if (rndItems > 10 && rndItems < 15)
                {
                    if (StompersID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Stompers;
                        StompersID = 0;
                    }
                }
                if (rndItems == 15)
                {
                    if (StompersID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Stompers;
                        StompersID = 0;
                    }
                }
                if (rndItems > 15 && rndItems < 20)
                {
                    if (ShieldsID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Shields;
                        ShieldsID = 0;
                    }
                }
                if (rndItems == 20)
                {
                    if (ShieldsID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Shields;
                        ShieldsID = 0;
                    }
                }
                if (rndItems > 20 && rndItems < 25)
                {
                    if (MaskID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Mask;
                        MaskID = 0;
                    }
                }
                if (rndItems == 25)
                {
                    if (MaskID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Mask;
                        MaskID = 0;
                    }
                }
                if (rndItems > 25 && rndItems < 30)
                {
                    if (MaskID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Mask;
                        MaskID = 0;
                    }
                }

                if (rndItems == 30)
                {
                    if (MaskID == 0)
                    {
                        SortedItem = Result;
                    }
                    else
                    {
                        SortedItem = Mask;
                        MaskID = 0;
                    }
                }
            }          
        }
        public void Select_Items()
        {
        }
    }
}
