// bacteriamage.wordpress.com

using System;

namespace BacteriaMage.OgreBattle.ArmyTool.DataModel
{
    public enum MessageMagnitude
    {
        Error = 1,
        Note = 2,
    }

    public class MessageEventArgs : EventArgs
    {
        public MessageMagnitude Magnitude
        {
            get;
            private set;
        }

        public Character Character
        {
            get;
            private set;
        }

        public int? Line
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }

        public static MessageEventArgs Error(string message)
        {
            return new MessageEventArgs(MessageMagnitude.Error, message);
        }

        public static MessageEventArgs Error(Character character, string message)
        {
            return new MessageEventArgs(MessageMagnitude.Error, character, message);
        }

        public static MessageEventArgs Note(string message)
        {
            return new MessageEventArgs(MessageMagnitude.Note, message);
        }

        public static MessageEventArgs Note(Character character, string message)
        {
            return new MessageEventArgs(MessageMagnitude.Note, character, message);
        }

        public MessageEventArgs(MessageMagnitude magnitude, Character character, string message)
            : this(magnitude, message)
        {
            Line = character?.LineNumber;
            Character = character;
        }

        public MessageEventArgs(MessageMagnitude magnitude, string message)
        {
            Magnitude = magnitude;
            Message = message;
        }
    }
}
