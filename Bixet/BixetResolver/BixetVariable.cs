using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;

namespace Bixet.BixetResolver
{
    public class BixetVariable
    {
        [JsonIgnore]
        public const string Version = "0.0.1";

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public LengthUnit Unit { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public BitexValueType ValueType { get; set; }

        [JsonProperty]
        public ushort Length { get; set; }

        [JsonIgnore]
        public object Value { get; set; }

        [JsonProperty]
        public IDictionary<object, object> Descriptions { get; set; } = new Dictionary<object, object>();

        [JsonIgnore]
        public int BitLength { get => this.Length * (this.Unit == LengthUnit.Bit ? 1 : 8); }

        public BixetVariable() { }

        public BixetVariable(BixetVariable bv)
        {
            this.Name = bv.Name;
            this.Unit = bv.Unit;
            this.ValueType = bv.ValueType;
            this.Length = bv.Length;
            this.Value = bv.Value;
            this.Descriptions = new Dictionary<object, object>(bv.Descriptions);
        }

        public object this[object o]
        {
            get => this.Descriptions[o];
            set => this.Descriptions[o] = value;
        }

        public override bool Equals(object obj)
        {
            return obj is BixetVariable variable &&
                   Name == variable.Name &&
                   Unit == variable.Unit &&
                   ValueType == variable.ValueType &&
                   Length == variable.Length &&
                   Descriptions.SequenceEqual(variable.Descriptions) &&
                   BitLength == variable.BitLength;
        }

        public override int GetHashCode()
        {
            int hashCode = 490240944;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Unit.GetHashCode();
            hashCode = hashCode * -1521134295 + ValueType.GetHashCode();
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value);
            hashCode = hashCode * -1521134295 + EqualityComparer<IDictionary<object, object>>.Default.GetHashCode(Descriptions);
            hashCode = hashCode * -1521134295 + BitLength.GetHashCode();
            return hashCode;
        }
    }
}
