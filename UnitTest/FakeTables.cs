// bacteriamage.wordpress.com

using BacteriaMage.OgreBattle.ArmyTool.DataModel;
using BacteriaMage.OgreBattle.ArmyTool.Metadata;
using BacteriaMage.OgreBattle.NamedValues;

using static BacteriaMage.OgreBattle.ArmyTool.Metadata.ClassTypes;
using static BacteriaMage.OgreBattle.UnitTest.Classes;

namespace BacteriaMage.OgreBattle.UnitTest
{
    public static class FakeTables
    {
        public static TableProvider BuildTableProvider()
        {
            return new TableProvider()
            {
                NamesTable = NewGenericTable(),
                ItemsTable = NewGenericTable(),
                IdentitiesTable = NewGenericTable(),
                ClassesTable = NewClassesTable(),
            };
        }

        private static ValuesTable NewGenericTable()
        {
            return new ValuesTable<TableEntry>();
        }

        private static ClassesTable NewClassesTable()
        {
            ClassesTable table = new ClassesTable();

            table.Add(Fighter, nameof(Fighter), Male, false, 2, 1);
            table.Add(Knight, nameof(Knight), Male, true, 2, 1);
            table.Add(Samurai, nameof(Samurai), Male, true, 2, 1);
            table.Add(Amazon, nameof(Amazon), Female, false, 1, 2);
            table.Add(Wizard, nameof(Wizard), Male, true, 2, 2);
            table.Add(Cleric, nameof(Cleric), Female, true, 2, 2);
            table.Add(HellHound, nameof(HellHound), Large, false, 3, 2);
            table.Add(Octopus, nameof(Octopus), Large, false, 4, 2);
            table.Add(Gryphon, nameof(Gryphon), Large, false, 2, 1);
            table.Add(LordIainuki, nameof(LordIainuki), Lord, true, 3, 1);
            table.Add(LordEvil, nameof(LordEvil), Lord, true, 2, 1);
            table.Add(LordHoly, nameof(LordHoly), Lord, true, 2, 1);
            table.Add(LordThunder, nameof(LordThunder), Lord, true, 2, 1);

            table.AddUndead(Ghost, nameof(Ghost), 1, 2);

            return table;
        }

        private static void Add(this ClassesTable table, int id, string name, ClassTypes type, bool canLead, int frontTurns, int backTurns)
        {
            table.Add(new ClassEntry(id, name)
            {
                ClassType = type,
                UnitFlag = ComputeFlag(type),
                CanLeadUnit = canLead,
                FrontRowTurns = frontTurns,
                BackRowTurns = backTurns,
            });
        }

        private static void AddUndead(this ClassesTable table, int id, string name, int frontTurns, int backTurns)
        {
            table.Add(new ClassEntry(id, name)
            {
                ClassType = Male,
                UnitFlag = UnitFlags.Undead,
                CanLeadUnit = false,
                FrontRowTurns = frontTurns,
                BackRowTurns = backTurns,
            });
        }

        private static UnitFlags? ComputeFlag(ClassTypes type)
        {
            switch(type)
            {
                case ClassTypes.Large:
                    return UnitFlags.Large;
                default:
                    return null;
            }
        }
    }
}
