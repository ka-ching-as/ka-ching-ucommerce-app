using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kaching.Extensions.Model
{
    public enum Channel
    {
        pos
    }

    public class MetadataConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var metadata = (KachingMetadata)value;

            var marketDict = new Dictionary<string, bool>();
            foreach (var market in metadata.Markets)
            {
                marketDict[market] = true;
            }

            var channelDict = new Dictionary<string, bool>();
            foreach (var channel in metadata.Channels)
            {
                switch (channel)
                {
                    case Channel.pos:
                        channelDict["pos"] = true;
                        break;
                }
            }

            var final = new Dictionary<string, Dictionary<string, bool>>();
            if (marketDict.Count > 0)
            {
                final["markets"] = marketDict;
            }

            if (channelDict.Count > 0)
            {
                final["channels"] = channelDict;
            }

            if (final.Count > 0)
            {
                serializer.Serialize(writer, final);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Only handling serialization for now
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(KachingMetadata);
        }
    }

    [JsonConverter(typeof(MetadataConverter))]
    public class KachingMetadata
    {
        public HashSet<string> Markets;
        public HashSet<Channel> Channels;
        public KachingMetadata()
        {
            Channels = new HashSet<Channel>();
            Channels.Add(Channel.pos);
            Markets = new HashSet<string>();
        }
    }
}
