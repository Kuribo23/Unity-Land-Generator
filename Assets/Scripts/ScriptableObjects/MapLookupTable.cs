using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public class LookupDict : SerializableDictionary<string, string> { }

[CreateAssetMenu(fileName = "LookUpTable", menuName = "ScriptableObjects/LookUpTable")]
public class MapLookupTable : ScriptableObject
{
    public MapData _mapData;
    public LookupDict _lookupDict = new LookupDict();
    public LookupDict _reverseDict = new LookupDict();

    private TextInfo mTextInfo = new CultureInfo("en-US", false).TextInfo;

    public void GenerateLookupTable()
    {
        _lookupDict.Clear();

        //Grab all the terrain maps        
        string prefix = string.Format(Helper.TERRAIN_PREFIX, _mapData._terrainName);
        foreach (var terrain in _mapData._terrainSprites)
        {
            string subText = terrain.name.Substring(prefix.Length);
            int length = subText.Length - 3;
            if(length > 0)
            {
                subText = subText.Substring(0, length);                
                subText = String.Format("{0} {1}", _mapData._terrainName, subText);
                subText = subText.Replace('_', ' ');
            }
            _lookupDict.Add(terrain.name, mTextInfo.ToTitleCase(subText));
        }

        //Grab all the 1x1
        prefix = string.Format(Helper.PROP_PREFIX, _mapData._terrainName, "1x1");
        foreach (var prop in _mapData._props_1x1)
        {
            string subText = prop.name.Substring(prefix.Length);
            int length = subText.Length - 3;
            if (length > 0)
            {
                subText = subText.Substring(0, length); //remove _x1
                subText = subText.Replace('_', ' ');
            }
            _lookupDict.Add(prop.name, mTextInfo.ToTitleCase(subText));
        }
        //Grab all the 2x1
        prefix = string.Format(Helper.PROP_PREFIX, _mapData._terrainName, "2x1");
        foreach (var prop in _mapData._props_2x1)
        {
            string subText = prop.name.Substring(prefix.Length);
            int length = subText.Length - 3;
            if(length > 0)
            {
                subText = subText.Substring(0, length); //remove _x1
                subText = subText.Replace('_', ' ');
            }                
            _lookupDict.Add(prop.name, mTextInfo.ToTitleCase(subText));
        }
        //Grab all the 2x2
        prefix = string.Format(Helper.PROP_PREFIX, _mapData._terrainName, "2x2");
        foreach (var prop in _mapData._props_2x2)
        {
            string subText = prop.name.Substring(prefix.Length);
            int length = subText.Length - 3;
            if(length > 0)
            {
                subText = subText.Substring(0, length); //remove _x1
                subText = subText.Replace('_', ' ');
            }                
            _lookupDict.Add(prop.name, mTextInfo.ToTitleCase(subText));
        }
    }

    public void GenerateReverseLookup()
    {
        _reverseDict.Clear();
        foreach(string key in _lookupDict.Keys)
        {
            string value = _lookupDict[key];
            _reverseDict.Add(value, key);
        }
    }
}
