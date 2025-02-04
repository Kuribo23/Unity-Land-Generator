using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Diagnostics;

public class MapRotator : MonoBehaviour
{
    public enum ImageExportType
    { 
        kGif,
        kWebM,
        kAPng,
        kNoGenerating,
    }

    [SerializeField] private MapLibrary _mapLibrary;
    [SerializeField] private MapController _mapController;
    [SerializeField] private ImageExportType _autoExportType = ImageExportType.kGif;

    private TextAsset[] mJsonAssets;
    private List<string> jsonTextList = new List<string> ();

    private int mCounter = 17;

    //Enable to test for single map
    //[SerializeField] private TextAsset _sampleJSON;

    private void Start()
    {
        //TestSingleMap();
        //BeginJSONLoop();
        //PopulateTextList();
        //SetMapData();
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1);
        BeginJSONLoop();
    }

    public void Next()
    {
        ++mCounter;
        SetMapData();
    }

    public bool IsDone()
    {
        return mCounter >= jsonTextList.Count;
    }

    private void PopulateTextList()
    {
        string baseFolder = "OUTPUTS";

        string[] json = Directory.GetFiles(baseFolder, "*.json", SearchOption.AllDirectories);
        foreach (string jsonString in json)
        {
            jsonTextList.Add(File.ReadAllText(jsonString));
        }
    }

    private void SetMapData()
    {
        if (!IsDone())
        {
            MapData newMapData = _mapLibrary.ReadJsonFileV2(jsonTextList[mCounter]);
            _mapController.SetMapData(newMapData);
        }
    }

    //private void TestSingleMap()
    //{
    //    if (_mapLibrary != null)
    //    {
    //        _mapLibrary._jsonTestFile = _sampleJSON;
    //        MapData newMapData = _mapLibrary.ReadJsonFileV2();
    //        _mapController.SetMapData(newMapData);
    //    }
    //}

    private void BeginJSONLoop()
    {
        PopulateTextList();
        StartCoroutine(LoopThruJSON());
    }

    IEnumerator LoopThruJSON()
    {
        int counter = 0;
        while (counter < jsonTextList.Count)
        {
            MapData newMapData = _mapLibrary.ReadJsonFileV2(jsonTextList[counter++]);
            _mapController.SetMapData(newMapData);

            yield return new WaitForSeconds(5);
        }
        if (_autoExportType == ImageExportType.kGif)
            Helper.StartGifBatchProcess();
        else if(_autoExportType == ImageExportType.kWebM)
            Helper.StartWebMBatchProcess();
        else if (_autoExportType == ImageExportType.kAPng)
            Helper.StartAPngBatchProcess();

        yield return null;
    }
}
