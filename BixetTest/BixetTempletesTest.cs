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
    public class BixetTempletesTest
    {
        private BixetTemplete CreateTemplete()
        {
            BixetTemplete bt = new BixetTemplete();
            bt.Name = "Test";
            BixetBlock bb = new BixetBlock()
            {
                Name = "T1",
                Description = "A test for bb",
                Repeat = 1,
            };
            bb.AddVariable(new BixetVariable()
            {
                Name = "ID",
                Length = 1,
                Unit = LengthUnit.Byte,
                ValueType = BitexValueType.Number,
            });
            bb.AddVariable(new BixetVariable()
            {
                Name = "Value",
                Length = 4,
                Unit = LengthUnit.Byte,
                ValueType = BitexValueType.Number,
            });
            bb.AddBlock(new BixetBlock()
            {
                Name = "OneBits",
                Description = "8 one-bit variables",
                Repeat = 8,
            });
            bb.Blocks["OneBits"].AddVariable(new BixetVariable()
            {
                Name = "OneBitss",
                Length = 1,
                Unit = LengthUnit.Bit,
                ValueType = BitexValueType.Number,
            });
            bb.AddBlock(new BixetBlock()
            {
                Name = "TwoBits",
                Description = "4 two-bits variables",
                Repeat = 4,
            });
            bb.Blocks["TwoBits"].AddVariable(new BixetVariable()
            {
                Name = "TwoBitss",
                Length = 2,
                Unit = LengthUnit.Bit,
                ValueType = BitexValueType.Number,
            });
            bb.AddVariable(new BixetVariable()
            {
                Name = "Hello",
                Length = 5,
                Unit = LengthUnit.Byte,
                ValueType = BitexValueType.String
            });
            bt.Add(bb);
            return bt;
        }

        private void Serialize(BixetTemplete bt)
        {
            JsonSerializer js = new JsonSerializer();
            using (JsonTextWriter jw = new JsonTextWriter(new StreamWriter("Test.json")))
            {
                js.Serialize(jw, bt);
            }
        }

        private BixetTemplete Deserialzie()
        {
            JsonSerializer js = new JsonSerializer();
            using (JsonTextReader jr = new JsonTextReader(new StreamReader("Test.json")))
            {
                BixetTemplete btt = js.Deserialize<BixetTemplete>(jr);
                return btt;
            }
        }

        [TestMethod]
        public void TestSerialization()
        {
            BixetTemplete bt = CreateTemplete();
            Serialize(bt);
            Deserialzie().Should().Be(bt);
            
        }
    }
}
