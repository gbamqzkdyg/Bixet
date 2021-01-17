using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Bixet;
using FluentAssertions;
using System.Collections;
using System.Text;

namespace BixetTest
{
    [TestClass]
    public class BReaderTest
    {
        [TestMethod]
        public void TestCreate()
        {
            BReader br = new BReader(new byte[] { 0x55, 0x55, 0x55, 0x55 });
            br.BytesCount.Should().Be(4);
            br.BitsCount.Should().Be(br.BytesCount * 8);
            br = new BReader(new byte[] { 0x00, 0x55, 0x55, 0x55, 0x55 }, 1, 4);
            br.BytesCount.Should().Be(4);
            br.BitsCount.Should().Be(br.BytesCount * 8);
            br = new BReader(new byte[] { 0x00, 0x55, 0x55, 0x55, 0x55 }, 4);
            br.BytesCount.Should().Be(4);
            br.BitsCount.Should().Be(br.BytesCount * 8);
        }

        [TestMethod]
        public void TestGetSingleByte()
        {
            BReader br = new BReader(new byte[] { 0x11, 0x22, 0x33, 0x44 });
            br[1].Should().Be(0x22);
            br[2].Should().Be(0x33);
            br.Invoking(_ => _[4]).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void TestGetSingleBit()
        {
            BReader br = new BReader(new byte[] { 0x20, 0x22, 0x33, 0x44 });
            0.Should().Be(br[0, 4]);
            1.Should().Be(br[0, 5]);
            1.Should().Be(br[3, 2]);
            br.Invoking(_ => _[0, 32]).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void TestGetRawBytes()
        {
            new BReader(new byte[] { 0, 1, 2, 3 }).GetRawBytes(1, 3).Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        }

        [TestMethod]
        public void TestGetRawBits()
        {
            new BReader(new byte[] { 0, 1, 2, 3 }).GetRawBits(17, 11).Should().BeEquivalentTo(new BitArray(new bool[] {
                true, false, false, false, false, false, false, true, true, false, false
            }));
            new BReader(new byte[] { 0, 1, 2, 3 }).GetRawBits(2, 1, 11).Should().BeEquivalentTo(new BitArray(new bool[] {
                true, false, false, false, false, false, false, true, true, false, false
            }));
        }

        private class Foo { }

        [TestMethod]
        public void TestReadValueByByteIndex()
        {
            BReader br = new BReader(new byte[] { 0x12, 0x34, 0x56, 0x78 });
            br.ReadValueByByteIndex<long>(0, 4).Should().Be(0x12345678);
            br.ReadValueByByteIndex<long>(1, 3).Should().Be(0x345678);
            br.Invoking(_ => _.ReadValueByByteIndex<short>(0, 4)).Should().Throw<ArgumentOutOfRangeException>();
            br.Invoking(_ => _.ReadValueByByteIndex<string>(0, 4)).Should().Throw<NotSupportedException>();
            br.Invoking(_ => _.ReadValueByByteIndex<bool>(0, 4)).Should().Throw<NotSupportedException>();
            br.Invoking(_ => _.ReadValueByByteIndex<Foo>(0, 4)).Should().Throw<NotSupportedException>();
            new BReader(new byte[] { 0x12, 0x34, 0x56, 0x78 }, Endian.SmallEndian).ReadValueByByteIndex<long>(0, 4).Should().Be(0x78563412);
        }

        [TestMethod]
        public void TestReadStringByByteIndex()
        {
            BReader br = new BReader(Encoding.Default.GetBytes("Hello world!"));
            br.ReadStringByByteIndex(0, 5).Should().Be("Hello");
            br.ReadStringByByteIndex(6, 5).Should().Be("world");
            
            br.Invoking(_ => _.ReadStringByByteIndex(8, 100)).Should().Throw<ArgumentOutOfRangeException>();
            new BReader(Encoding.Default.GetBytes("Hello world!"), Endian.SmallEndian).ReadStringByByteIndex(6, 5).Should().Be("dlrow");
        }

        [TestMethod]
        public void TestReadValueByBitIndex()
        {
            BReader br = new BReader(new byte[] { 0x12, 0x34 }, Endian.SmallEndian);
            br.ReadValueByBitIndex<int>(0, 16).Should().Be(0x3412);
            br.ReadValueByBitIndex<long>(4, 8).Should().Be(0x41);
            br.ReadValueByBitIndex<bool>(13, 1).Should().Be(true);
            br.ReadValueByBitIndex<byte>(9, 5).Should().Be(0x1A);
            br.ReadValueByBitIndex<byte>(1, 1, 5).Should().Be(0x1A);
            br.Invoking(_ => _.ReadValueByBitIndex<byte>(0, 9)).Should().Throw<ArgumentOutOfRangeException>();
            br.Invoking(_ => _.ReadValueByBitIndex<string>(0, 4)).Should().Throw<NotSupportedException>();
            br.Invoking(_ => _.ReadValueByBitIndex<Foo>(0, 4)).Should().Throw<NotSupportedException>();
            br = new BReader(new byte[] { 0x12, 0x34 }, Endian.BigEndian, Endian.BigEndian);
            br.ReadValueByBitIndex<int>(0, 16).Should().Be(0x482C);
            br.ReadValueByBitIndex<byte>(9, 6).Should().Be(0x16);
        }

        private byte ReverseByte(byte b)
        {
            byte res = 0;
            for (int i = 0; i < 8; ++i)
            {
                res <<= 1;
                res += (byte)(b % 2);
                b >>= 1;
            }
            return res;
        }

        [TestMethod]
        public void TestReadStringByBitIndex()
        {
            BReader br = new BReader(Encoding.Default.GetBytes("Hello world!"));
            br.ReadStringByBitIndex(0, 96).Should().Be("Hello world!");
            br.ReadStringByBitIndex(48, 48).Should().Be("world!");
            br.Invoking(_ => _.ReadStringByBitIndex(0, 42)).Should().Throw<FormatException>();
            byte[] bixetBytes = Encoding.Default.GetBytes("bixet");
            br = new BReader(bixetBytes);
            br.ReadStringByBitIndex(0, 40).Should().Be("bixet");
            byte[] bixetReverseBytes = new byte[] { this.ReverseByte((byte)'t'), this.ReverseByte((byte)'e'), this.ReverseByte((byte)'x'), this.ReverseByte((byte)'i'), this.ReverseByte((byte)'b'), };
            br = new BReader(bixetReverseBytes, Endian.SmallEndian, Endian.BigEndian);
            br.ReadStringByBitIndex(0, 40).Should().Be("texib");
            byte[] bixetComplexBytes = new byte[] { 0x7F, 0b10110001, 0b00110100, 0b10111100, 0b00110010, 0b10111010 };
            br = new BReader(bixetComplexBytes);
            br.ReadStringByBitIndex(7, 40).Should().Be("bixet");
        }

        [TestMethod]
        public void TestFunction()
        {
            byte[] message = new byte[] { 0x01, 0x12, 0x34, 0x56, 0x78, 0b10101010, 0b11100100, (byte)'S', (byte)'u', (byte)'c', (byte)'c', (byte)'e', (byte)'s', (byte)'s', };
            BReader br = new BReader(message);
            br.ReadValueByByteIndex<byte>(0, 1).Should().Be(0x01);
            br.ReadValueByByteIndex<int>(1, 4).Should().Be(0x12345678);
            for (int i = 0; i < 8; ++i) br.ReadValueByBitIndex<bool>(5, i, 1).Should().Be(i % 2 == 1);
            for (byte i = 0; i < 4; ++i) br.ReadValueByBitIndex<byte>(6, 2 * i, 2).Should().Be(i);
            br.ReadStringByByteIndex(7, 7).Should().Be("Success");
        }
    }
}
