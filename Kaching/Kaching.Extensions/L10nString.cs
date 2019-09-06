using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
            }
            else
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
}
