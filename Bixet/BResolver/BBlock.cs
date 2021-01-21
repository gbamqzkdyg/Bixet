using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Bixet.BixetResolver
{
    public class BBlock
    {
        [JsonIgnore]
        public const string version = "0.1.0";

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public int Repeat { get; set; }

        [JsonProperty]
        public IList<string> Children { get; set; } = new List<string>();

        [JsonProperty]
        public IDictionary<string, BVariable> Variables { get; set; } = new Dictionary<string, BVariable>();

        [JsonProperty]
        public IDictionary<string, BBlock> Blocks { get; set; } = new Dictionary<string, BBlock>();

        [JsonIgnore]
        public int BitLength { get => this.GetBitLength(); }

        [JsonIgnore]
        public int ByteLength { get => (this.BitLength + 7) / 8; }

        public BBlock() { }

        public BBlock(BBlock bb)
        {
            this.Name = bb.Name;
            this.Description = bb.Description;
            this.Repeat = bb.Repeat;
            this.Children = new List<string>(bb.Children);
            this.Variables = new Dictionary<string, BVariable>(bb.Variables);
            this.Blocks = new Dictionary<string, BBlock>(bb.Blocks);
        }

        public void AddBlock(BBlock bb)
        {
            this.AddChild(bb.Name);
            this.Blocks.Add(bb.Name, bb);
        }

        public void AddVariable(BVariable bv)
        {
            this.AddChild(bv.Name);
            this.Variables.Add(bv.Name, bv);
        }

        private void AddChild(string name)
        {
            this.Children.Add(name);
        }

        private int GetBitLength()
        {
            int res = this.Variables.Values.Sum(v => v.BitLength);
            res += this.Blocks.Values.Sum(b => b.GetBitLength());
            return res * this.Repeat;
        }

        public override bool Equals(object obj)
        {
            return obj is BBlock block &&
                   Name == block.Name &&
                   Description == block.Description &&
                   Repeat == block.Repeat &&
                   Children.SequenceEqual(block.Children) &&
                   Variables.SequenceEqual(block.Variables) &&
                   Blocks.SequenceEqual(block.Blocks) &&
                   BitLength == block.BitLength &&
                   ByteLength == block.ByteLength;
        }

        public override int GetHashCode()
        {
            int hashCode = 1528622779;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + Repeat.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IList<string>>.Default.GetHashCode(Children);
            hashCode = hashCode * -1521134295 + EqualityComparer<IDictionary<string, BVariable>>.Default.GetHashCode(Variables);
            hashCode = hashCode * -1521134295 + EqualityComparer<IDictionary<string, BBlock>>.Default.GetHashCode(Blocks);
            hashCode = hashCode * -1521134295 + BitLength.GetHashCode();
            hashCode = hashCode * -1521134295 + ByteLength.GetHashCode();
            return hashCode;
        }
    }
}
