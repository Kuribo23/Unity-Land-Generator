using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class SpriteLibrary : SerializableDictionary<string, Sprite> { }

[Serializable]
public class TerrainsLibrary : SerializableDictionary<string, MapData> { }

//[Serializable]
//public class AnimatorsList : SerializableDictionary.Storage<List<RuntimeAnimatorController>> { }

[Serializable]
public class AnimatorLibrary : SerializableDictionary<string, RuntimeAnimatorController> { }

[Serializable]
public class SlimeNameLUT : SerializableDictionary<string, string> { }

[CreateAssetMenu(fileName = "Library", menuName = "ScriptableObjects/Library")]
public class MapLibrary : ScriptableObject
{
    [Header("Libraries")]
    public SpriteLibrary _frameLibrary;
    //public SpriteLibrary _artifactSlotsLibrary;
    //public SpriteLibrary _artifactLibrary;
    public SpriteLibrary _normalArtifactLibrary;
    public SpriteLibrary _rareArtifactLibrary;
    public SpriteLibrary _legendaryArtifactLibrary;
    public TerrainsLibrary _terrainLibrary;
    public AnimatorLibrary _weatherLibrary;
    public AnimatorLibrary _slimeLibrary;
    public AnimatorLibrary _monsterLibrary;


    [Header("Distributions")]
    public DistributionLibrary _distributionLibrary;

   [Header("Permutator")]
    public PermutatorObject _permutatorObject;

    [Header("Number of JSONs to be generated")]
    public int _count;
    public int _startingNo;

    public void InitAllTerrains()
    {
        foreach(MapData map in _terrainLibrary.Values)
        {
            map.AddSprites();
        }
    }

    public void ClearAllRandomizations()
    {
        foreach (MapData map in _terrainLibrary.Values)
        {
            map.ClearRandomizedV2();
        }
    }

    public void GenerateRandomJSONs()
    {
        //_distributionLibrary._totalCount = _count;
        //_distributionLibrary.DistributeSlots();
        int terrainCount = _terrainLibrary.Count;
 
        for (int i = 0; i < _count; i++)
        {
            //randomly pick a number
            int chosenIndex = Helper.Randomize(0, terrainCount - 1);
            MapData chosenMap = _terrainLibrary.ElementAt(chosenIndex).Value;
            chosenMap.RandomizeMapV2();
            chosenMap.WriteJsonFile(_startingNo++);
        }
    }

    public void GenerateRandomJSONsV2()
    {
        foreach(MapData map in _terrainLibrary.Values)
        {
            map.ClearAllVariantList();
        }

        for (int i = 0; i < _count; i++)
        {
            //randomly pick an index
            int chosenMapIndex = _distributionLibrary.PickAMapNumber();
            //Grab the map
            MapData chosenMap = _terrainLibrary.ElementAt(chosenMapIndex).Value;
            chosenMap.RandomizeMapV3();
            chosenMap.WriteJsonFile(_startingNo++);
        }
    }

    public void GenerateRandomJSONsV3()
    {
        int total = 10000;

        //First reset the distribution table
        _distributionLibrary._totalMap = total;
        _distributionLibrary._totalCount = total;
        _distributionLibrary._totalArtifacts = total;
        _distributionLibrary._totalArtifactRarities = total;
        _distributionLibrary.DistributeMap();
        _distributionLibrary.DistributeSlots();
        _distributionLibrary.DistributeArtifacts();
        _distributionLibrary.DistributeArtifactRarities();

        //Tell the permutator to tabulate
        long finalCount = _permutatorObject.Tabulate(total);
        Debug.Log("Final Count: " + finalCount);

        //reset the running numer
        _count = total;
        _startingNo = 0;

        //Now, work!!! PART 1
        foreach (var result in _permutatorObject.tabulatedList)
        {
            //1 first number represent the choen map index
            MapData chosenMap = _terrainLibrary.ElementAt(result[0]).Value;            
            chosenMap.RandomizeMapV4(result);
            string jsonString = chosenMap.WriteJsonFile(_startingNo);
            switch(chosenMap._artifactSlots)
            {
                case 1:
                    mOneSlotJsons.Add(new Tuple<string, int>(jsonString, _startingNo));
                    break;

                case 2:
                    mTwoSlotJsons.Add(new Tuple<string, int>(jsonString, _startingNo));
                    break;

                case 3:
                    mThreeSlotJons.Add(new Tuple<string, int>(jsonString, _startingNo));
                    break;

                case 4:
                    mFourSlotJons.Add(new Tuple<string, int>(jsonString, _startingNo));
                    break;
            }

            _startingNo++;
        }


        GetRandomArtifacts(5);
        JumbleFolder();
    }

