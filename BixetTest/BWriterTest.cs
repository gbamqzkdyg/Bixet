using Bixet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System;
using System.Collections;
using System.Text;

namespace BixetTest
{
    [TestClass]
    public class BWriterTest
    {
        [TestMethod]
        public void TestCreate()
        {
            BWriter bw = new BWriter(5);
            bw.BitsCount.Should().Be(40);
        }

        [TestMethod]
        public void TestSetSingleByte()
        {
            BWriter bw = new BWriter(6);
            bw[1] = 1;
            bw[3] = 2;
            bw[5] = 3;
            byte[] res = bw.GetData();
            res[1].Should().Be(1);
            res[3].Should().Be(2);
            res[5].Should().Be(3);
        }

        [TestMethod]
        public void TestSetSingleBit()
        {
            BWriter bw = new BWriter(6);
            bw[2, 4] = 1;
            bw.GetData()[2].Should().Be(16);
            bw.Invoking(_ => _[1, 1] = 100).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void TestSetRawBytes()
        {
            BWriter bw = new BWriter(8);
            bw.SetRawBytes(1, new byte[] { 0, 0, 1, 2, 3 }, 2, 3);
            byte[] res = bw.GetData();
            for (byte i = 1; i <= 3; ++i) res[i].Should().Be(i);
            bw.SetRawBytes(0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }, 0, 8);
            res = bw.GetData();
            for (byte i = 0; i < 8; ++i) res[i].Should().Be(i);
            bw.Invoking(_ => _.SetRawBytes(7, new byte[] { 1, 2, 3, 4, 5 })).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void TestSetRawBits()
        {
            BWriter bw = new BWriter(8);
            BitArray ba = new BitArray(7);
            for (int i = 0; i < 7; ++i) ba[i] = true;
            bw.SetRawBits(4, ba, 0, 7);
            byte[] res = bw.GetData();
            //res[0].Should().Be(0xF0);
            res[1].Should().Be(0x07);
            res[0].Should().Be(0xF0);
        }

        [TestMethod]
        public void TestWriteValueByByteIndex()
        {
            BWriter bw = new BWriter(5);
            bw.WriteValueByByteIndex<ulong>(1, 0xAABBCCDD12345678, 4);
            byte[] res = bw.GetData();
            res[1].Should().Be(0x12);
            res[2].Should().Be(0x34);
            res[3].Should().Be(0x56);
            res[4].Should().Be(0x78);
            bw = new BWriter(5);
            bw.WriteValueByByteIndex<ulong>(1, 0xAABBCCDD12345678, 4, Endian.SmallEndian);
            res = bw.GetData();
            res[1].Should().Be(0x78);
            res[2].Should().Be(0x56);
            res[3].Should().Be(0x34);
            res[4].Should().Be(0x12);
            bw.Invoking(_ => _.WriteValueByByteIndex<bool>(0, false, 1)).Should().Throw<NotSupportedException>();
            bw.Invoking(_ => _.WriteValueByByteIndex<BWriter>(0, _, 1)).Should().Throw<NotSupportedException>();
            bw.Invoking(_ => _.WriteValueByByteIndex<int>(0, 0, 5)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void TestWriteValueByBitIndex()
        {
            BWriter bw = new BWriter(8);
            bw.WriteValueByBitIndex<bool>(42, true, 1);
            byte[] res = bw.GetData();
            res[5].Should().Be(4);
            bw.WriteValueByBitIndex<byte>(55, 0b10110111, 7);
            res = bw.GetData();
            res[6].Should().Be(0x80);
            res[7].Should().Be(0x1b);
        }

        [TestMethod]
        public void TestWriteStringByByteIndex()
        {
            BWriter bw = new BWriter(10);
            bw.WriteStringByByteIndex(2, "bixet", 5);
            Encoding.Default.GetString(bw.GetData(), 2, 5).Should().Be("bixet");
            bw = new BWriter(10);
            bw.WriteStringByByteIndex(2, "texib", 5, Endian.SmallEndian);
            Encoding.Default.GetString(bw.GetData(), 2, 5).Should().Be("bixet");
        }

        [TestMethod]
        public void TestWriteStringByBitIndex()
        {
            BWriter bw = new BWriter(10);
            bw.WriteStringByBitIndex(16, "bixet", 40);
            Encoding.Default.GetString(bw.GetData(), 2, 5).Should().Be("bixet");
        }

        [TestMethod]
        public void TestFunction()
        {
            byte[] message = new byte[] { 0x01, 0x12, 0x34, 0x56, 0x78, 0b10101010, 0b11100100, (byte)'S', (byte)'u', (byte)'c', (byte)'c', (byte)'e', (byte)'s', (byte)'s', };
            BWriter bw = new BWriter(message.Length);
            bw.WriteValueByByteIndex<byte>(0, 0x01);
            bw.WriteValueByByteIndex<int>(1, 0x12345678);
            for (int i = 0; i < 8; ++i) bw.WriteValueByBitIndex<bool>(5, i, i % 2 == 1, 1);
            for (byte i = 0; i < 4; ++i) bw.WriteValueByBitIndex<byte>(6, 2 * i, i, 2);
            bw.WriteStringByByteIndex(7, "Success", 7);
            bw.GetData().Should().BeEquivalentTo(message);
        }
    }
}
