using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class MonDenSpritesLibrary : SerializableDictionary<string, Sprite> { }

[CreateAssetMenu(fileName = "MapDataSO", menuName = "ScriptableObjects/MapData")]
public class MapData : ScriptableObject
{
    public string _parcelName = string.Empty;    
    public string _terrainName = string.Empty;

    [Header("Dependencies")]
    public MapLookupTable _LUT;
    public MapLibrary _library;
    public DistributionLibrary _distribution;

    //Fixed
    [Header("Sprites")]
    public List<Sprite> _terrainSprites;
    public List<TextAsset> _overlayJSONS;
    public List<TextAsset> _safeSpotsJSONS;
    public List<Sprite> _props_1x1;
    public List<Sprite> _props_2x1;
    public List<Sprite> _props_2x2;
    //public List<Sprite> _monsterDen;
    public MonDenSpritesLibrary _monDenDict;

    [Header("Slots")]
    public float _maxSlots1X1 = 6;
    public float _maxSlots2X1 = 1;
    public float _maxSlots2X2 = 1;
    public float _maxSlotsDen = 1;

    [Header("Slots Probablilities")]
    public List<float> _probSlots1x1 = new List<float>()
    {
        0.5f,
        0.5f,
        0.5f,
        0.5f,
        0.5f,
        0.5f,
    };
    public List<float> _probSlots2x1 = new List<float>()
    {
        0.5f,
    };
    public List<float> _probSlots2x2 = new List<float>()
    {
        0.5f,
    };
    public float _probMonsterDen = 0.2f;

    [Header("Frame")]
    public string _selectedFrame = string.Empty;
    public Sprite _selectedFrameSprite = null;

    [Header("Artifacts")]
    public int _artifactSlots = 1;
    public char _symbol = '?';
    public List<string> _randomArtifacts = new List<string>();
    public List<Sprite> _randomArtifactSprites = new List<Sprite>()
    {
        null,
        null,
        null,
        null,
    };
    public float _expGain = 1.0f;
    public List<float> _expValues = new List<float>()
    {
        2.0f,
        1.5f,
        1.0f,
        0.5f,
    };


    [Header("Weather Probablity")]
    public float _weatherProbability = 0.5f;

    //Randomized
    [Header("Randomized Data")]
    public int _randomMapIndex = 0;
    public Sprite _randomMap;
    public TextAsset _corrOverlayJSON;
    public TextAsset _corrSafeSpotJSON;
    public List<Sprite> _randomProps_1x1 = new List<Sprite>()
    {
        null,
        null,
        null,
        null,
        null,
        null,
    };
    public List<Sprite> _randomProps_2x1 = new List<Sprite>()
    {
        null,
    };
    public List<Sprite> _randomProps_2x2 = new List<Sprite>()
    {
        null,
    };

    [Header("Monster Den Section")]
    public bool _hasDen = false;
    public string _monsterName = string.Empty;
    public RuntimeAnimatorController _monsterAnimator = null;

    [Header("Weather Section")]
    public string _selectedWeather;
    //public List<RuntimeAnimatorController> _weatherAnimators = new List<RuntimeAnimatorController>();
    public RuntimeAnimatorController _selectedWeatherAnimator = null;

    [Header("Slimes Section")]
    public bool _hasSlime = false;
    public float _slimeProbablility = 0.3f;
    public string _chosenSlimeName = string.Empty;
    public RuntimeAnimatorController _chosenSlimeAnimator = null;

    #region Random General
    private int GetRandomMap()
    {
        int chosenIndex = 0; 

        if (_terrainSprites != null)
        {
            int chosen = Helper.Randomize(0.0f, _terrainSprites.Count - 1);
            _randomMap = _terrainSprites[chosen];
            _corrOverlayJSON = _overlayJSONS[chosen];
            _corrSafeSpotJSON = _safeSpotsJSONS[chosen];

            chosenIndex = chosen;
        }

        return chosenIndex;
    }

    private void HasMonsterDen()
    {
        float probability = Random.Range(0.0f, 1.0f);
        if (probability < _probMonsterDen)
        {
            _hasDen = true;

            //then choose a monster
            int cuteMonIndex = Helper.Randomize(0.0f, _library._monsterLibrary.Count - 1);
            string cuteMonName = _library._monsterLibrary.ElementAt(cuteMonIndex).Key;
            _monsterName = cuteMonName;
            _monsterAnimator = _library._monsterLibrary[_monsterName];
        }            
        else
            _hasDen = false;
    }

