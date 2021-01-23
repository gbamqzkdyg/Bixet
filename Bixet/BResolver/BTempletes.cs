using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Bixet.BixetResolver
{
    public class BTempletes
    {
        [JsonIgnore]
        public const string version = "0.0.1";

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public IDictionary<string, BBlock> Templetes { get; set; } = new Dictionary<string, BBlock>();

        public BTempletes() { }

        public BTempletes(BTempletes bt)
        {
            this.Name = bt.Name;
            this.Templetes = bt.Templetes;
        }

        public BBlock this[string name]
        {
            get => this.Templetes[name];
        }

        public void Add(BBlock bb)
        {
            this.Templetes.Add(bb.Name, bb);
        }

        public override bool Equals(object obj)
        {
            return obj is BTempletes templete &&
                   Name == templete.Name &&
                   Templetes.SequenceEqual(templete.Templetes);
        }

        public override int GetHashCode()
        {
            int hashCode = 1780610354;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<IDictionary<string, BBlock>>.Default.GetHashCode(Templetes);
            return hashCode;
        }
    }
}
