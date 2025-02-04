using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class SafeSpotJsonData
{
    [JsonIgnore]
    private List<List<int>> mSafeSpots = new List<List<int>>();

    [JsonProperty("SafeSpots")]
    public List<List<int>> SafeSpots
    {
        get => mSafeSpots;
        set
        {
            if (mSafeSpots != value)
                mSafeSpots = value;
        }
    }

    public List<Vector2> SafeSpotsPos()
    {
        if (mSafeSpots.Count != 0)
        {
            List<Vector2> safeSpots = new List<Vector2>();
            for (int i = 0; i < mSafeSpots.Count; ++i)
            {
                safeSpots.Add(new Vector2(mSafeSpots[i][0], mSafeSpots[i][1]));
            }
            return safeSpots;
        }

        return null;
    }
}