    private void GetRandomWeather()
    {
        int weatherCount = _library._weatherLibrary.Count;
        //float probability = Random.Range(0, (float)weatherCount);
        //int probInt = Mathf.RoundToInt(probability);
        int probInt = Helper.Randomize(0.0f, 1.0f);
        if (probInt <= _weatherProbability) //HIT!
        {
            int randomWeatherIndex = Helper.Randomize(0, weatherCount - 1);
            _selectedWeather = _library._weatherLibrary.ElementAt(randomWeatherIndex).Key;
            //_selectedWeatherPrefab = _library._weatherLibrary[_selectedWeather];
            //foreach(var animator in _library._weatherLibrary[_selectedWeather].data)
            //{
            //    _weatherAnimators.Add(animator);
            //}
            //_weatherAnimators = _library._weatherLibrary[_selectedWeather].data;            
            _selectedWeatherAnimator = _library._weatherLibrary[_selectedWeather];
        }            
        else
        {
            _selectedWeather = "None";
            _selectedWeatherAnimator = null;            
        }
    }

    private void DetermineFrame()
    {
        int frameCount = _library._frameLibrary.Count;
        //float probability = Random.Range(0, (float)frameCount - 1);
        //float probInt = Mathf.RoundToInt(probability);        
        int probInt = Helper.Randomize(0, frameCount - 1);
        _selectedFrame = _library._frameLibrary.ElementAt(probInt).Key;
        _selectedFrameSprite = _library._frameLibrary[_selectedFrame];
    }

    private void DetermineArtifactSlots()
    {
        #region version 1
        //_artifactSlots = Random.Range(1.0f, 4.0f);
        //_artifactSlots = Mathf.RoundToInt(_artifactSlots);
        #endregion

        //if(!fromDistribution)
        //{
        //    #region version 2
        //    float totalRatio = _bronzeChance + _silverChance + _goldChance + _aetherealChance;
        //    float result = Random.Range(0.0f, totalRatio);
        //    if ((result -= _bronzeChance) < 0.0f)
        //    {
        //        _artifactSlots = 1;
        //    }
        //    else if ((result -= _silverChance) < 0.0f)
        //    {
        //        _artifactSlots = 2;
        //    }
        //    else if ((result -= _goldChance) < 0.0f)
        //    {
        //        _artifactSlots = 3;
        //    }
        //    else
        //    {
        //        _artifactSlots = 4;
        //    }
        //    #endregion
        //}
        //else
        {
            #region version 3 - From Distribution Library
            _artifactSlots = _distribution.PickASlotNumber();
            _expGain = _expValues[_artifactSlots - 1];
            #endregion
        }
    }

