using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class OverlayJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(OverlayJson) == objectType;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader == null)
            return null;
        if (reader.TokenType == JsonToken.Null)
            return null;

        JToken t = JToken.Load(reader);

        return new OverlayJson
        {
            Slots1x1 = t["1x1"].ToObject<List<List<int>>>(),
            Slots2x1 = t["2x1"].ToObject<List<int>>(),
            Slots4x4p = t["4x4p"].ToObject<List<int>>(),
            Slots4x4y = t["4x4y"].ToObject<List<int>>()
        };
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
    }
}