    public void GenerateRandomJSONsV4()
    {
        int grandTotal = 20000;
        int subTotal = 10000;
        //Map and slots
        _distributionLibrary._totalMap = grandTotal;
        _distributionLibrary._totalCount = subTotal;
        _distributionLibrary.DistributeMap();
        _distributionLibrary.DistributeSlots();

        _startingNo = 0;

        long finalCount = _permutatorObject.Tabulate(grandTotal);
        Debug.Log("Final Count: " + finalCount);

        //Now, work!!! PART 1
        foreach (var result in _permutatorObject.tabulatedList)
        {
            //1 first number represent the choen map index
            MapData chosenMap = _terrainLibrary.ElementAt(result[0]).Value;
            chosenMap.RandomizeMapV4(result);
            string jsonString = chosenMap.WriteJsonFile(_startingNo);
            switch (chosenMap._artifactSlots)
            {
                case 1:
                    mOneSlotJsons.Add(new Tuple<string, int>(jsonString, _startingNo));
                    break;

                case 2:
                    mTwoSlotJsons.Add(new Tuple<string, int>(jsonString, _startingNo));
                    break;

                case 3:
                    mThreeSlotJons.Add(new Tuple<string, int>(jsonString, _startingNo));
                    break;

                case 4:
                    mFourSlotJons.Add(new Tuple<string, int>(jsonString, _startingNo));
                    break;
            }

            _startingNo++;

            if(_distributionLibrary._remainingCount == 0)
            {
                _distributionLibrary._totalCount = subTotal;
                _distributionLibrary.DistributeSlots();
            }
        }
    }

    private void GerRandomeArtifacts(string jsonString, int number)
    {
        //Read the data
        MapJsonFlatData mapJsonData = JsonUtility.FromJson<MapJsonFlatData>(jsonString);
        MapData found = _terrainLibrary[mapJsonData.map];
        found.ReadJsonFile(ref mapJsonData);
        found.DetermineArtifacts();
        found.WriteJsonFile(number);
    }

    public void GeneratePermutatedMapJSONs()
    {
        //if(_permutatorObject._combinedList.Count == 0)
        //{
        //    Debug.Log("Permutator List is empty");
        //    return;
        //}

        ////1. for testing randomly pick a number from 0 to permutator count
        //int chosenIndex = Helper.Randomize(0, _permutatorObject._combinedList.Count - 1);        
        //var chosenMap = _permutatorObject._combinedList[chosenIndex];
        //Debug.Log("you have chosen :" + chosenIndex + " size: " + chosenMap.Count);
        ////2. then from the map, random choose a outcome 
        //var chosenOutcomet = UnityEngine.Random.Range(0, chosenMap.Count - 1);

        //1. pick a number from the mapdistribution
        int chosenMapIndex = _distributionLibrary.PickAMapNumber();

    }

    public MapData ReadJsonFileV2(string jsonInput)
    {
        MapJsonFlatData testData = JsonUtility.FromJson<MapJsonFlatData>(jsonInput);
        MapData found = _terrainLibrary[testData.map];
        found.ReadJsonFile(ref testData);
        return found;
    }

    public void DeletaAllImagesAndGifs()
    {
        string[] pngs = Directory.GetFiles("OUTPUTS", "*.png", SearchOption.AllDirectories);
        foreach (string png in pngs)
        {
            File.Delete(png);
        }

        string[] gifs = Directory.GetFiles("OUTPUTS", "*.gif", SearchOption.AllDirectories);
        {
            foreach(string gif in gifs)
            {
                File.Delete(gif);
            }
        }

        gifs = Directory.GetFiles("ImageConversion", "*.gif", SearchOption.AllDirectories);
        {
            foreach (string gif in gifs)
            {
                File.Delete(gif);
            }
        }

        string[] webms = Directory.GetFiles("ImageConversion", "*.webm", SearchOption.AllDirectories);
        {
            foreach (string webm in webms)
            {
                File.Delete(webm);
            }
        }
    }

    #region artifacts distribution
    
