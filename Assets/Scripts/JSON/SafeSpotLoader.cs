
using Newtonsoft.Json;
using System;
using UnityEngine;

public class SafeSpotLoader
{
    public SafeSpotJsonData LoadSafeSpots(String json)
    {
        SafeSpotJsonData returningData = null;
        if (json != null && json.Length > 0)
        {
            try
            {
                returningData = JsonConvert.DeserializeObject<SafeSpotJsonData>(json,
                    new JsonConverter[] { new SafeSpotJsonConverter() });

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        return returningData;
    }
}
