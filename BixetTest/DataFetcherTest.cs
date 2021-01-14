using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Bixet;
using FluentAssertions;
using System.Collections;
using System.Text;

namespace BixetTest
{
    [TestClass]
    public class DataFetcherTest
    {
        [TestMethod]
        public void TestCreate()
        {
            DataFetcher df = new DataFetcher(new byte[] { 0x55, 0x55, 0x55, 0x55 });
            df.BytesCount.Should().Be(4);
            df.BitsCount.Should().Be(df.BytesCount * 8);
            df = new DataFetcher(new byte[] { 0x00, 0x55, 0x55, 0x55, 0x55 }, 1, 4);
            df.BytesCount.Should().Be(4);
            df.BitsCount.Should().Be(df.BytesCount * 8);
            df = new DataFetcher(new byte[] { 0x00, 0x55, 0x55, 0x55, 0x55 }, 4);
            df.BytesCount.Should().Be(4);
            df.BitsCount.Should().Be(df.BytesCount * 8);
        }

        [TestMethod]
        public void TestGetSingleByte()
        {
            DataFetcher df = new DataFetcher(new byte[] { 0x11, 0x22, 0x33, 0x44 });
            df[1].Should().Be(0x22);
            df[2].Should().Be(0x33);
            df.Invoking(_ => _[4]).Should().Throw<IndexOutOfRangeException>().WithMessage("给定的参数异常");
        }

        [TestMethod]
        public void TestGetSingleBit()
        {
            DataFetcher df = new DataFetcher(new byte[] { 0x20, 0x22, 0x33, 0x44 });
            0.Should().Be(df % 3);
            1.Should().Be(df % 2);
            1.Should().Be(df % 29);
            df.Invoking(_ => _ % 32).Should().Throw<IndexOutOfRangeException>().WithMessage("给定的参数异常");
        }

        [TestMethod]
        public void TestEndians()
        {
            byte[] bytes = new byte[] { 1, 0, 0, 0 };
            1.Should().Be(new DataFetcher(bytes, Endian.BigEndian, Endian.SmallEndian) % 7);
            1.Should().Be(new DataFetcher(bytes, Endian.SmallEndian, Endian.SmallEndian) % 31);
            1.Should().Be(new DataFetcher(bytes, Endian.BigEndian, Endian.BigEndian) % 0);
            1.Should().Be(new DataFetcher(bytes, Endian.SmallEndian, Endian.BigEndian) % 24);
        }

        [TestMethod]
        public void TestGetRawBytes()
        {
            new DataFetcher(new byte[] { 0, 1, 2, 3 }).GetRawBytes(1, 3).Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        }

        [TestMethod]
        public void TestGetRawBits()
        {
            new DataFetcher(new byte[] { 0, 1, 2, 3 }).GetRawBits(17, 11).Should().BeEquivalentTo(new BitArray(new bool[] {
                false, false, false, false, false, true, false, false, false, false, false
            }));
            new DataFetcher(new byte[] { 0, 1, 2, 3 }).GetRawBits(2, 1, 11).Should().BeEquivalentTo(new BitArray(new bool[] {
                false, false, false, false, false, true, false, false, false, false, false
            }));
        }

        private class Foo { }

        [TestMethod]
        public void TestGetValueByByteIndex()
        {
            DataFetcher df = new DataFetcher(new byte[] { 0x12, 0x34, 0x56, 0x78 });
            df.GetValueByByteIndex<long>(0, 4).Should().Be(0x12345678);
            df.GetValueByByteIndex<long>(0, 4, true).Should().Be(0x78563412);
            df.GetValueByByteIndex<long>(1, 3).Should().Be(0x345678);
            df.Invoking(_ => _.GetValueByByteIndex<int>(0, 4)).Should().Throw<IndexOutOfRangeException>();
            df.Invoking(_ => _.GetValueByByteIndex<string>(0, 4)).Should().Throw<NotSupportedException>();
            df.Invoking(_ => _.GetValueByByteIndex<bool>(0, 4)).Should().Throw<NotSupportedException>();
            df.Invoking(_ => _.GetValueByByteIndex<Foo>(0, 4)).Should().Throw<NotSupportedException>();
        }

        [TestMethod]
        public void TestGetStringByByteIndex()
        {
            DataFetcher df = new DataFetcher(Encoding.Default.GetBytes("Hello world!"));
            df.GetStringByByteIndex(0, 5).Should().Be("Hello");
            df.GetStringByByteIndex(6, 5).Should().Be("world");
            df.GetStringByByteIndex(6, 5, true).Should().Be("dlrow");
            df.Invoking(_ => _.GetStringByByteIndex(8, 100)).Should().Throw<IndexOutOfRangeException>();
        }

        [TestMethod]
        public void TestGetValueByBitIndex()
        {
            DataFetcher df = new DataFetcher(new byte[] { 0x12, 0x34 });
            df.GetValueByBitIndex<int>(0, 16).Should().Be(0x1234);
            df.GetValueByBitIndex<int>(0, 16, true).Should().Be(0x2c48);
            df.GetValueByBitIndex<long>(4, 8).Should().Be(0x23);
            df.GetValueByBitIndex<bool>(13, 1).Should().Be(true);
            df.GetValueByBitIndex<byte>(9, 5).Should().Be(0x0d);
            df.GetValueByBitIndex<byte>(1, 1, 5).Should().Be(0x0d);
            df.Invoking(_ => _.GetValueByBitIndex<byte>(0, 9)).Should().Throw<IndexOutOfRangeException>();
            df.Invoking(_ => _.GetValueByBitIndex<string>(0, 4)).Should().Throw<NotSupportedException>();
            df.Invoking(_ => _.GetValueByBitIndex<Foo>(0, 4)).Should().Throw<NotSupportedException>();
        }
    }
}