    private List<MapData> mOneSlots = new List<MapData>();
    private List<MapData> mTwoSlots = new List<MapData>();
    private List<MapData> mThreeSlots = new List<MapData>();
    private List<MapData> mFourSlots = new List<MapData>();
    private List<Tuple<string, int>> mOneSlotJsons = new List<Tuple<string, int>>();
    private List<Tuple<string, int>> mTwoSlotJsons = new List<Tuple<string, int>>();
    private List<Tuple<string, int>> mThreeSlotJons = new List<Tuple<string, int>>();
    private List<Tuple<string, int>> mFourSlotJons = new List<Tuple<string, int>>();

    public void DistributeArtifacts()
    {
        mOneSlotJsons.Clear();
        mTwoSlotJsons.Clear();
        mThreeSlotJons.Clear();
        mFourSlotJons.Clear();

        int total = 10000;

        _distributionLibrary._totalArtifacts = total;
        _distributionLibrary.DistributeArtifacts();

        _distributionLibrary._totalArtifactRarities = total;
        _distributionLibrary.DistributeArtifactRarities();

        string[] jsonFiles = Directory.GetFiles("OUTPUTS", "*.json", SearchOption.AllDirectories);
        foreach (string jsonFile in jsonFiles)
        {
            //Find the number
            string numString = jsonFile.Substring(jsonFile.IndexOf(Helper.PARCEL_FILENAME_PREFIX) + Helper.PARCEL_FILENAME_PREFIX.Length, 4);
            int parcelNum = Int32.Parse(numString);

            string jsonString = File.ReadAllText(jsonFile);
            MapJsonFlatData testData = JsonUtility.FromJson<MapJsonFlatData>(jsonString);
            MapData found = _terrainLibrary[testData.map];
            found.ReadJsonFile(ref testData);
            switch (found._artifactSlots)
            {
                case 1:
                    mOneSlotJsons.Add(new Tuple<string, int>(jsonString, parcelNum));
                    break;

                case 2:
                    mTwoSlotJsons.Add(new Tuple<string, int>(jsonString, parcelNum));
                    break;

                case 3:
                    mThreeSlotJons.Add(new Tuple<string, int>(jsonString, parcelNum));
                    break;

                case 4:
                    mFourSlotJons.Add(new Tuple<string, int>(jsonString, parcelNum));
                    break;
            }
        }

        GetRandomArtifacts(5);        
    }

    private void GetRandomArtifacts(int numRounds = 1)
    {
        //determine starting index and ending index
        int split1 = mOneSlotJsons.Count / numRounds;
        int split2 = mTwoSlotJsons.Count / numRounds;
        int split3 = mThreeSlotJons.Count / numRounds;
        int split4 = mFourSlotJons.Count / numRounds;


        for (int i = 0; i < numRounds; i++)
        {
            Debug.Log("Round: " + i + " Started!");

            //One slot
            int startingIndex1 = split1 * i;
            int endingIndex1 = startingIndex1 + split1;
            Debug.Log(string.Format("Slot1 {0} - {1}", startingIndex1, endingIndex1));
            for (int j = startingIndex1; j < endingIndex1; j++)
            {                
                GerRandomeArtifacts(mOneSlotJsons[j].Item1, mOneSlotJsons[j].Item2);
            }

            //Two slots
            int startingIndex2 = split2 * i;
            int endingIndex2 = startingIndex2 + split2;
            Debug.Log(string.Format("Slot2 {0} - {1}", startingIndex2, endingIndex2));
            for (int k = startingIndex2; k < endingIndex2; k++)
            {
                GerRandomeArtifacts(mTwoSlotJsons[k].Item1, mTwoSlotJsons[k].Item2);
            }

            //Three slots
            int startingIndex3 = split3 * i;
            int endingIndex3 = startingIndex3 + split3;
            Debug.Log(string.Format("Slot3 {0} - {1}", startingIndex3, endingIndex3));
            for (int l = startingIndex3; l < endingIndex3; l++)
            {
                GerRandomeArtifacts(mThreeSlotJons[l].Item1, mThreeSlotJons[l].Item2);
            }

            //Four slots
            int startingIndex4 = split4 * i;
            int endingIndex4 = startingIndex4 + split4;
            Debug.Log(string.Format("Slot4 {0} - {1}", startingIndex4, endingIndex4));
            for (int m = startingIndex4; m < endingIndex4; m++)
            {
                GerRandomeArtifacts(mFourSlotJons[m].Item1, mFourSlotJons[m].Item2);
            }
        }

        //Now new work PART 2!!
        //foreach (var data in mOneSlotJsons)
        //{
        //    GerRandomeArtifacts(data.Item1, data.Item2);
        //}
        //foreach (var data in mTwoSlotJsons)
        //{
        //    GerRandomeArtifacts(data.Item1, data.Item2);
        //}
        //foreach (var data in mThreeSlotJons)
        //{
        //    GerRandomeArtifacts(data.Item1, data.Item2);
        //}
        //foreach (var data in mFourSlotJons)
        //{
        //    GerRandomeArtifacts(data.Item1, data.Item2);
        //}

        //Debug.Log(mOneSlotJsons.Count);
        //Debug.Log(mTwoSlotJsons.Count);
        //Debug.Log(mThreeSlotJons.Count);
        //Debug.Log(mFourSlotJons.Count);
    }

