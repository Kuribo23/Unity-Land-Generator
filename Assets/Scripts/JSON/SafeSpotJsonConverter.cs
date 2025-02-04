using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SafeSpotJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(SafeSpotJsonData) == objectType;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader == null)
            return null;
        if (reader.TokenType == JsonToken.Null)
            return null;

        JToken t = JToken.Load(reader);

        return new SafeSpotJsonData
        {
            SafeSpots = t["SafeSpots"].ToObject<List<List<int>>>(),
        };
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
    }
}
