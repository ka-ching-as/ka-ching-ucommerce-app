using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kaching.Extensions
{

    public class L10nStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            L10nString l10n = (L10nString)value;
            if (l10n.Localized != null)
            {
                serializer.Serialize(writer, l10n.Localized);
            } else
            {
                writer.WriteValue(l10n.Unlocalized);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Only handling serialization for now
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(L10nString);
        }
    }


    [JsonConverter(typeof(L10nStringConverter))]
    public class L10nString
    {
        public string Unlocalized { get; set; }
        public Dictionary<string, string> Localized { get; set; } 

        public L10nString(string Value)
        {
            this.Unlocalized = Value;
        }

        public L10nString(Dictionary<string, string> Value)
        {
            this.Localized = Value;
        }
    }

    public class KachingProduct
    {
        public L10nString Name { get; set; }
        public L10nString Description { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public string Id { get; internal set; }

        public string ImageUrl { get; set; }

        public Variant[] Variants { get; set; }

        public Dictionary<string, string> Attributes { get; set; }
        public Dictionary<string, bool> Tags { get; set; }
    }

    public class Variant
    {
        public L10nString Name { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public string Id { get; internal set; }
        public string ImageUrl { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

    }
}
