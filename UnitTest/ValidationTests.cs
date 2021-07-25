// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BacteriaMage.OgreBattle.ArmyTool.DataModel;
using BacteriaMage.OgreBattle.ArmyTool.GameSave;
using BacteriaMage.OgreBattle.Common;

using static BacteriaMage.OgreBattle.ArmyTool.GameSave.UnitPosition;
using static BacteriaMage.OgreBattle.ArmyTool.GameSave.UnitRow;
using static BacteriaMage.OgreBattle.UnitTest.CharacterStats;
using static BacteriaMage.OgreBattle.UnitTest.Classes;
using static BacteriaMage.OgreBattle.UnitTest.Names;

using Character = BacteriaMage.OgreBattle.ArmyTool.DataModel.Character;

namespace BacteriaMage.OgreBattle.UnitTest
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        [ExpectedException(typeof(OgreBattleException))]
        public void EmptyArmy_Exception()
        {
            ExecuteTest();
        }

        [TestMethod]
        public void ArmyTooLarge_Error()
        {
            AddOpinionLeader();
            AddDuplicateCharacters(Paula, AmazonLevel2, Constants.MaxCharacters);

            AssertOneError("too many");
        }

        [TestMethod]
        public void NoLord_Error()
        {
            AddCharacter(Karen, AmazonLevel2);

            AssertOneError("not found");
        }

        [TestMethod]
        public void MultipleLords_Error()
        {
            AddCharacter(Cotton, FighterLevel2);
            AddCharacter(Alyn, ClericLevel2);
            AddOpinionLeader("Dave", LordIainuki);
            AddCharacter(Aytorros, GryphonLevel2);
            AddOpinionLeader("Joe", LordThunder);

            AssertErrorContains("exactly one", 1);
        }

        [TestMethod]
        public void LordNotUnitLeader_Error()
        {
            AddCharacter(Alyn, ClericLevel2, 1, Back, Center, true);
            AddOpinionLeader("Sam", LordEvil, 1, Front, Center, false);

            AssertOneError("must be the leader");
        }

        [TestMethod]
        public void LordLeadsWrongUnit_Error()
        {
            AddOpinionLeader("Sally", LordHoly, 2);

            AssertOneError("unit 1");
        }

        [TestMethod]
        public void LordSpecialIdentity_Error()
        {
            AddOpinionLeader("Jamie", new CharacterStats(OpinionLeader)
            {
                Identity = 0x63,
            });

            AssertOneError("special identity");
        }

        [TestMethod]
        public void LargeUnitLeader_Error()
        {
            AddOpinionLeader();
            AddCharacter(Mauls, WizardLevel4, 2, Back, Center, false);
            AddCharacter(Kujkos, OctopusLevel2, 2, Front, Center, true);

            AssertOneError("cannot lead units");
        }

        [TestMethod]
        public void NoNameCharacter_Error()
        {
            AddOpinionLeader();
            AddCharacter(null, SamuraiLevel3);
            AddCharacter(Melanie, AmazonLevel2);

            AssertOneError("name is required");
        }

        [TestMethod]
        public void NoLordName_Error()
        {
            AddCharacter(Pizza, HellHoundLevel4);
            AddOpinionLeader("");

            AssertOneError("name is required");
        }

        [TestMethod]
        public void LordNameTooLong_Error()
        {
            AddOpinionLeader("Schwarzenegger");
            AddCharacter(Penia, GryphonLevel2);

            AssertOneError("too long");
        }

        [TestMethod]
        public void LordNameIllegalCharacter_Error()
        {
            AddOpinionLeader("Tilda~");

            AssertOneError("invalid characters");
        }

        [TestMethod]
        public void IdentityTooBig_Error()
        {
            AddOpinionLeader();
            AddCharacter(Sylphy, new CharacterStats(FighterLevel2)
            {
                Identity = 0x100,
            });

            AssertOneError("not a valid value");
        }

        [TestMethod]
        public void ClassMissing_Error()
        {
            AddOpinionLeader();
            AddCharacter(Alyn, new CharacterStats(ClericLevel2)
            {
                Class = null,
            });

            AssertOneError("is required");
        }

        [TestMethod]
        public void LevelTooSmall_Error()
        {
            AddOpinionLeader();
            AddCharacter(Burns, new CharacterStats(KnightLevel3)
            {
                Level = 0,
            });

            AssertOneError("not a valid value");
        }

        [TestMethod]
        public void HitPointsTooBig_Error()
        {
            AddOpinionLeader();
            AddCharacter(Satiros, new CharacterStats(WizardLevel4)
            {
                HitPoints = 1000,
            });

            AssertOneError("not a valid value");
        }

        [TestMethod]
        public void StrengthMissing_Error()
        {
            AddOpinionLeader();
            AddCharacter(Yohan, new CharacterStats(HellHoundLevel4)
            {
                Strength = null,
            });

            AssertOneError("is required");
        }

        [TestMethod]
        public void AgilityTooBig_Error()
        {
            AddOpinionLeader();
            AddCharacter(Ricky, new CharacterStats(GryphonLevel2)
            {
                Agility = 0x0100,
            });

            AssertOneError("not a valid value");
        }

        [TestMethod]
        public void IntelligenceTooSmall_Error()
        {
            AddOpinionLeader();
            AddCharacter(Ryukeios, new CharacterStats(OctopusLevel2)
            {
                Intelligence = -1,
            });

            AssertOneError("not a valid value");
        }

        [TestMethod]
        public void CharismaMissing_Error()
        {
            AddOpinionLeader();
            AddCharacter(Songas, new CharacterStats(SamuraiLevel3)
            {
                Charisma = null,
            });

            AssertOneError("is required");
        }

        [TestMethod]
        public void AlignmentTooSmall_Error()
        {
            AddOpinionLeader();
            AddCharacter(Helene, new CharacterStats(AmazonLevel2)
            {
                Alignment = -1,
            });

            AssertOneError("not a valid value");
        }

        [TestMethod]
        public void LuckTooBig_Error()
        {
            AddOpinionLeader();
            AddCharacter(Hilda, new CharacterStats(ClericLevel2)
            {
                Luck = 101,
            });

            AssertOneError("not a valid value");
        }

        [TestMethod]
        public void ExpTooBig_Error()
        {
            AddOpinionLeader();
            AddCharacter(Cotton, new CharacterStats(FighterLevel2)
            {
                Exp = 100,
            });

            AssertOneError("not a valid value");
        }

        [TestMethod]
        public void SalaryTooBig_Error()
        {
            AddOpinionLeader();
            AddCharacter(Demetel, new CharacterStats(KnightLevel3)
            {
                Salary = 0x10000,
            });

            AssertOneError("not a valid value");
        }

        [TestMethod]
        public void EquippedItemTooBig_Error()
        {
            AddOpinionLeader();
            AddCharacter(Calkas, new CharacterStats(SamuraiLevel3)
            {
                EquippedItem = 0x0100,
            });

            AssertOneError("not a valid value");
        }

        [TestMethod]
        public void NonUndeadWithZeroHealth_Error()
        {
            AddOpinionLeader();
            AddCharacter(Dilkay, new CharacterStats(WizardLevel4)
            {
                HitPoints = 0,
            });
        
            AssertOneError("must be greater than zero");
        }

        [TestMethod]
        public void UnitTooBig_Error()
        {
            AddOpinionLeader();
            AddCharacter(Bakis, FighterLevel2, 2, Front, RightFlank);
            AddCharacter(Burns, FighterLevel2, 2, Front, Center);
            AddCharacter(Calkas, FighterLevel2, 2, Front, LeftFlank);
            AddCharacter(Marbelik, ClericLevel2, 2, Back, RightFlank);
            AddCharacter(Moses, KnightLevel3, 2, Back, Center, true);
            AddCharacter(Pyryura, ClericLevel2, 2, Back, LeftFlank);

            AssertOneError("too many characters");
        }

        [TestMethod]
        public void UnitHasNoLeader_Error()
        {
            AddOpinionLeader();
            AddCharacter(Pizza, SamuraiLevel3, 2, Back, Center);

            AssertOneError("no unit leader");
        }

        [TestMethod]
        public void UnitMultipleLeaders_Error()
        {
            AddOpinionLeader();
            AddCharacter(Ricky, KnightLevel3, 1, Front, Center, true);

            AssertOneError("more than one leader");
        }

        [TestMethod]
        public void UnitTooManyLargeCharacters_Error()
        {
            AddOpinionLeader();
            AddCharacter(Satiros, GryphonLevel2, 3, Front, RightFlank);
            AddCharacter(Ryukeios, GryphonLevel2, 3, Front, Center);
            AddCharacter(Takeda, GryphonLevel2, 3, Front, LeftFlank);
            AddCharacter(Sylphy, WizardLevel4, 3, Back, Center, true);

            AssertOneError("too many large characters");
        }

        [TestMethod]
        public void UnitTooBigWithOneLarge_Error()
        {
            AddOpinionLeader();
            AddCharacter(Karen, AmazonLevel2, 4, Front, RightFlank);
            AddCharacter(Paula, AmazonLevel2, 4, Front, Center);
            AddCharacter(Melanie, AmazonLevel2, 4, Front, LeftFlank);
            AddCharacter(Kujkos, GryphonLevel2, 4, Back, RightFlank);
            AddCharacter(Hilda, ClericLevel2, 4, Back, LeftCenter, true);

            AssertOneError("limit is 4");
        }

        [TestMethod]
        public void UnitTooBigWithTwoLarge_Error()
        {
            AddOpinionLeader();
            AddCharacter(Marbelik, HellHoundLevel4, 20, Front, RightCenter);
            AddCharacter(Songas, HellHoundLevel4, 20, Front, LeftCenter);
            AddCharacter(Yohan, WizardLevel4, 20, Back, RightFlank);
            AddCharacter(Burns, WizardLevel4, 20, Back, LeftFlank, true);

            AssertOneError("limit is 3");
        }

        [TestMethod]
        public void UnitCharactersInSameSlot_Error()
        {
            AddOpinionLeader();
            AddCharacter(Calkas, SamuraiLevel3, 1, Front, Center);
            AddCharacter(Cotton, KnightLevel3, 1, Front, Center);

            AssertOneError("position overlaps");
        }

        [TestMethod]
        public void UnitCharactersInConflictingSlots_Error()
        {
            AddOpinionLeader();
            AddCharacter(Calkas, KnightLevel3, 10, Back, Center, true);
            AddCharacter(Aytorros, GryphonLevel2, 10, Front, Center);
            AddCharacter(Helene, ClericLevel2, 10, Back, RightCenter);
            AddCharacter(Mauls, WizardLevel4, 10, Back, LeftCenter);

            AssertErrorCount(2);
            AssertErrorContains("position overlaps", 2);
        }

        [TestMethod]
        public void UnitRowIsMissing_Error()
        {
            AddOpinionLeader();
            AddCharacter(Ryukeios, ClericLevel2, 25, null, Center, true);

            AssertOneError("row is required");
        }

        [TestMethod]
        public void UnitPositionIsMissing_Error()
        {
            AddCharacter(Sylphy, WizardLevel4, 1, Back, null);
            AddOpinionLeader();

            AssertOneError("position is required");
        }

        [TestMethod]
        public void UnitInvalidUnitNumber_Error()
        {
            AddCharacter(Ricky, SamuraiLevel3, 0, Front, Center);
            AddOpinionLeader();
            AddCharacter(Penia, ClericLevel2, 26, Back, RightCenter);

            AssertErrorCount(2);
            AssertErrorContains("not a valid unit number", 2);
        }

        [TestMethod]
        public void UnitValueUnexpected_Error()
        {
            AddOpinionLeader();
            AddCharacter(Alyn, SamuraiLevel3, 3, Front, RightFlank, false);
            AddCharacter(Bakis, SamuraiLevel3, 3, Front, Center, true);
            AddCharacter(Marbelik, SamuraiLevel3, null, Front, LeftFlank, false);
            AddCharacter(Atise, ClericLevel2, 3, Back, RightCenter, false);

            AssertErrorCount(3);
            AssertErrorContains("without a unit number", 3);
        }

        [TestMethod]
        public void LeaderWithoutUnit_Error()
        {
            AddCharacter(Atise, KnightLevel3, null, null, null, true);
            AddOpinionLeader();
            
            AssertOneError("leader must be blank");
        }

        [TestMethod]
        public void LordOnly_NoError()
        {
            AddOpinionLeader();

            AssertSuccess();
        }

        [TestMethod]
        public void UndeadWithZeroHP_NoError()
        {
            AddOpinionLeader();
            AddCharacter(Ricky, GhostLevel3);

            AssertSuccess();
        }

        [TestMethod]
        public void CompleteArmy_NoError()
        {
            AddCharacter(Cotton, FighterLevel2, 1, Front, RightFlank);
            AddCharacter(Kaukon, FighterLevel2, 1, Front, Center);
            AddCharacter(Sylphy, FighterLevel2, 1, Front, LeftFlank);
            AddCharacter(Kaukon, WizardLevel4, 1, Back, LeftFlank);
            AddOpinionLeader();

            AddCharacter(Kujkos, GryphonLevel2, 2, Front, RightFlank);
            AddCharacter(Moses, KnightLevel3, 2, Front, LeftFlank, true);
            AddCharacter(Marbelik, GryphonLevel2, 2, Back, Center);

            AddCharacter(Ryukeios, HellHoundLevel4, 3, Front, Center);
            AddCharacter(Marbelik, AmazonLevel2, 3, Back, RightFlank);
            AddCharacter(Penia, ClericLevel2, 3, Back, Center, true);
            AddCharacter(Helene, AmazonLevel2, 3, Back, LeftFlank);

            AddCharacter(Demetel, OctopusLevel2);
            AddCharacter(Bakis, SamuraiLevel3);
            AddCharacter(Huamos, KnightLevel3);

            AssertSuccess();
        }

        #region Assert methods

        public void AssertSuccess()
        {
            ExecuteTest();

            Assert.IsTrue(Writer.Success);
            Assert.IsTrue(Errors.Count == 0);
        }

        public void AssertFailed()
        {
            ExecuteTest();

            Assert.IsFalse(Writer.Success);
            Assert.IsFalse(Errors.Count == 0);
        }

        public void AssertErrorCount(int count)
        {
            ExecuteTest();

            Assert.AreEqual((Errors.Count == 0), Writer.Success);
            Assert.AreEqual(count, Errors.Count);
        }

        public void AssertErrorContains(string text, int times)
        {
            ExecuteTest();

            int matches = Errors.Count((message) => message.Contains(text.ToLowerInvariant()));

            Assert.AreEqual(times, matches);
        }

        public void AssertOneError(string text)
        {
            AssertErrorCount(1);
            AssertErrorContains(text, 1);
        }

        #endregion

        #region Test infrastructure

        private TableProvider Tables { get; set; }
        private Characters Characters { get; set; }
        private SaveRam SaveRam { get; set; }
        private Slot Slot { get; set; }
        private SlotWriter Writer { get; set; }
        private List<string> Errors { get; set; }
        private bool TestHasRun { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Tables = FakeTables.BuildTableProvider();
            Characters = new Characters(Tables);
            SaveRam = new SaveRam();
            Slot = SaveRam.Slot2;
            Writer = new SlotWriter(Characters, Slot);
            Errors = new List<string>();
            TestHasRun = false;

            Writer.OnErrorMessage += OnWriterError;
        }

        private void ExecuteTest()
        {
            if (!TestHasRun)
            {
                try
                {
                    Writer.WriteCharacters();
                }
                finally
                {
                    TestHasRun = true;
                }
            }
        }

        private void OnWriterError(object sender, MessageEventArgs e)
        {
            if (e.Line == null)
            {
                Console.WriteLine(e.Message);
            }
            else
            {
                Console.WriteLine("(Row {0}) {1}", e.Line, e.Message);
            }

            Errors.Add(e.Message.ToLowerInvariant());
        }

        #endregion

        #region Character creation methods

        private void AddOpinionLeader()
        {
            AddOpinionLeader("DESTIN");
        }

        private void AddOpinionLeader(int? classId)
        {
            AddOpinionLeader("DESTIN", classId);
        }

        private void AddOpinionLeader(string nameText)
        {
            AddOpinionLeader(nameText, OpinionLeader);
        }

        private void AddOpinionLeader(string nameText, int? classId, int? unit = 1, UnitRow? row = Back, UnitPosition? position = Center, bool? leader = true)
        {
            CharacterStats stats = new CharacterStats(OpinionLeader) { Class = classId };

            AddOpinionLeader(nameText, stats, unit, row, position, leader);
        }

        private void AddOpinionLeader(string nameText, CharacterStats stats, int? unit = 1, UnitRow? row = Back, UnitPosition? position = Center, bool? leader = true)
        {
            AddCharacter(null, nameText, stats, unit, row, position, leader);
        }

        private void AddCharacter(int? nameId, CharacterStats stats, int? unit = null, UnitRow? row = null, UnitPosition? position = null, bool? leader = null)
        {
            string name = (nameId == null ? string.Empty : $"0x{nameId:X4}");

            AddCharacter(nameId, name, stats, unit, row, position, leader);
        }

        private void AddCharacter(int? nameId, string nameText, CharacterStats stats, int? unit, UnitRow? row, UnitPosition? position, bool? leader)
        {
            Character character = new Character(Tables)
            {
                LineNumber = Characters.Count + 2,
                Number = Characters.Count + 1,
                Name = new CharacterName() { NameId = nameId, NameText = nameText },
                Identity = stats.Identity,
                Class = stats.Class,
                Level = stats.Level,
                HitPoints = stats.HitPoints,
                Strength = stats.Strength,
                Agility = stats.Agility,
                Intelligence = stats.Intelligence,
                Charisma = stats.Charisma,
                Alignment = stats.Alignment,
                Luck = stats.Luck,
                Exp = stats.Exp,
                Salary = stats.Salary,
                EquippedItem = stats.EquippedItem,
                Unit = unit,
                UnitRow = row,
                UnitPosition = position,
                UnitLeader = leader,
            };

            Characters.Add(character);
        }

        private void AddDuplicateCharacters(int? nameId, CharacterStats stats, int times)
        {
            for (int i = 0; i < times; i++)
            {
                AddCharacter(nameId, stats);
            }
        }

        #endregion
    }
}
