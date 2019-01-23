using System;
using System.IO;
using Lib.Hash.Hashable.ByteExtractors;
using Lib.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Lib.Hash.Hashable.Test
{
    /// <summary>
    /// Indirect test, <see cref="PrimitiveByteExtractor"/> is internal, but <see cref="BaseTypesByteExtractor"/> should contain it.
    /// </summary>
    [TestClass]
    public class PrimitiveByteExtractorTest
    {
        private IByteExtractor Extractor => new PrimitiveByteExtractor();

        [TestMethod]
        public void Bool_ExtractedAs_Bytes()
        {
            Check.That(ExtractBytes(true)).Equals(BitConverter.GetBytes(true));
            Check.That(ExtractBytes(false)).Equals(BitConverter.GetBytes(false));
            Check.That(ExtractBytes(false)).Not.Equals(BitConverter.GetBytes(true));
        }

        [TestMethod]
        public void SByte_ExtractedAs_Bytes()
        {
            Check.That(ExtractBytes(sbyte.MinValue)).Equals(BitConverter.GetBytes(sbyte.MinValue));
            Check.That(ExtractBytes(sbyte.MaxValue)).Equals(BitConverter.GetBytes(sbyte.MaxValue));
            Check.That(ExtractBytes(sbyte.MinValue)).Not.Equals(BitConverter.GetBytes(sbyte.MaxValue));
        }

        [TestMethod]
        public void Char_ExtractedAs_Bytes()
        {
            Check.That(ExtractBytes(char.MinValue)).Equals(BitConverter.GetBytes(char.MinValue));
            Check.That(ExtractBytes(char.MaxValue)).Equals(BitConverter.GetBytes(char.MaxValue));
            Check.That(ExtractBytes(char.MinValue)).Not.Equals(BitConverter.GetBytes(char.MaxValue));
        }

        [TestMethod]
        public void Double_ExtractedAs_Bytes()
        {
            Check.That(ExtractBytes(double.MinValue)).Equals(BitConverter.GetBytes(double.MinValue));
            Check.That(ExtractBytes(double.MaxValue)).Equals(BitConverter.GetBytes(double.MaxValue));
            Check.That(ExtractBytes(double.MinValue)).Not.Equals(BitConverter.GetBytes(double.MaxValue));
        }

        [TestMethod]
        public void Float_ExtractedAs_Bytes()
        {
            Check.That(ExtractBytes(float.MinValue)).Equals(BitConverter.GetBytes(float.MinValue));
            Check.That(ExtractBytes(float.MaxValue)).Equals(BitConverter.GetBytes(float.MaxValue));
            Check.That(ExtractBytes(float.MinValue)).Not.Equals(BitConverter.GetBytes(float.MaxValue));
        }

        [TestMethod]
        public void Int_ExtractedAs_Bytes()
        {
            Check.That(ExtractBytes(int.MinValue)).Equals(BitConverter.GetBytes(int.MinValue));
            Check.That(ExtractBytes(int.MaxValue)).Equals(BitConverter.GetBytes(int.MaxValue));
            Check.That(ExtractBytes(int.MinValue)).Not.Equals(BitConverter.GetBytes(int.MaxValue));
        }

        [TestMethod]
        public void UInt_ExtractedAs_Bytes()
        {
            Check.That(ExtractBytes(uint.MinValue)).Equals(BitConverter.GetBytes(uint.MinValue));
            Check.That(ExtractBytes(uint.MaxValue)).Equals(BitConverter.GetBytes(uint.MaxValue));
            Check.That(ExtractBytes(uint.MinValue)).Not.Equals(BitConverter.GetBytes(uint.MaxValue));
        }

        [TestMethod]
        public void Long_ExtractedAs_Bytes()
        {
            Check.That(ExtractBytes(long.MinValue)).Equals(BitConverter.GetBytes(long.MinValue));
            Check.That(ExtractBytes(long.MaxValue)).Equals(BitConverter.GetBytes(long.MaxValue));
            Check.That(ExtractBytes(long.MinValue)).Not.Equals(BitConverter.GetBytes(long.MaxValue));
        }

        [TestMethod]
        public void ULong_ExtractedAs_Bytes()
        {
            Check.That(ExtractBytes(ulong.MinValue)).Equals(BitConverter.GetBytes(ulong.MinValue));
            Check.That(ExtractBytes(ulong.MaxValue)).Equals(BitConverter.GetBytes(ulong.MaxValue));
            Check.That(ExtractBytes(ulong.MinValue)).Not.Equals(BitConverter.GetBytes(ulong.MaxValue));
        }

        [TestMethod]
        public void Short_ExtractedAs_Bytes()
        {
            Check.That(ExtractBytes(short.MinValue)).Equals(BitConverter.GetBytes(short.MinValue));
            Check.That(ExtractBytes(short.MaxValue)).Equals(BitConverter.GetBytes(short.MaxValue));
            Check.That(ExtractBytes(short.MinValue)).Not.Equals(BitConverter.GetBytes(short.MaxValue));
        }

        [TestMethod]
        public void UShort_ExtractedAs_Bytes()
        {
            Check.That(ExtractBytes(ushort.MinValue)).Equals(BitConverter.GetBytes(ushort.MinValue));
            Check.That(ExtractBytes(ushort.MaxValue)).Equals(BitConverter.GetBytes(ushort.MaxValue));
            Check.That(ExtractBytes(ushort.MinValue)).Not.Equals(BitConverter.GetBytes(ushort.MaxValue));
        }

        [TestMethod]
        public void Decimal_ExtractedAndBitConverted()
        {
            Check.That(ExtractBytes(decimal.MinValue)).Equals(decimal.MinValue.GetBytes());
            Check.That(ExtractBytes(decimal.MaxValue)).Equals(decimal.MaxValue.GetBytes());
            Check.That(ExtractBytes(decimal.MinValue)).Not.Equals(decimal.MaxValue.GetBytes());
        }

        [TestMethod]
        public void Byte_SimplyReturned()
        {
            Check.That(ExtractBytes((byte)0x12)).Equals(new [] { (byte)0x12 });
            Check.That(ExtractBytes((byte)0x00)).Equals(new [] { (byte)0x00 });
            Check.That(ExtractBytes((byte)0x12)).Not.Equals(new[] { (byte)0x00 });
        }

        private byte[] ExtractBytes(object instance)
        {
            using (var stream = new MemoryStream())
            {
                Extractor.Extract(instance, stream);
                return stream.ToArray();
            }
        }
    }
}
