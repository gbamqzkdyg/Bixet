using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Bixet.BixetResolver;
using Bixet;
using Newtonsoft.Json;
using System.IO;

namespace BixetTest
{
    [TestClass]
    public class BTempletesTest
    {
        private BTemplete CreateTemplete()
        {
            BTemplete bt = new BTemplete();
            bt.Name = "Test";
            BBlock bb = new BBlock()
            {
                Name = "T1",
                Description = "A test for bb",
                Repeat = 1,
            };
            bb.AddVariable(new BVariable()
            {
                Name = "ID",
                Length = 1,
                Unit = LengthUnit.Byte,
                ValueType = BitexValueType.Number,
            });
            bb.AddVariable(new BVariable()
            {
                Name = "Value",
                Length = 4,
                Unit = LengthUnit.Byte,
                ValueType = BitexValueType.Number,
            });
            bb.AddBlock(new BBlock()
            {
                Name = "OneBits",
                Description = "8 one-bit variables",
                Repeat = 8,
            });
            bb.Blocks["OneBits"].AddVariable(new BVariable()
            {
                Name = "OneBitss",
                Length = 1,
                Unit = LengthUnit.Bit,
                ValueType = BitexValueType.Number,
            });
            bb.AddBlock(new BBlock()
            {
                Name = "TwoBits",
                Description = "4 two-bits variables",
                Repeat = 4,
            });
            bb.Blocks["TwoBits"].AddVariable(new BVariable()
            {
                Name = "TwoBitss",
                Length = 2,
                Unit = LengthUnit.Bit,
                ValueType = BitexValueType.Number,
            });
            bb.AddVariable(new BVariable()
            {
                Name = "Hello",
                Length = 5,
                Unit = LengthUnit.Byte,
                ValueType = BitexValueType.String
            });
            bt.Add(bb);
            return bt;
        }

        private void Serialize(BTemplete bt)
        {
            JsonSerializer js = new JsonSerializer();
            using (JsonTextWriter jw = new JsonTextWriter(new StreamWriter("Test.json")))
            {
                js.Serialize(jw, bt);
            }
        }

        private BTemplete Deserialzie()
        {
            JsonSerializer js = new JsonSerializer();
            using (JsonTextReader jr = new JsonTextReader(new StreamReader("Test.json")))
            {
                BTemplete btt = js.Deserialize<BTemplete>(jr);
                return btt;
            }
        }

        [TestMethod]
        public void TestSerialization()
        {
            BTemplete bt = CreateTemplete();
            Serialize(bt);
            Deserialzie().Should().Be(bt);
            
        }
    }
}
