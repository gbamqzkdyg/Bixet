using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Bixet;
using FluentAssertions;
using System.Collections;
using System.Text;

namespace BixetTest
{
    [TestClass]
    public class BixetReaderTest
    {
        [TestMethod]
        public void TestCreate()
        {
            BixetReader br = new BixetReader(new byte[] { 0x55, 0x55, 0x55, 0x55 });
            br.BytesCount.Should().Be(4);
            br.BitsCount.Should().Be(br.BytesCount * 8);
            br = new BixetReader(new byte[] { 0x00, 0x55, 0x55, 0x55, 0x55 }, 1, 4);
            br.BytesCount.Should().Be(4);
            br.BitsCount.Should().Be(br.BytesCount * 8);
            br = new BixetReader(new byte[] { 0x00, 0x55, 0x55, 0x55, 0x55 }, 4);
            br.BytesCount.Should().Be(4);
            br.BitsCount.Should().Be(br.BytesCount * 8);
        }

        [TestMethod]
        public void TestGetSingleByte()
        {
            BixetReader br = new BixetReader(new byte[] { 0x11, 0x22, 0x33, 0x44 });
            br[1].Should().Be(0x22);
            br[2].Should().Be(0x33);
            br.Invoking(_ => _[4]).Should().Throw<IndexOutOfRangeException>().WithMessage("给定的参数异常");
        }

        [TestMethod]
        public void TestGetSingleBit()
        {
            BixetReader br = new BixetReader(new byte[] { 0x20, 0x22, 0x33, 0x44 });
            0.Should().Be(br % 3);
            1.Should().Be(br % 2);
            1.Should().Be(br % 29);
            br.Invoking(_ => _ % 32).Should().Throw<IndexOutOfRangeException>().WithMessage("给定的参数异常");
        }

        [TestMethod]
        public void TestEndians()
        {
            byte[] bytes = new byte[] { 1, 0, 0, 0 };
            1.Should().Be(new BixetReader(bytes, Endian.BigEndian, Endian.SmallEndian) % 7);
            1.Should().Be(new BixetReader(bytes, Endian.SmallEndian, Endian.SmallEndian) % 31);
            1.Should().Be(new BixetReader(bytes, Endian.BigEndian, Endian.BigEndian) % 0);
            1.Should().Be(new BixetReader(bytes, Endian.SmallEndian, Endian.BigEndian) % 24);
        }

        [TestMethod]
        public void TestGetRawBytes()
        {
            new BixetReader(new byte[] { 0, 1, 2, 3 }).GetRawBytes(1, 3).Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        }

        [TestMethod]
        public void TestGetRawBits()
        {
            new BixetReader(new byte[] { 0, 1, 2, 3 }).GetRawBits(17, 11).Should().BeEquivalentTo(new BitArray(new bool[] {
                false, false, false, false, false, true, false, false, false, false, false
            }));
            new BixetReader(new byte[] { 0, 1, 2, 3 }).GetRawBits(2, 1, 11).Should().BeEquivalentTo(new BitArray(new bool[] {
                false, false, false, false, false, true, false, false, false, false, false
            }));
        }

        private class Foo { }

        [TestMethod]
        public void TestReadValueByByteIndex()
        {
            BixetReader br = new BixetReader(new byte[] { 0x12, 0x34, 0x56, 0x78 });
            br.ReadValueByByteIndex<long>(0, 4).Should().Be(0x12345678);
            br.ReadValueByByteIndex<long>(0, 4, true).Should().Be(0x78563412);
            br.ReadValueByByteIndex<long>(1, 3).Should().Be(0x345678);
            br.Invoking(_ => _.ReadValueByByteIndex<int>(0, 4)).Should().Throw<IndexOutOfRangeException>();
            br.Invoking(_ => _.ReadValueByByteIndex<string>(0, 4)).Should().Throw<NotSupportedException>();
            br.Invoking(_ => _.ReadValueByByteIndex<bool>(0, 4)).Should().Throw<NotSupportedException>();
            br.Invoking(_ => _.ReadValueByByteIndex<Foo>(0, 4)).Should().Throw<NotSupportedException>();
        }

        [TestMethod]
        public void TestReadStringByByteIndex()
        {
            BixetReader br = new BixetReader(Encoding.Default.GetBytes("Hello world!"));
            br.ReadStringByByteIndex(0, 5).Should().Be("Hello");
            br.ReadStringByByteIndex(6, 5).Should().Be("world");
            br.ReadStringByByteIndex(6, 5, true).Should().Be("dlrow");
            br.Invoking(_ => _.ReadStringByByteIndex(8, 100)).Should().Throw<IndexOutOfRangeException>();
        }

        [TestMethod]
        public void TestReadValueByBitIndex()
        {
            BixetReader br = new BixetReader(new byte[] { 0x12, 0x34 });
            br.ReadValueByBitIndex<int>(0, 16).Should().Be(0x1234);
            br.ReadValueByBitIndex<int>(0, 16, true).Should().Be(0x2c48);
            br.ReadValueByBitIndex<long>(4, 8).Should().Be(0x23);
            br.ReadValueByBitIndex<bool>(13, 1).Should().Be(true);
            br.ReadValueByBitIndex<byte>(9, 5).Should().Be(0x0d);
            br.ReadValueByBitIndex<byte>(1, 1, 5).Should().Be(0x0d);
            br.Invoking(_ => _.ReadValueByBitIndex<byte>(0, 9)).Should().Throw<IndexOutOfRangeException>();
            br.Invoking(_ => _.ReadValueByBitIndex<string>(0, 4)).Should().Throw<NotSupportedException>();
            br.Invoking(_ => _.ReadValueByBitIndex<Foo>(0, 4)).Should().Throw<NotSupportedException>();
        }
    }
}
