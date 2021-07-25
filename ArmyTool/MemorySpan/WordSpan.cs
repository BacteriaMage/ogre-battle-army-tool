// bacteriamage.wordpress.com

using System;

namespace BacteriaMage.OgreBattle.MemorySpan
{
    public class WordSpan
    {
        private const int WordSize = 2;

        private ByteSpan byteSpan;

        public WordSpan(ByteSpan byteSpan)
        {
            this.byteSpan = byteSpan ?? throw new NullReferenceException();

            if(byteSpan.Length % WordSize != 0)
            {
                throw new ArgumentException($"Length of ByteSpan passed to a new WordSpan must be divisible by {WordSize}", nameof(byteSpan));
            }
        }

        public int this[int index]
        {
            get
            {
                return byteSpan.GetWordAt(index * WordSize);
            }
            set
            {
                byteSpan.SetWordAt(index * WordSize, value);
            }
        }

        public WordSpan Slice(int startWord, int words)
        {
            return Slice(byteSpan, startWord * WordSize, words);
        }

        public static WordSpan Slice(ByteSpan byteSpan, int byteOffset, int wordsSize)
        {
            ByteSpan byteSpanForWordSpan = byteSpan.Slice(byteOffset, wordsSize * WordSize);

            return new WordSpan(byteSpanForWordSpan);
        }
    }
}
