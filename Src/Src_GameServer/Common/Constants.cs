namespace GrandChase
{
    public class EndianConvert
    {
        /*
           C# Endian Converter
        */
        public static byte[] EndianConverter(byte[] val)

        {
            int len = val.Length;
            byte[] tmp = new byte[val.Length];
            int i, j;

            for (i = 0, j = len; i < len; i++, j--)
            {
                tmp[i] = val[j - 1];
            }

            for (i = 0; i < len; i++)
            {
                val[i] = tmp[i];
            }

            return tmp;
        }

        public static byte[] EndianConverter(byte[] val, int startIndex, int count)
        {
            //int len = val.Length;
            byte[] tmp = new byte[count];
            int i, j;

            j = startIndex + count;

            for (i = 0; i < count; i++, j--)
            {
                tmp[i] = val[j - 1];
            }

            for (i = 0; i < count; i++, j++)
            {
                val[j] = tmp[i];
            }

            return tmp;
        }
    }

    public static class Constants
    {
        public static readonly Version Version = new Version()
        {
            Major = 1,
            Minor = 1295,
            Localisation = Localisation.Korea,
            TestServer = false
        };
    }

    public static class PacketConstants
    {
        public const uint TCP_PACKET_CHECKSUM_XOR_VALUE = 0xC9F84A90;
        public const uint PACKET_HEADER_XOR_VALUE = 0xF834A608;
        public const int PACKET_IV_XOR_VALUE = 0x1473F19;
        public const uint UDP_PACKET_CHECKSUM_XOR_VALUE = 0x4F3816C3;
    }

    public enum LoginResult : int
    {
        Success = 0,
        AlreadyConnected,
        PmapCantLogin,
        PcCafe,
        Invalid,

    }

    public enum VerifyAccountResult : byte
    {
        SUCCESS = 0,
        WRONG_PASSWD,
        MULTIPLE_CONN_TRY,
        INVALID_GUILD,
        INVALID_PROTOCOL,
        BAD_PLAYER,
    }

    public static class ItemConstants
    {
        public static ItemType GetItemType(int i)
        {
            switch (i)
            {
                case 1: return ItemType.Character;
                case 2: return ItemType.Color;
                case 3: return ItemType.Kart;
                case 4: return ItemType.Plate;
                case 5: return ItemType.Goggle;
                case 6: return ItemType.Balloon;
                case 7: return ItemType.TempLicense;
                case 8: return ItemType.SlotChanger;
                case 14: return ItemType.Pet;
                default: return ItemType.None;
            }
        }
    }

    public class RoomConstants
    {
        public const int MAX_SLOTS = 16;
        public const int MAX_PLAYERS = 8;
    }

    public static class GameConstants
    {
        public const int GAME_WAIT_TIME = 42000;
        public const int GAME_START_DELAY = 7000;
        public const int GAME_END_DELAY = 10000;
        public const int GAME_RESULT_DELAY = 10000;

        public static int[] TEAM_POINT = new int[] { 0, 10, 8, 6, 5, 4, 3, 2, 1 };

        public static bool IsTeamGame(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.SpeedTeamGame:
                case GameMode.ItemTeamGame:
                case GameMode.FlagTeamGame:
                case GameMode.ClassicTeamGame:
                case GameMode.VsItemTeamGame:
                case GameMode.ChampionsItemTeamGame:
                case GameMode.SpeedPremiumTeam:
                case GameMode.SpeedNormalTeam:
                case GameMode.BokbulbokItemTeam:
                case GameMode.BokbulbokSpeedTeam:
                    return true;

                default:
                    return false;
            }
        }
    }
    
    public struct Version
    {
        public short Major { get; set; }
        public short Minor { get; set; }
        public Localisation Localisation { get; set; }
        public bool TestServer { get; set; }
    }

    public enum Localisation : short
    {
        Korea = 1,
        China = 3,
        Taiwan = 4,
        America = 5
    }

    public enum LoginResponse : int
    {
        Valid = 0,
        AlreadyLoggedIn = 1,
        Banned = 2,
        InternetCafe = 3,
        InvalidID = 4
    }

    public enum ItemType : short
    {
        None,
        Character,
        Color,
        Kart,
        Plate,
        TempLicense = 6,
        SlotChanger,
        Goggle,
        Balloon,
        SuperBoss,
        HeadBand,
        HeadPhone,
        Ticket,
        UpgradeKit,
        HandGearL = 16,
        HandGearR,
        Uniform,
        Decal = 20,
        Pet,
        InitialCard,
        Card,
        Lottery,
        QuestKey,
        Aura,
        SkidMark,
        RoomCard,
        LucciItem,
        SpecialKit,
        RidColor,
        RpLucciBonus,
        Jewel,
        BonusCard
    }

    public enum SpeedType : byte
    {
        Novice,
        Rookie,
        L3,
        L2,
        L1,
        Pro
    }

    public enum Team : byte
    {
        None,
        Red,
        Blue
    }

    public enum ChannelSwitchResponse : int
    {
        Success,
        GoToMain,
        Full,
        Fail,
        Fail2
    }

    public enum PlayerInfoStatus : byte
    {
        Offline,
        SinglePlay
    }

    public enum GameMode : int
    {
        SpeedIndiGame = 1,
        ItemIndiGame,
        SpeedTeamGame,
        ItemTeamGame,
        SpeedIndiGame2,
        ItemIndiGame2,
        FlagTeamGame,
        FlagIndiGame,
        ClassicTeamGame = 10,
        ClassicIndiGame,
        SpeedMatch = 13,
        ItemMatch,
        VsItemTeamGame = 17,
        ChampionsItemTeamGame,
        SpeedPremium,
        SpeedNormal,
        ItemPremium,
        ItemNormal,
        SpeedPremiumTeam,
        SpeedNormalTeam,
        ChallengeItemTeamGame,
        BokbulbokItemIndi,
        BokbulbokItemTeam,
        BokbulbokSpeedIndi,
        BokbulbokSpeedTeam,
    }

    public enum SlotState : int
    {
        None,
        Closed,
        NotReady,
        Ready,
        Observer,
        Setup
    }

    public enum TrackRibbon
    {
        Hot
    }

    public enum GameRoomStartResponse : int
    {
        Success,
        NotReady,
        NotEnoughPlayers,
        TeamsMismatch,
        NotRoomMaster
    }

    public enum GamePlayerStatus
    {
        Prepare,
        Finish = 2
    }

    public enum GameControlStatus
    {
        Start = 1,
        Middle = 3, // TODO: FIXME Find a better name for this.
        Finish = 4
    }

    public enum GarageEntranceResponse
    {
        Success,
        Fail
    }

    // TODO: FIXME Find a better name for this.
    public enum GarageSecedeResponse
    {
        Fail,
        Success
    }
}
