using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapJsonFlatData
{
    [JsonProperty("name")]
    public string name = string.Empty;

    [JsonProperty("image")]
    public string image = string.Empty;

    [JsonProperty("animation_url")]
    public string animation_url = string.Empty;

    [JsonProperty("map")]
    public string map = string.Empty;

    [JsonProperty("attributes")]
    public List<AttributeJsonData> attributes = new List<AttributeJsonData>();
}

[Serializable]
public class AttributeJsonData
{
    [JsonProperty("trait_type")]
    public string trait_type;
    [JsonProperty("value")]
    public string value;
}
