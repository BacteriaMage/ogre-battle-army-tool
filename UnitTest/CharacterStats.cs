// bacteriamage.wordpress.com

namespace BacteriaMage.OgreBattle.UnitTest
{
    public struct CharacterStats
    {
        public int? Identity;
        public int? Class;
        public int? Level;
        public int? HitPoints;
        public int? Strength;
        public int? Agility;
        public int? Intelligence;
        public int? Charisma;
        public int? Alignment;
        public int? Luck;
        public int? Exp;
        public int? Salary;
        public int? EquippedItem;

        public CharacterStats(CharacterStats source)
        {
            Identity = source.Identity;
            Class = source.Class;
            Level = source.Level;
            HitPoints = source.HitPoints;
            Strength = source.Strength;
            Agility = source.Agility;
            Intelligence = source.Intelligence;
            Charisma = source.Charisma;
            Alignment = source.Alignment;
            Luck = source.Luck;
            Exp = source.Exp;
            Salary = source.Salary;
            EquippedItem = source.EquippedItem;
        }

        public static readonly CharacterStats OpinionLeader = new CharacterStats()
        {
            Class = Classes.LordHoly,
            Level = 1,
            HitPoints = 81,
            Strength = 43,
            Agility = 57,
            Intelligence =54,
            Charisma = 64,
            Alignment = 72,
            Luck = 48,
            Exp = 0,
            Salary = 0,
        };

        public static readonly CharacterStats FighterLevel2 = new CharacterStats()
        {
            Class = Classes.Fighter,
            Level = 2,
            HitPoints = 86,
            Strength = 47,
            Agility = 49,
            Intelligence = 43,
            Charisma = 48,
            Alignment = 50,
            Luck = 45,
            Exp = 0,
            Salary = 120,
        };

        public static readonly CharacterStats KnightLevel3 = new CharacterStats()
        {
            Class = Classes.Knight,
            Level = 3,
            HitPoints = 89,
            Strength = 55,
            Agility = 57,
            Intelligence = 46,
            Charisma = 55,
            Alignment = 62,
            Luck = 50,
            Exp = 0,
            Salary = 780,
        };

        public static readonly CharacterStats SamuraiLevel3 = new CharacterStats()
        {
            Class = Classes.Samurai,
            Level = 3,
            HitPoints = 96,
            Strength = 56,
            Agility = 58,
            Intelligence = 55,
            Charisma = 56,
            Alignment = 61,
            Luck = 47,
            Exp = 0,
            Salary = 900,
        };

        public static readonly CharacterStats WizardLevel4 = new CharacterStats()
        {
            Class = Classes.Wizard,
            Level = 4,
            HitPoints = 96,
            Strength = 40,
            Agility = 54,
            Intelligence = 58,
            Charisma = 50,
            Alignment = 45,
            Luck = 50,
            Exp = 0,
            Salary = 610,
        };

        public static readonly CharacterStats AmazonLevel2 = new CharacterStats()
        {
            Class = Classes.Amazon,
            Level = 2,
            HitPoints = 88,
            Strength = 46,
            Agility = 54,
            Intelligence = 54,
            Charisma = 46,
            Alignment = 50,
            Luck = 48,
            Exp = 0,
            Salary = 130,
        };

        public static readonly CharacterStats ClericLevel2 = new CharacterStats()
        {
            Class = Classes.Cleric,
            Level = 2,
            HitPoints = 93,
            Strength = 43,
            Agility = 55,
            Intelligence = 55,
            Charisma = 59,
            Alignment = 65,
            Luck = 49,
            Exp = 0,
            Salary = 260,
        };

        public static readonly CharacterStats GhostLevel3 = new CharacterStats()
        {
            Class = Classes.Ghost,
            Level = 3,
            HitPoints = 0,
            Strength = 51,
            Agility = 47,
            Intelligence = 53,
            Charisma = 37,
            Alignment = 0,
            Luck = 49,
            Exp = 50,
            Salary = 160,
        };

        public static readonly CharacterStats HellHoundLevel4 = new CharacterStats()
        {
            Class = Classes.HellHound,
            Level = 4,
            HitPoints = 105,
            Strength = 61,
            Agility = 69,
            Intelligence = 42,
            Charisma = 17,
            Alignment = 44,
            Luck = 51,
            Exp = 0,
            Salary = 190,
        };

        public static readonly CharacterStats GryphonLevel2 = new CharacterStats()
        {
            Class = Classes.Gryphon,
            Level = 2,
            HitPoints = 87,
            Strength = 54,
            Agility = 63,
            Intelligence = 40,
            Charisma = 30,
            Alignment = 45,
            Luck = 53,
            Exp = 0,
            Salary = 250,
        };

        public static readonly CharacterStats OctopusLevel2 = new CharacterStats()
        {
            Class = Classes.Octopus,
            Level = 2,
            HitPoints = 88,
            Strength = 54,
            Agility = 42,
            Intelligence = 50,
            Charisma = 20,
            Alignment = 50,
            Luck = 50,
            Exp = 0,
            Salary = 350,
        };
    }

    public static class Names
    {
        public const int Alyn = 0x9C83;
        public const int Atise = 0xAA32;
        public const int Aytorros = 0xA9C6;
        public const int Bakis = 0xB1B8;
        public const int Burns = 0x95C0;
        public const int Calkas = 0xAD1F;
        public const int Cotton = 0x9F47;
        public const int Demetel = 0xA2B1;
        public const int Dilkay = 0xAFFB;
        public const int Helene = 0xA4C4;
        public const int Hilda = 0xA3E3;
        public const int Huamos = 0xB27E;
        public const int Karen = 0x9E54;
        public const int Kaukon = 0xAC9E;
        public const int Kujkos = 0xAD5C;
        public const int Marbelik = 0x9888;
        public const int Mauls = 0xB5F1;
        public const int Melanie = 0xA4F9;
        public const int Moses = 0x996C;
        public const int Paula = 0xA54F;
        public const int Penia = 0xB401;
        public const int Pizza = 0x96C2;
        public const int Pyryura = 0xB2CB;
        public const int Ricky = 0xA78D;
        public const int Ryukeios = 0xB874;
        public const int Satiros = 0xAE34;
        public const int Songas = 0x933E;
        public const int Sylphy = 0xA06C;
        public const int Takeda = 0x95B9;
        public const int Yohan = 0x99CC;
    }

    public static class Classes
    {
        public const int Fighter = 0x01;
        public const int Knight = 0x02;
        public const int Samurai = 0x06;
        public const int Amazon = 0x0A;
        public const int Wizard = 0x13;
        public const int Cleric = 0x18;
        public const int HellHound = 0x2C;
        public const int Octopus = 0x2E;
        public const int Ghost = 0x3C;
        public const int Gryphon = 0x4A;
        public const int LordIainuki = 0x4E;
        public const int LordEvil = 0x4F;
        public const int LordHoly = 0x50;
        public const int LordThunder = 0x51;
    }
}
