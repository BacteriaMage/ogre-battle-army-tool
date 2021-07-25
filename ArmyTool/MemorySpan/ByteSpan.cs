// bacteriamage.wordpress.com

using System;

namespace BacteriaMage.OgreBattle.MemorySpan
{
    /// <summary>
    /// Wrapper for a byte array that allows slicing to subset ranges. All slices share the same array reference so
    /// changes to the parent span affect the child and visa versa. Each new subset span is zero indexed on the start of
    /// the span and indexes outside the range of the specific span are not accessible using that span. This is similar
    /// to the Span object introduced in .NET Core 2.0 but not available on earlier .NET versions.
    /// </summary>
    public class ByteSpan
    {
        private readonly byte[] bytes;
        private readonly int start;
        private readonly int end;

        /// <summary>
        /// Create a span for entire byte array.
        /// </summary>
        public ByteSpan(byte[] bytes)
        {
            this.bytes = bytes ?? throw new NullReferenceException();

            start = 0;
            end = bytes.Length - 1;
        }

        /// <summary>
        /// Create a span for a subset of a byte array.
        /// </summary>
        private ByteSpan(byte[] bytes, int start, int end)
        {
            this.bytes = bytes;
            this.start = start;
            this.end = end;
        }

        /// <summary>
        /// Lenght of the span in bytes.
        /// </summary>
        public int Length
        {
            get => end - start + 1;
        }

        /// <summary>
        /// Access an index within the span where the zero-index is the start of the span.
        /// </summary>
        public int this[int index]
        {
            get
            {
                ValidateIndex(index);

                return bytes[start + index];
            }
            set
            {
                ValidateIndex(index);

                bytes[start + index] = (byte)value;
            }
        }

        /// <summary>
        /// Return the little endian 16-bit word located a specific index in the span.
        /// </summary>
        public int GetWordAt(int index)
        {
            return this[index] + (this[index + 1] * 0x100);
        }

        /// <summary>
        /// Store a little endian 16-bit word at a specific index in the span.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetWordAt(int index, int value)
        {
            ushort word = (ushort)value;

            ValidateIndex(index);
            ValidateIndex(index + 1);

            this[index] = word;
            this[index + 1] = (word >> 8);
        }

        /// <summary>
        /// Make sure an index is within the span range (using the span's index positions).
        /// </summary>
        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Create a new zero-indexed span which is a subset of the current span.
        /// </summary>
        public ByteSpan Slice(int start, int length)
        {
            int parentStart = this.start;
            int parentEnd = end;

            int sliceStart = parentStart + start;
            int sliceEnd = sliceStart + length - 1;

            if (sliceStart < parentStart || sliceEnd > parentEnd || length < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            return new ByteSpan(bytes, sliceStart, sliceEnd);
        }

        /// <summary>
        /// Set every byte in the span to the specified value.
        /// </summary>
        public void Fill(int value)
        {
            for (int index = start; index <= end; index++)
            {
                bytes[index] = (byte)value;
            }
        }

        /// <summary>
        /// Copy the bytes of the array into the span.
        /// </summary>
        public void Fill(byte[] array)
        {
            if (array.Length > 0)
            {
                ValidateIndex(array.Length - 1);

                Array.Copy(array, 0, bytes, start, array.Length);
            }
        }

        /// <summary>
        /// Copy the span to a new byte array.
        /// </summary>
        public byte[] ToArray()
        {
            byte[] array = new byte[Length];

            Array.Copy(bytes, start, array, 0, Length);

            return array;
        }
    }
}