    public void AddtoArtifactSlotsList(ref List<MapData> list, ref MapData mapData) => list.Add(mapData);
    public void AddToOneSlots(ref MapData mapdata) => AddtoArtifactSlotsList(ref mOneSlots, ref mapdata);
    public void AddToTwoSlots(ref MapData mapdata) => AddtoArtifactSlotsList(ref mTwoSlots, ref mapdata);
    public void AddToThreeSlots(ref MapData mapdata) => AddtoArtifactSlotsList(ref mThreeSlots, ref mapdata);
    public void AddToFourSlots(ref MapData mapdata) => AddtoArtifactSlotsList(ref mFourSlots, ref mapdata);

    public void ResetArtifactDistribution()
    {
        _distributionLibrary.SetArtifactRaritiesQuantities(_normalArtifactLibrary.Count, _rareArtifactLibrary.Count, _legendaryArtifactLibrary.Count);
    }

    #endregion

    #region JUMBLE IT UP
    public void JumbleFolder()
    {
        string jumbledPath = Helper.OUTPUT_JUMBLED_FOLDER;
        if (!Directory.Exists(jumbledPath))
            Directory.CreateDirectory(jumbledPath);

        //Get all JsonFiles
        string[] jsonFiles = Directory.GetFiles("OUTPUTS", "*.json", SearchOption.AllDirectories);
        List<string> tempList = new List<string>();
        foreach (string jsonFile in jsonFiles)
        {
            tempList.Add(File.ReadAllText(jsonFile));
        }

        int totalCount = tempList.Count;
        int counter = 0;
        for (int i = 0; i < totalCount; i++)
        {
            //Randomly pick a number
            int randomIndex = UnityEngine.Random.Range(0, tempList.Count - 1);

            //Update the name and the image/animation
            string jsonString = tempList[randomIndex];
            MapJsonFlatData mapJsonData = JsonUtility.FromJson<MapJsonFlatData>(jsonString);
            mapJsonData.name = string.Format(Helper.PARCEL_NAME, counter);
            mapJsonData.image = string.Format(Helper.JSON_IMAGE_ANIMATION_PATH, counter);
            mapJsonData.animation_url = string.Format(Helper.JSON_IMAGE_ANIMATION_PATH, counter);
            string jumbledString = JsonUtility.ToJson(mapJsonData);

            //Save it under new filename
            string parcelFilename = string.Format(Helper.PARCEL_FILENAME, counter);            
            File.WriteAllText(string.Format("{0}/{1}.json", Helper.OUTPUT_JUMBLED_FOLDER, parcelFilename), jumbledString);
            tempList.RemoveAt(randomIndex);

            counter++;
        }
    }
    #endregion

    #region Rename Files
    public void RenameWebmFiles()
    {
        string[] webms = Directory.GetFiles("WEBM", "*.webm", SearchOption.AllDirectories);
        foreach(string webm in webms)
        {
            FileInfo webmFile = new FileInfo(webm);
            string numString = webmFile.Name.Substring(webmFile.Name.IndexOf(Helper.PARCEL_FILENAME_PREFIX) + Helper.PARCEL_FILENAME_PREFIX.Length, 5);
            Debug.Log(numString);
            File.Move(webm, string.Format("WEBM/{0}.webm", numString));
        }
    }

