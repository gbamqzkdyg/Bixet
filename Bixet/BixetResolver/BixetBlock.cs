using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Bixet.BixetResolver
{
    public class BixetBlock
    {
        [JsonIgnore]
        public const string Version = "0.0.1";

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public int Repeat { get; set; }

        [JsonProperty]
        public IList<string> Children { get; set; } = new List<string>();

        [JsonProperty]
        public IDictionary<string, BixetVariable> Variables { get; set; } = new Dictionary<string, BixetVariable>();

        [JsonProperty]
        public IDictionary<string, BixetBlock> Blocks { get; set; } = new Dictionary<string, BixetBlock>();

        [JsonIgnore]
        public int BitLength { get => this.GetBitLength(); }

        [JsonIgnore]
        public int ByteLength { get => (this.BitLength + 7) / 8; }

        public BixetBlock() { }

        public BixetBlock(BixetBlock bb)
        {
            this.Name = bb.Name;
            this.Description = bb.Description;
            this.Repeat = bb.Repeat;
            this.Children = new List<string>(bb.Children);
            this.Variables = new Dictionary<string, BixetVariable>(bb.Variables);
            this.Blocks = new Dictionary<string, BixetBlock>(bb.Blocks);
        }

        public void AddBlock(BixetBlock bb)
        {
            this.AddChild(bb.Name);
            this.Blocks.Add(bb.Name, bb);
        }

        public void AddVariable(BixetVariable bv)
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
            return obj is BixetBlock block &&
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
            hashCode = hashCode * -1521134295 + EqualityComparer<IDictionary<string, BixetVariable>>.Default.GetHashCode(Variables);
            hashCode = hashCode * -1521134295 + EqualityComparer<IDictionary<string, BixetBlock>>.Default.GetHashCode(Blocks);
            hashCode = hashCode * -1521134295 + BitLength.GetHashCode();
            hashCode = hashCode * -1521134295 + ByteLength.GetHashCode();
            return hashCode;
        }
    }
}
