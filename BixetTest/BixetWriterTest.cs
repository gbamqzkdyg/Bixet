using Bixet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System;
using System.Collections;

namespace BixetTest
{
    [TestClass]
    public class BixetWriterTest
    {
        [TestMethod]
        public void TestCreate()
        {
            BixetWriter bw = new BixetWriter(5);
            bw.BitsCount.Should().Be(40);
        }

        [TestMethod]
        public void TestSetSingleByte()
        {
            BixetWriter bw = new BixetWriter(6);
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
            BixetWriter bw = new BixetWriter(6);
            bw[2, 4] = 1;
            bw.GetData()[2].Should().Be(8);
            bw.Invoking(_ => _[1, 1] = 100).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void TestSetRawBytes()
        {
            BixetWriter bw = new BixetWriter(8);
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
            BixetWriter bw = new BixetWriter(8);
            BitArray ba = new BitArray(7);
            for (int i = 0; i < 7; ++i) ba[i] = true;
            bw.SetRawBits(4, ba, 0, 7);
            byte[] res = bw.GetData();
            //res[0].Should().Be(0xF0);
            res[1].Should().Be(0x0E);
        }
    }
}