    public void RenameSubDirectories()
    {
        DirectoryInfo folderInfo = new DirectoryInfo("OUTPUTS/1/");
        var subDirs = folderInfo.GetDirectories();        
        foreach(DirectoryInfo subDir in subDirs)
        {
            string name = subDir.Name;
            string fullname = subDir.FullName;
            string path = fullname.Substring(0, fullname.IndexOf(Helper.PARCEL_FILENAME_PREFIX));            
            string numString = name.Substring(name.IndexOf(Helper.PARCEL_FILENAME_PREFIX) + Helper.PARCEL_FILENAME_PREFIX.Length);
            string newPath = path + numString;
            System.IO.Directory.Move(fullname, newPath);
        }
    }
    #endregion

    #region Grab the frameImages
    public void FindStillFrame()
    {
        string[] jsonFiles = Directory.GetFiles("OUTPUTS", "*.json", SearchOption.AllDirectories);
        foreach(string jsonFilePath in jsonFiles)
        {
            FileInfo jsonFilename = new FileInfo(jsonFilePath);
            Debug.Log(jsonFilename.Name);
            string numStr = jsonFilename.Name.Substring(0, jsonFilename.Name.Length - 5);

            string jsonString = File.ReadAllText(jsonFilePath);
            MapJsonFlatData jsonData = JsonUtility.FromJson<MapJsonFlatData>(jsonString);
            MapData found = _terrainLibrary[jsonData.map];
            found.ReadJsonFile(ref jsonData);
            int stillPngIndex = found._selectedWeather == "Dark Energy" ? 25 : 14;

            //find the the frame folder
            int slotCount = found._artifactSlots;
            string parcelFilename = string.Format(Helper.PARCEL_FILENAME, Helper.GetParcelNumber(found._parcelName));
            string frameFolderPath = string.Format(Helper.OUTPUT_DIST_FOLDER_FRAMES, slotCount, parcelFilename);
            
            //grab all the frames inside
            string[] pngFiles = Directory.GetFiles(frameFolderPath, "*.png", SearchOption.AllDirectories);
            string stillPng = pngFiles[stillPngIndex];
            string newImagePath = string.Format("IMAGES/{0}.png", numStr);
            File.Copy(stillPng, newImagePath);

            //rename the image path
            jsonData.image = string.Format(Helper.JSON_IMAGE_PNG_PATH, numStr);
            Debug.Log(jsonData.image);

            //save json file
            string modJsonString = JsonUtility.ToJson(jsonData);
            File.WriteAllText(jsonFilePath, modJsonString);
        }

    }
    #endregion

    #region Testing/Vetting process
    public void FindAllDenWebm()
    {
        //First find all the json files
        string[] jsonFiles = Directory.GetFiles("10K_FIRST/JSONS/", "*.json", SearchOption.AllDirectories);
        //Second find all the webm files
        string[] webmFiles = Directory.GetFiles("10K_FIRST/WEBM/", "*.webm", SearchOption.AllDirectories);
        for(int i = 0; i < jsonFiles.Length; i++)
        {
            //Read the json content
            string jsonPath = jsonFiles[i];
            //FileInfo jsonFilename = new FileInfo(jsonPath);
            string jsonString = File.ReadAllText(jsonPath);
            MapJsonFlatData jsonData = JsonUtility.FromJson<MapJsonFlatData>(jsonString);
            MapData found = _terrainLibrary[jsonData.map];
            found.ReadJsonFile(ref jsonData);
            if(found._hasDen && jsonData.map == "magic_forest")
            {
                FileInfo webmInfo = new FileInfo(webmFiles[i]);
                File.Copy(webmFiles[i], string.Format("DENDEN/{0}", webmInfo.Name));
            }
        }
    }

    public void GetThoseMF2and4Jsons()
    {
        string[] jsonFiles = Directory.GetFiles("10K_SECOND/JSONS/", "*.json", SearchOption.AllDirectories);
        for (int i = 0; i < jsonFiles.Length; i++)
        {
            string jsonPath = jsonFiles[i];
            string jsonString = File.ReadAllText(jsonPath);
            MapJsonFlatData jsonData = JsonUtility.FromJson<MapJsonFlatData>(jsonString);
            MapData found = _terrainLibrary[jsonData.map];
            found.ReadJsonFile(ref jsonData);
            if ((found._hasDen || found._hasSlime) && jsonData.map == "magic_forest" && (found._randomMapIndex == 1 || found._randomMapIndex == 3))
            {
                FileInfo jsonFilename = new FileInfo(jsonPath);
                File.Copy(jsonPath, string.Format("MF/{0}", jsonFilename.Name));
            }
        }
    }
    #endregion
}


