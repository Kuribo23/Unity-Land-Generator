using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class OverlayJson
{
    [JsonIgnore]
    private List<List<int> > mSlots1x1 = new List<List <int>>();

    [JsonProperty("1x1")]
    public List<List<int>> Slots1x1
    {
        get => mSlots1x1;
        set
        {
            if (mSlots1x1 != value)
                mSlots1x1 = value;
        }
    }

    [JsonIgnore]
    private List<int> mSlots2x1 = new List<int>();
    
    [JsonProperty("2x1")]
    public List<int> Slots2x1
    {
        get => mSlots2x1;
        set
        {
            if (mSlots2x1 != value)
                mSlots2x1 = value;
        }
    }

    [JsonIgnore]
    private List<int> mSlots4x4p = new List<int>();

    [JsonProperty("4x4p")]
    public List<int> Slots4x4p
    {
        get => mSlots4x4p;
        set
        {
            if (mSlots4x4p != value)
                mSlots4x4p = value;
        }
    }

    [JsonIgnore]
    private List<int> mSlots4x4y = new List<int>();

    [JsonProperty("4x4y")]
    public List<int> Slots4x4y
    {
        get => mSlots4x4y;
        set
        {
            if (mSlots4x4y != value)
                mSlots4x4y = value;
        }
    }

    public Vector2 Slot4x4yPos()
    {
        if (mSlots4x4y.Count != 0)
        {
            return new Vector2(mSlots4x4y[0], mSlots4x4y[1]);
        }

        return Vector2.zero;
    }

    public Vector2 Slot4x4pPos()
    {
        if (mSlots4x4p.Count != 0)
        {
            return new Vector2(mSlots4x4p[0], mSlots4x4p[1]);
        }

        return Vector2.zero;
    }

    public Vector2 Slot2x1Pos()
    {
        if (mSlots2x1.Count != 0)
        {
            return new Vector2(mSlots2x1[0], mSlots2x1[1]);
        }

        return Vector2.zero;
    }

    public bool IsFacingNE()
    {
        if (mSlots2x1.Count != 0)
        {
            return mSlots2x1[2] == 1;
        }
        return false;
    }

    public string MonDenDirection()
    {
        if (mSlots4x4y.Count == 3)
        {
            switch (mSlots4x4y[2])
            {
                case 0:
                    return "any";

                case 1:
                    return "se";

                case 2:
                    return "sw";
            }
        }
        return "any";
    }

    public List<Vector2> Slot1x1Pos()
    {
        if (mSlots1x1.Count != 0)
        {
            List<Vector2> newSlots = new List<Vector2>();
            for(int i = 0; i< mSlots1x1.Count; ++i )
            {
                newSlots.Add(new Vector2(mSlots1x1[i][0], mSlots1x1[i][1]));
            }
            return newSlots;
        }

        return null;
    }
}
