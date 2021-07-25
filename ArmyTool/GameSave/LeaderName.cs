// bacteriamage.wordpress.com

using System.Text;
using System.Text.RegularExpressions;
using BacteriaMage.OgreBattle.Common;
using BacteriaMage.OgreBattle.MemorySpan;

using static BacteriaMage.OgreBattle.ArmyTool.GameSave.Constants;

namespace BacteriaMage.OgreBattle.ArmyTool.GameSave
{
    internal class LeaderName
    {
        private readonly ByteSpan _nameSpan;

        public LeaderName(ByteSpan slotSpan)
        {
            _nameSpan = slotSpan.Slice(LeaderNameOffset, LeaderNameSize);
        }

        #region Decode to string

        public string GetString()
        {
            string name = Encoding.ASCII.GetString(GetNormalizedBytes(), 0, ComputeNameLength());

            if (NameContainsIllegalCharacter(name))
            {
                // if the name contains junk then just return nothing
                return string.Empty;
            }

            return name;
        }

        private byte[] GetNormalizedBytes()
        {
            byte[] bytes = _nameSpan.ToArray();

            ForceSevenBitAsciiBytes(bytes);

            return bytes;
        }

        private void ForceSevenBitAsciiBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] & 0x7f);
            }
        }

        private int ComputeNameLength()
        {
            for (int index = 0; index < LeaderNameSize; index++)
            {
                if (_nameSpan[index] == 0x00)
                {
                    // null terminated so length is the position of the first zero
                    return index;
                }
            }

            // no zero so maximum length
            return LeaderNameSize;
        }

        #endregion

        #region Encode from string

        public void SetString(string leaderName)
        {
            if (!TrySetString(leaderName, out string errorMessage))
            {
                throw new OgreBattleException(errorMessage);
            }
        }

        public bool TrySetString(string leaderName, out string errorMessage)
        {
            leaderName = NormalizeNameString(leaderName);

            if (NameTooLong(leaderName))
            {
                errorMessage = $"Name is too long; maximum length is {LeaderNameSize} characters.";
                return false;
            }
            if (NameContainsIllegalCharacter(leaderName))
            {
                errorMessage = "Name contains invalid characters; A-Z, 0-9, space, and /-=!?&+%,.: are allowed.";
                return false;
            }

            _nameSpan.Fill(0x00);
            _nameSpan.Fill(GetNameAsciiBytes(leaderName));

            errorMessage = null;
            return true;
        }

        private static string NormalizeNameString(string name)
        {
            return name.TrimEnd().ToUpperInvariant();
        }

        private static bool NameTooLong(string name)
        {
            return name.Length > LeaderNameSize;
        }

        private static bool NameContainsIllegalCharacter(string name)
        {
            Regex valid = new Regex(@"^[0-9A-Z =!&%/,:\-\?\+\.]+$");

            name = name.TrimEnd().ToUpperInvariant();

            return !valid.IsMatch(name);
        }

        private static byte[] GetNameAsciiBytes(string name)
        {
            return Encoding.ASCII.GetBytes(name);
        }

        #endregion
    }
}
