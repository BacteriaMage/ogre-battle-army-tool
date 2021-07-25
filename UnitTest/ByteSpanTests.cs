// bacteriamage.wordpress.com

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BacteriaMage.OgreBattle.MemorySpan;

namespace BacteriaMage.OgreBattle.UnitTest
{
    [TestClass]
    public class ByteSpanTests
    {
        #region Constructor tests

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ByteSpan_NullBytes_Exception()
        {
            new ByteSpan(null);
        }

        [TestMethod]
        public void ByteSpan_EmptySpan_EmptySlice()
        {
            CreateByteSpan(0);
            CreateResultArray(0, 0);
            AssertResultEqual();
        }

        [TestMethod]
        public void ByteSpan_FullSpan_EqualsFullSlice()
        {
            CreateByteSpan(5);
            CreateResultArray(0, 5);
            AssertResultEqual();
        }
        
        #endregion

        #region Slice tests

        [TestMethod]
        public void Slice_MiddleSlice_EqualsSubset()
        {
            CreateByteSpan(10);
            CreateResultArray(1, 8);

            byteSpan = byteSpan.Slice(1, 8);

            AssertResultEqual();
        }

        [TestMethod]
        public void Slice_LeftSlice_EqualsSubset()
        {
            CreateByteSpan(10);
            CreateResultArray(0, 5);

            byteSpan = byteSpan.Slice(0, 5);

            AssertResultEqual();
        }

        [TestMethod]
        public void Slice_RightSlice_EqualsSubset()
        {
            CreateByteSpan(10);
            CreateResultArray(5, 5);

            byteSpan = byteSpan.Slice(5, 5);

            AssertResultEqual();
        }

        [TestMethod]
        public void Slice_DoubleSlice_EqualsSubset()
        {
            CreateByteSpan(10);
            CreateResultArray(2, 6);

            byteSpan = byteSpan.Slice(1, 8);
            byteSpan = byteSpan.Slice(1, 6);

            AssertResultEqual();
        }

        [TestMethod]
        public void Slice_EmptySlice_EmptySlice()
        {
            CreateByteSpan(5);
            CreateResultArray(0, 0);

            byteSpan = byteSpan.Slice(0, 0);

            AssertResultEqual();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Slice_NegativeOffset_Exception()
        {
            CreateByteSpan(5);

            byteSpan.Slice(-1, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Slice_SliceTooLong_Exception()
        {
            CreateByteSpan(10);

            byteSpan.Slice(8, 3);
        }

        #endregion

        #region Index tests

        [TestMethod]
        public void Index_GetIndex_ExpectedValue()
        {
            CreateByteSpan(6);

            int expected = 3;
            int actual = byteSpan[3];

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Index_GetNegativeIndex_Exception()
        {
            CreateByteSpan(6);

            int actual = byteSpan[-1];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Index_GetIndexPastEnd_Exception()
        {
            CreateByteSpan(6);

            int actual = byteSpan[6];
        }

        [TestMethod]
        public void Index_SetIndex_ResultsMatch()
        {
            CreateByteSpan(6);
            CreateResultArray(0, 6);

            byteSpan[2] = 99;
            resultArray[2] = 99;

            AssertResultEqual();
        }

        [TestMethod]
        public void Index_SetIndexOverflowValue_AutoCastToByte()
        {
            CreateByteSpan(6);
            CreateResultArray(0, 6);

            byteSpan[2] = 0x0101;
            resultArray[2] = 0x0001;

            AssertResultEqual();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Index_SetNegativeIndex_Exception()
        {
            CreateByteSpan(6);

            byteSpan[-1] = 7;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Index_SetIndexPastEnd_Exception()
        {
            CreateByteSpan(6);

            byteSpan[6] = 7;
        }

        #endregion

        #region Length tests

        [TestMethod]
        public void Length_EmptySpan_ZeroLength()
        {
            CreateByteSpan(0);

            int expected = 0;
            int actual = byteSpan.Length;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Length_NonEmptySpan_NonZeroLength()
        {
            CreateByteSpan(4);

            int expected = 4;
            int actual = byteSpan.Length;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Length_SlicedSpan_SubsetLength()
        {
            CreateByteSpan(10);
            
            int expected = 4;
            int actual = byteSpan.Slice(3, 4).Length;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Test Helpers

        private ByteSpan byteSpan;

        private byte[] resultArray;

        private void CreateByteSpan(int length)
        {
            byte[] bytes = CreateSubsetArray(0, length);

            byteSpan = new ByteSpan(bytes);
        }

        private void CreateResultArray(int startIndex, int length)
        {
            resultArray = CreateSubsetArray(startIndex, length);
        }

        private void AssertResultEqual()
        {
            byte[] spanArray = byteSpan.ToArray();

            CollectionAssert.AreEqual(resultArray, spanArray);
        }

        private byte[] CreateSubsetArray(int startIndex, int length)
        {
            byte[] bytes = new byte[length];

            for (byte i = 0; i < length; i++)
            {
                bytes[i] = (byte)(startIndex + i);
            }

            return bytes;
        }

        #endregion
    }
}
