
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class OverlayLoader
{
    public OverlayJson LoadOverlay(String json)
    {
        OverlayJson returningData = null;
        if (json != null && json.Length > 0)
        {
            try
            {
                returningData = JsonConvert.DeserializeObject<OverlayJson>(json,
                    new JsonConverter[] { new OverlayJsonConverter() });

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        return returningData;
    }

}
