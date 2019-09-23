using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kaching.Extensions.Model
{
    public class ProductTagsConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var tags = (HashSet<string>)value;
            if (tags.Count > 0)
            {
                var tagsDict = new Dictionary<string, bool>();
                foreach (var tag in tags)
                {
                    tagsDict[tag] = true;
                }
                serializer.Serialize(writer, tagsDict);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Only handling serialization for now
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(HashSet<string>);
        }
    }


    public class KachingProduct
    {
        public L10nString Name { get; set; }
        public L10nString Description { get; set; }
        public MarketPrice RetailPrice { get; set; }
        public string Id { get; internal set; }

        public string ImageUrl { get; set; }

        public Variant[] Variants { get; set; }
        public List<Dimension> Dimensions { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        [JsonConverter(typeof(ProductTagsConverter))]
        public HashSet<string> Tags { get; set; }
    }

    public class Variant
    {
        public L10nString Name { get; set; }
        public MarketPrice RetailPrice { get; set; }
        public string Id { get; internal set; }
        public string ImageUrl { get; set; }
        public Dictionary<string, string> DimensionValues { get; set; }

        public Dictionary<string, string> Attributes { get; set; }
    }

    public class Dimension
    {
        public L10nString Name { get; set; }
        public string Id { get; set; }
        public List<DimensionValue> Values { get; set; }
    }

    public class DimensionValue
    {
        public string Id { get; set; }
        public L10nString Name { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }
    }


}