    public void DetermineArtifacts()
    {
        List<string> normalRarities = new List<string>();
        foreach(string name in _library._normalArtifactLibrary.Keys)
            normalRarities.Add(name);

        List<string> raraRarities = new List<string>();
        foreach(string name in _library._rareArtifactLibrary.Keys)
            raraRarities.Add(name);

        List<string> legendaryRarities = new List<string>();
        foreach(string name in _library._legendaryArtifactLibrary.Keys)
            legendaryRarities.Add(name);

        try
        {
            //FIRST THING FIRST, RESET THE DISTRIBUTION NUMBER
            _library.ResetArtifactDistribution();

            //1. Pick a number for the number of artifacts you are getting!!
            int artifactCount = _distribution.PickAnArtifactCount(_artifactSlots);
            for (int i = 0; i < artifactCount; i++)
            {
                //1. pick an artifact. Higher rarity =  lower chance
                int rarity = _distribution.PickAnArtifactRarity();
                switch (rarity)
                {
                    case 0: //Normal Rarity
                        int chosenNormalArtifact = Helper.Randomize(0.0f, normalRarities.Count - 1);
                        string normalSpriteName = normalRarities[chosenNormalArtifact];
                        Debug.Log(normalSpriteName);
                        _randomArtifacts.Add(normalSpriteName);
                        _randomArtifactSprites[i] = _library._normalArtifactLibrary[normalSpriteName];
                        normalRarities.RemoveAt(chosenNormalArtifact);
                        break;

                    case 1: //Rare Rarity
                        int chosenRareArtifact = Helper.Randomize(0.0f, raraRarities.Count - 1);
                        string rareSpriteName = raraRarities[chosenRareArtifact];
                        Debug.Log(rareSpriteName);
                        _randomArtifacts.Add(rareSpriteName);
                        _randomArtifactSprites[i] = _library._rareArtifactLibrary[rareSpriteName];
                        raraRarities.RemoveAt(chosenRareArtifact);
                        break;

                    case 2: //Legendary Rarity
                        int chosenLegendaryArtifact = Helper.Randomize(0.0f, legendaryRarities.Count - 1);
                        string legendarySpriteName = legendaryRarities[chosenLegendaryArtifact];
                        Debug.Log(legendarySpriteName);
                        _randomArtifacts.Add(legendarySpriteName);
                        _randomArtifactSprites[i] = _library._legendaryArtifactLibrary[legendarySpriteName];
                        legendaryRarities.RemoveAt(chosenLegendaryArtifact);
                        break;
                }
            }
        }    
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    private string DetermineSlime()
    {
        string slimeName = Helper.JSON_ATTRIBUTE_NONE;

        if (_hasSlime)
            return slimeName;

        //1. roll for slime appearance
        float slimeAppearingProb = Random.Range(0.0f, 1.0f);
        //if YES, move on
        if(slimeAppearingProb <= _slimeProbablility)
        {
            _hasSlime = true;

            //Version 1
            int chosenSlimeIndex = Helper.Randomize(0.0f, (float)_library._slimeLibrary.Count - 1);
            _chosenSlimeName = _library._slimeLibrary.ElementAt(chosenSlimeIndex).Key;
            _chosenSlimeAnimator = _library._slimeLibrary[_chosenSlimeName];
        }
        else
        {
            _hasSlime = false;
            _chosenSlimeName = string.Empty;
            _chosenSlimeAnimator = null;
        }

        return slimeName;
    }

    #endregion

    #region Initialization
    public void AddSprites()
    {
        LoadMapSprites();
        LoadProp1X1Sprites();
        LoadProp2X1Sprites();
        LoadProp2X2Sprites();
        //LoadPropMonserDenSprites();
        LoadMonsterDenSprites();
    }

    private void LoadMapSprites()
    {
        _terrainSprites.Clear();

        //From the name, grab the map folder
        string mapFolderPath = string.Format(Helper.MAP_FOLDERPATH, _terrainName);
        Sprite[] allSprites = Resources.LoadAll<Sprite>(mapFolderPath);
        foreach (Sprite sprite in allSprites)
        {
            if (!sprite.name.Contains("_slots_"))
            {
                _terrainSprites.Add(sprite);
            }
        }
    }

    private void LoadProp1X1Sprites() => LoadPropSprites(Helper.PROP_1X1_FOLDERPATH, ref _props_1x1);
    private void LoadProp2X1Sprites() => LoadPropSprites(Helper.PROP_2X1_FOLDERPATH, ref _props_2x1);
    private void LoadProp2X2Sprites() => LoadPropSprites(Helper.PROP_2X2_FOLDERPATH, ref _props_2x2);
    //private void LoadPropMonserDenSprites() => LoadPropSprites(Helper.PROP_MONSTER_DEN_FOLDERPATH, ref _monsterDen);

    private void LoadPropSprites(string folderPath, ref List<Sprite> spriteList)
    {
        spriteList.Clear();

        //From the name, grab the map folder
        string mapFolderPath = string.Format(folderPath, _terrainName);
        Sprite[] allSprites = Resources.LoadAll<Sprite>(mapFolderPath);
        foreach (Sprite sprite in allSprites)
        {
            spriteList.Add(sprite);
        }
    }

    private void LoadMonsterDenSprites()
    {
        _monDenDict.Clear();
        string mapFolderPath = string.Format(Helper.PROP_MONSTER_DEN_FOLDERPATH, _terrainName);
        string prefix = string.Format(Helper.MONSTER_DEN_PREFIX, _terrainName);
        Sprite[] allSprites = Resources.LoadAll<Sprite>(mapFolderPath);
        foreach (Sprite sprite in allSprites)
        {
            string subString = sprite.name.Substring(prefix.Length);
            subString = subString.Substring(0, subString.Length - 3); //remove _x1
            _monDenDict.Add(subString, sprite);
        }
    }
    #endregion


    #region Version 1
    public void RandomizeMap()
    {
        GetRandomMap();
        GetRandom1x1Props();
        GetRandom2x1Props();
        GetRandom2x2Props();
        HasMonsterDen();
    }

    public void GetRandom1x1Props() => GetRandomProps(_props_1x1, _maxSlots1X1, ref _randomProps_1x1);
    public void GetRandom2x1Props() => GetRandomProps(_props_2x1, _maxSlots2X1, ref _randomProps_2x1);
    public void GetRandom2x2Props() => GetRandomProps(_props_2x2, _maxSlots2X2, ref _randomProps_2x2);

    private void GetRandomProps(List<Sprite> input, float maxNum, ref List<Sprite> output)
    {
        List<Sprite> targetProps = input;
        if (targetProps == null || targetProps.Count == 0)
            return;

        //float maxProp = Random.Range(0.0f, maxNum);        
        //int maxPropInt = Mathf.RoundToInt(maxProp);
        int maxPropInt = Helper.Randomize(0, maxNum);
      
        output.Clear();
        for (int i = 0; i < maxPropInt; i++)
        {            
            int chosenNum = Random.Range(0, targetProps.Count - 1);            
            //make sure chosen list doesn't contains the index
            while(output.Contains(targetProps[chosenNum]))
                chosenNum = Random.Range(0, targetProps.Count - 1);            
            output.Add(targetProps[chosenNum]);
        }
    }

    public void ClearRandomized()
    {
        _randomMap = null;
        _randomProps_1x1.Clear();
        _randomProps_2x1.Clear();
        _randomProps_2x2.Clear();
        _hasDen = false;
    }

    #endregion

    #region Version 2
    public void RandomizeMapV2()
    {
        ClearRandomizedV2();

        GetRandomMap();
        GetRandom1x1PropsV2();
        GetRandom2x1PropsV2();
        GetRandom2x2PropsV2();
        HasMonsterDen();

        DetermineFrame();
        DetermineArtifactSlots();
        GetRandomWeather();
        DetermineSlime();
    }

    public string GetRandom1x1PropsV2() => GetRandomProps(_props_1x1, _maxSlots1X1, ref _probSlots1x1, ref _randomProps_1x1);
    public string GetRandom2x1PropsV2() => GetRandomProps(_props_2x1, _maxSlots2X1, ref _probSlots2x1, ref _randomProps_2x1);
    public string GetRandom2x2PropsV2() => GetRandomProps(_props_2x2, _maxSlots2X2, ref _probSlots2x2, ref _randomProps_2x2);

    private string GetRandomProps(List<Sprite> input, float maxNum, ref List<float> probs, ref List<Sprite> output)
    {
        string combined = string.Empty;

        List<Sprite> toGrab = new List<Sprite>();
        foreach(Sprite sprite in input)
        {
            toGrab.Add(sprite);
        }
        for (int i = 0; i < maxNum; i++)
        {
            float prob = Random.Range(0.0f, 1.0f);
            //If lower, we proceed to randomly pick a prop (unique??)
            if (prob <= probs[i])
            {
                //first determine the propIndex from the 'toGrab' temp list
                int chosenIndex = Helper.Randomize(0, toGrab.Count - 1);
                //Put it into output
                output[i] = toGrab[chosenIndex];                
                //Remove the chosen from 'toGrab'
                toGrab.RemoveAt(chosenIndex);

                int foundIndex = input.IndexOf(output[i]) + 1;
                combined += foundIndex.ToString();
            }
            else
            {
                combined += "0";
            }
        }

        return combined;
    }
    public void ClearRandomizedV2()
    {
        _randomMap = null;
        _corrOverlayJSON = null;
        _corrSafeSpotJSON = null;
        _randomProps_1x1.Clear();
        for (int i = 0; i < _maxSlots1X1; i++)
            _randomProps_1x1.Add(null);
        _randomProps_2x1.Clear();
        for (int i = 0; i < _maxSlots2X1; i++)
            _randomProps_2x1.Add(null);
        _randomProps_2x2.Clear();
        for (int i = 0; i < _maxSlots2X2; i++)
            _randomProps_2x2.Add(null);
        _hasDen = false;
        _monsterName = string.Empty;
        _monsterAnimator = null;
        _selectedFrame = _library._frameLibrary.ElementAt(0).Key;
        _selectedFrameSprite = _library._frameLibrary[_selectedFrame];
        _artifactSlots = 1;
        _expGain = _expValues[0];
        _randomArtifacts.Clear();
        _randomArtifactSprites.Clear();
        for(int i = 0; i < 4; i++)
            _randomArtifactSprites.Add(null);
        _selectedWeather = Helper.JSON_ATTRIBUTE_NONE;
        //_weatherAnimators.Clear();
        _selectedWeatherAnimator = null;
        _hasSlime = false;
        _chosenSlimeName = string.Empty;
        _chosenSlimeAnimator = null;
    }
    #endregion

    #region Version 3 - Brute Force Checking way
    string mCombined = string.Empty;
    //List<List<string>> variant1 = new List<List<string>>();
    //List<List<string>> variant2 = new List<List<string>>();
    //List<List<string>> variant3 = new List<List<string>>();
    //List<List<string>> variant4 = new List<List<string>>();
    HashSet<string> mVariant1 = new HashSet<string>();
    HashSet<string> mVariant2 = new HashSet<string>();
    HashSet<string> mVariant3 = new HashSet<string>();
    HashSet<string> mVariant4 = new HashSet<string>();
    public void RandomizeMapV3()
    {
        ClearRandomizedV2();

        //Check if landslots has duplicates
        int mapIndex = GetRandomMap();
        AddPropsInternal();
        //while(isDuplicate(mapIndex, ref combined))
        //{
        //    Debug.Log("CLone Found");
        //    AddPropsInternal();
        //}
        while(!AddtoVariantList(mapIndex))
        {
            Debug.Log("CLone Found in mapindex: " + mapIndex + " | value: " + mCombined);
            AddPropsInternal();
        }

        //AddtoVariantList(mapIndex, combined);

        HasMonsterDen();
        DetermineFrame();
        DetermineArtifactSlots();
        DetermineArtifacts();
        GetRandomWeather();
        DetermineSlime();
    }

    public void ClearAllVariantList()
    {
        mVariant1.Clear();
        mVariant2.Clear();
        mVariant3.Clear();
        mVariant4.Clear();
    }

    private void AddPropsInternal()
    {
        var list1 = GetRandom1x1PropsV2();
        var list2 = GetRandom2x1PropsV2();
        var list3 = GetRandom2x2PropsV2();
        //combined = new List<string>();
        //combined.AddRange(list1);
        //combined.AddRange(list2);
        //combined.AddRange(list3);

        mCombined = string.Empty;
        mCombined += list1;
        mCombined += list2;
        mCombined += list3;
    }

    private bool AddtoVariantList(int index)
    {
        switch(index)
        {
            case 0:
                return mVariant1.Add(mCombined);

            case 1:
                return mVariant2.Add(mCombined);

            case 2:
                return mVariant3.Add(mCombined);

            case 3:
                return mVariant4.Add(mCombined);
        }

        return true;
    }

    #endregion

    #region Version 4 - Daniel's Permutation Way
    public void RandomizeMapV4(List<int> result)
    {
        //0 - the map
        //1 - the variant
        //2 - 1x1 slot 1
        //3 - 1x1 slot 2
        //4 - 1x1 slot 3
        //5 - 1x1 slot 4
        //6 - 1x1 slot 5
        //7 - 1x1 slot 6
        //8 - 2x1 slot 1
        //9 - 2x2 slot 1

        ClearRandomizedV2();

        //Map variant
        _randomMap = _terrainSprites[result[1]];
        _corrOverlayJSON = _overlayJSONS[result[1]];
        _corrSafeSpotJSON = _safeSpotsJSONS[result[1]];

        //1x1 slot1
        if(result[2] > 0)
            _randomProps_1x1[0] = _props_1x1[result[2] - 1];

        //1x1 slot2
        if(result[3] > 0)
            _randomProps_1x1[1] = _props_1x1[result[3] - 1];

        //1x1 slot3
        if (result[4] > 0)
            _randomProps_1x1[2] = _props_1x1[result[4] - 1];

        //1x1 slot4
        if (result[5] > 0)
            _randomProps_1x1[3] = _props_1x1[result[5] - 1];

        //1x1 slot5
        if (result[6] > 0)
            _randomProps_1x1[4] = _props_1x1[result[6] - 1];

        //1x1 slot6
        if (result[7] > 0)
            _randomProps_1x1[5] = _props_1x1[result[7] - 1];

        //2x1 slot1
        if (result[8] > 0)
            _randomProps_2x1[0] = _props_2x1[result[8] - 1];

        //2x2 slot1
        if (result[9] > 0)
            _randomProps_2x2[0] = _props_2x2[result[9] - 1];

        HasMonsterDen();
        DetermineFrame();
        DetermineArtifactSlots();
        //DetermineArtifacts();
        GetRandomWeather();
        DetermineSlime();
    }

    private void AddPropByIndex(ref List<Sprite> input, ref List<Sprite> output, int index)
    {
        output[index] = input[index];
    }
    #endregion

    #region JSON functions
    public void ReadJsonFile(string JsonString)
    {
        MapJsonFlatData jsonData = JsonUtility.FromJson<MapJsonFlatData>(JsonString);
        ReadJsonFile(ref jsonData);
    }

    public void ReadJsonFile(ref MapJsonFlatData jsonData)
    {
        ClearRandomizedV2();

        //parcel name
        _parcelName = jsonData.name;

        Debug.Log("Reading parcel: " + _parcelName);

        //image??
        //Attributes
        foreach(AttributeJsonData att in jsonData.attributes)
        {
            switch(att.trait_type)
            {
                case Helper.JSON_ATTRIBUTE_ARTIFACT_SLOTS:
                    _artifactSlots = att.value.Length;
                    //Debug.Log("Slots: " + _artifactSlots + " | " + _filename);
                    break;

                case Helper.JSON_ATTRIBUTE_ARTIFACT_1:
                    string artifact1Name = att.value;
                    Sprite artifact1 = findArtifactSprite(artifact1Name);
                    if (_randomArtifacts.Count < 1)
                        _randomArtifacts.Add(artifact1Name);
                    else
                        _randomArtifacts[0] = artifact1Name;
                    _randomArtifactSprites[0] = artifact1;
                    break;

                case Helper.JSON_ATTRIBUTE_EXP_GAIN:
                    _expGain = float.Parse(att.value);
                    break;

                case Helper.JSON_ATTRIBUTE_ARTIFACT_2:
                    string artifact2Name = att.value;
                    Sprite artifact2 = findArtifactSprite(artifact2Name);
                    if (_randomArtifacts.Count < 2)
                        _randomArtifacts.Add(artifact2Name);
                    else
                        _randomArtifacts[1] = artifact2Name;
                    _randomArtifactSprites[1] = artifact2;
                    break;

                case Helper.JSON_ATTRIBUTE_ARTIFACT_3:
                    string artifact3Name = att.value;
                    Sprite artifact3 = findArtifactSprite(artifact3Name);
                    if (_randomArtifacts.Count < 3)
                        _randomArtifacts.Add(artifact3Name);
                    else
                        _randomArtifacts[2] = artifact3Name;
                    _randomArtifactSprites[2] = artifact3;
                    break;

                case Helper.JSON_ATTRIBUTE_ARTIFACT_4:
                    string artifact4Name = att.value;
                    Sprite artifact4 = findArtifactSprite(artifact4Name);
                    if (_randomArtifacts.Count < 4)
                        _randomArtifacts.Add(artifact4Name);
                    else
                        _randomArtifacts[3] = artifact4Name;
                    _randomArtifactSprites[3] = artifact4;
                    break;

                case Helper.JSON_ATTRIBUTE_SLIME:
                    _chosenSlimeName = att.value;
                    //Debug.Log("Slime name " + _chosenSlimeName);
                    if (!_chosenSlimeName.Equals(Helper.JSON_ATTRIBUTE_NONE))
                        _chosenSlimeAnimator = _library._slimeLibrary[_chosenSlimeName];
                    else
                        _chosenSlimeAnimator = null;
                    break;

                case Helper.JSON_ATTRIBUTE_MONSTER_DEN:
                    _hasDen = att.value == Helper.JSON_DEN_PRESENT ? true : false;
                    break;

                case Helper.JSON_ATTRIBUTE_CUTE_MONSTER:
                    _monsterName = att.value;
                    _monsterAnimator = _library._monsterLibrary[_monsterName];
                    break;

                case Helper.JSON_ATTRIBUTE_FRAME:
                    //Debug.Log("Frame: " + att.value + " | " + _filename);
                    _selectedFrame = att.value;
                    _selectedFrameSprite = _library._frameLibrary[_selectedFrame];
                    break;

                case Helper.JSON_ATTRIBUTE_WEATHER:
                    //Debug.Log("Weather: " + att.value + " | " + _filename);
                    _selectedWeather = att.value;
                    if(_selectedWeather.Equals(Helper.JSON_ATTRIBUTE_NONE))
                    {
                        _selectedWeather = Helper.JSON_ATTRIBUTE_NONE;
                        _selectedWeatherAnimator = null;
                    }
                    else
                    {
                        //foreach (var animator in _library._weatherLibrary[_selectedWeather].data)
                        //{
                        //    _weatherAnimators.Add(animator);
                        //}
                        _selectedWeatherAnimator = _library._weatherLibrary[_selectedWeather];
                    }                    
                    break;

                case Helper.JSON_ATTRIBUTE_TERRAIN:
                    //Need reverse LUT
                    string spriteName = _LUT._reverseDict[att.value];
                    for(int i = 0; i < _terrainSprites.Count; i++)
                    {
                        if (_terrainSprites[i].name.Equals(spriteName))
                        {
                            _randomMapIndex = i;
                            _randomMap = _terrainSprites[i];
                            _corrOverlayJSON = _overlayJSONS[i];
                            _corrSafeSpotJSON = _safeSpotsJSONS[i];
                            break;
                        }
                    }
                    foreach(Sprite sprite in _terrainSprites)
                    {
                        if(sprite.name.Equals(spriteName))
                        {
                            _randomMap = sprite;                            
                            break;
                        }
                    }
                    break;

                case Helper.JSON_ATTRIBUTE_SLOT_1X1_1:
                    if(att.value != Helper.JSON_ATTRIBUTE_NONE)
                    {
                        _randomProps_1x1[0] = GetPropSprite(att.value, ref _props_1x1);
                    }
                    break;

                case Helper.JSON_ATTRIBUTE_SLOT_1X1_2:
                    if (att.value != Helper.JSON_ATTRIBUTE_NONE)
                    {
                        _randomProps_1x1[1] = GetPropSprite(att.value, ref _props_1x1);
                    }
                    break;

                case Helper.JSON_ATTRIBUTE_SLOT_1X1_3:
                    if (att.value != Helper.JSON_ATTRIBUTE_NONE)
                    {
                        _randomProps_1x1[2] = GetPropSprite(att.value, ref _props_1x1);
                    }
                    break;

                case Helper.JSON_ATTRIBUTE_SLOT_1X1_4:
                    if (att.value != Helper.JSON_ATTRIBUTE_NONE)
                    {
                        _randomProps_1x1[3] = GetPropSprite(att.value, ref _props_1x1);
                    }
                    break;

                case Helper.JSON_ATTRIBUTE_SLOT_1X1_5:
                    if (att.value != Helper.JSON_ATTRIBUTE_NONE)
                    {
                        _randomProps_1x1[4] = GetPropSprite(att.value, ref _props_1x1);
                    }
                    break;

                case Helper.JSON_ATTRIBUTE_SLOT_1X1_6:
                    if (att.value != Helper.JSON_ATTRIBUTE_NONE)
                    {
                        _randomProps_1x1[5] = GetPropSprite(att.value, ref _props_1x1);
                    }
                    break;

                case Helper.JSON_ATTRIBUTE_SLOT_2X1_1:
                    if (att.value != Helper.JSON_ATTRIBUTE_NONE)
                    {
                        _randomProps_2x1[0] = GetPropSprite(att.value, ref _props_2x1);
                    }
                    break;

                case Helper.JSON_ATTRIBUTE_SLOT_2X2_1:
                    if (att.value != Helper.JSON_ATTRIBUTE_NONE)
                    {
                        _randomProps_2x2[0] = GetPropSprite(att.value, ref _props_2x2);
                    }
                    break;
            }
        }
    }

    private Sprite findArtifactSprite(string name)
    {
        if(_library._normalArtifactLibrary.Contains(name))
            return _library._normalArtifactLibrary[name];
        
        if(_library._rareArtifactLibrary.Contains(name))
            return _library._rareArtifactLibrary[name];

        if(_library._legendaryArtifactLibrary.Contains(name))
            return _library._legendaryArtifactLibrary[name];

        return null;
    }

    private Sprite GetPropSprite(string name, ref List<Sprite> list)
    {
        Sprite found = null;
        //1. get the sprite name
        string spriteFilename = _LUT._reverseDict[name];
        foreach(Sprite sprite in list)
        {
            if(sprite.name.Equals(spriteFilename))
            {
                found = sprite;
                break;
            }
        }

        return found;
    }

    public string WriteJsonFile(int counter, string singleFilePath = "")
    {
        MapJsonFlatData newJson = new MapJsonFlatData();
        //Name
        _parcelName = Helper.GetName(counter);
        newJson.name = _parcelName;
        //Image
        newJson.image = string.Format(Helper.JSON_IMAGE_ANIMATION_PATH, counter);
        //animation
        newJson.animation_url = string.Format(Helper.JSON_IMAGE_ANIMATION_PATH, counter);
        //map
        newJson.map = _terrainName;

        //Artifact Slot Attribute
        AttributeJsonData artifactSlotsData = new AttributeJsonData();
        artifactSlotsData.trait_type = Helper.JSON_ATTRIBUTE_ARTIFACT_SLOTS;
        string valueStr = string.Empty;
        for (int i = 0; i < _artifactSlots; i++)
        {
            valueStr = string.Concat(valueStr, _symbol);
        }
        artifactSlotsData.value = valueStr;
        newJson.attributes.Add(artifactSlotsData);

        //Artifacts Attribute
        for(int i = 0; i < _randomArtifacts.Count; i++)
        {

            if (string.IsNullOrEmpty(_randomArtifacts[i]))
                continue;

            string name = string.Empty;
            switch(i)
            {
                case 0:
                    name = Helper.JSON_ATTRIBUTE_ARTIFACT_1;
                    break;

                case 1:
                    name = Helper.JSON_ATTRIBUTE_ARTIFACT_2;
                    break;

                case 2:
                    name = Helper.JSON_ATTRIBUTE_ARTIFACT_3;
                    break;

                case 3:
                    name = Helper.JSON_ATTRIBUTE_ARTIFACT_4;
                    break;
            }

            AttributeJsonData artifactData = new AttributeJsonData();
            artifactData.trait_type = name;
            artifactData.value = _randomArtifacts[i];
            newJson.attributes.Add(artifactData);
        }

        //EXP Gain
        AttributeJsonData expGainData = new AttributeJsonData();
        expGainData.trait_type = Helper.JSON_ATTRIBUTE_EXP_GAIN;
        expGainData.value = _expGain.ToString();
        newJson.attributes.Add(expGainData);

        //Slime
        AttributeJsonData slimeData = new AttributeJsonData();
        slimeData.trait_type = Helper.JSON_ATTRIBUTE_SLIME;
        slimeData.value = _hasSlime ? _chosenSlimeName : Helper.JSON_ATTRIBUTE_NONE;
        newJson.attributes.Add(slimeData);

        //Monster Den
        AttributeJsonData monsterDenData = new AttributeJsonData();
        monsterDenData.trait_type = Helper.JSON_ATTRIBUTE_MONSTER_DEN;
        monsterDenData.value = _hasDen ? Helper.JSON_DEN_PRESENT : Helper.JSON_DEN_NOT_PRESENT;
        newJson.attributes.Add(monsterDenData);

        //Cute Creature
        if(_hasDen)
        {
            AttributeJsonData cutemonsterData = new AttributeJsonData();
            cutemonsterData.trait_type = Helper.JSON_ATTRIBUTE_CUTE_MONSTER;
            cutemonsterData.value = _monsterName;
            newJson.attributes.Add(cutemonsterData);
        }

        //Frame Attribute
        AttributeJsonData frameData = new AttributeJsonData();
        frameData.trait_type = Helper.JSON_ATTRIBUTE_FRAME;
        frameData.value = _selectedFrame;
        newJson.attributes.Add(frameData);

        //Weather Attribute
        AttributeJsonData weatherData = new AttributeJsonData();
        weatherData.trait_type = Helper.JSON_ATTRIBUTE_WEATHER;
        weatherData.value = _selectedWeather;
        newJson.attributes.Add(weatherData);

        //Terrain Attribute
        AttributeJsonData terrainData = new AttributeJsonData();
        terrainData.trait_type = Helper.JSON_ATTRIBUTE_TERRAIN;        
        terrainData.value = _LUT._lookupDict[_randomMap.name];
        newJson.attributes.Add(terrainData);                

        //1x1 slots
        for (int i = 0; i < _randomProps_1x1.Count; i++)
        {
            AttributeJsonData prop1x1Data = new AttributeJsonData();
            prop1x1Data.trait_type = string.Format(Helper.JSON_ATTRIBUTE_SLOT_1X1, i + 1);

            var sprite = _randomProps_1x1[i];
            prop1x1Data.value = (sprite != null ? _LUT._lookupDict[sprite.name] : Helper.JSON_ATTRIBUTE_NONE);
            newJson.attributes.Add(prop1x1Data);
        }

        //2x1 slots
        for (int i = 0; i < _randomProps_2x1.Count; i++)
        {
            AttributeJsonData prop2x1Data = new AttributeJsonData();
            prop2x1Data.trait_type = string.Format(Helper.JSON_ATTRIBUTE_SLOT_2X1, i + 1);

            var sprite = _randomProps_2x1[i];
            prop2x1Data.value = (sprite != null ? _LUT._lookupDict[sprite.name] : Helper.JSON_ATTRIBUTE_NONE);
            newJson.attributes.Add(prop2x1Data);
        }

        //2x2 slots
        for (int i = 0; i < _randomProps_2x2.Count; i++)
        {
            AttributeJsonData prop2x2Data = new AttributeJsonData();
            prop2x2Data.trait_type = string.Format(Helper.JSON_ATTRIBUTE_SLOT_2X2, i + 1);

            var sprite = _randomProps_2x2[i];
            prop2x2Data.value = (sprite != null ? _LUT._lookupDict[sprite.name] : Helper.JSON_ATTRIBUTE_NONE);
            newJson.attributes.Add(prop2x2Data);
        }

        string JsonString = JsonUtility.ToJson(newJson);
        string parcelFilename = string.Format(Helper.PARCEL_FILENAME, counter);
        string folderPath = string.Format(Helper.OUTPUT_DIST_FOLDER, _artifactSlots, parcelFilename);
        string frameFolderPath = string.Format(Helper.OUTPUT_DIST_FOLDER_FRAMES, _artifactSlots, parcelFilename);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Directory.CreateDirectory(frameFolderPath);
        }
        Debug.Log("Generating new JSON file: " + parcelFilename);
        //File.WriteAllText(string.Format(Helper.JSON_OUTPUT_PATH, _filename), JsonString);
        string finalOutput = string.IsNullOrEmpty(singleFilePath) ? string.Format("{0}/{1}.json", /*folderPath*/Helper.OUTPUT_FOLDER, parcelFilename): singleFilePath;
        File.WriteAllText(finalOutput, JsonString);

        return JsonString;
    }
    #endregion
}
