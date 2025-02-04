using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapJsonData
{
    [JsonProperty("name")]
    public string name = string.Empty;

    [JsonProperty("image")]
    public string imageURL = string.Empty;

    [JsonProperty("weather")]
    public string weather = string.Empty;

    [JsonProperty("terrain")]
    public TerrainJsonData terrain = new TerrainJsonData();

    [JsonProperty("slot_1x1")]
    public List<TerrainSlot> slotList_1x1 = new List<TerrainSlot>
    {
        null,
        null,
        null,
        null,
        null,
        null,
    };

    [JsonProperty("slot_2x1")]
    public List<TerrainSlot> slotList_2x1 = new List<TerrainSlot>
    {
        null,        
    };

    [JsonProperty("slot_2x2")]
    public List<TerrainSlot> slotList_2x2 = new List<TerrainSlot>
    {
        null,
    };

    [JsonProperty("monster_den")]
    public bool has_monster_den = false;
}

[Serializable]
public class TerrainJsonData
{
    [JsonProperty("name")]
    public string name;

    [JsonProperty("type")]
    public int type;
}

[Serializable]
public class TerrainSlot
{
    [JsonProperty("index")]
    public int index;

    [JsonProperty("prop")]
    public string propName;
}
