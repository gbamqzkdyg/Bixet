using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Bixet.BixetResolver
{
    public class BixetTemplete
    {
        [JsonIgnore]
        public const string Version = "0.0.1";

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public IDictionary<string, BixetBlock> Templetes { get; set; } = new Dictionary<string, BixetBlock>();

        public BixetTemplete() { }

        public BixetTemplete(BixetTemplete bt)
        {
            this.Name = bt.Name;
            this.Templetes = bt.Templetes;
        }

        public BixetBlock this[string name]
        {
            get => this.Templetes[name];
        }

        public void Add(BixetBlock bb)
        {
            this.Templetes.Add(bb.Name, bb);
        }

        public override bool Equals(object obj)
        {
            return obj is BixetTemplete templete &&
                   Name == templete.Name &&
                   Templetes.SequenceEqual(templete.Templetes);
        }

        public override int GetHashCode()
        {
            int hashCode = 1780610354;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<IDictionary<string, BixetBlock>>.Default.GetHashCode(Templetes);
            return hashCode;
        }
    }
}
