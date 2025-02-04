using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeController : MonoBehaviour
{
    public enum Mode
    {
        Single,
        Normal,
        Recording,
        SingleRecording,
    }

    [Header("The controllers")]
    public FrameCaptureController _frameController;
    public MapController _mapController;
    public GridCalculator _gridCalculator;
    public MapRotator _mapRotator;

    [Header("The libraries")]
    public MapLibrary _library;

    public Mode _mode = Mode.Normal;
    public TextAsset _jsonFile;

    public void Start()
    {
        switch(_mode)
        {
            case Mode.Single:
                if(_jsonFile != null)
                {
                    MapData singleMap = _library.ReadJsonFileV2(_jsonFile.text);
                    _mapController.SetMapData(singleMap);
                }
                break;

            case Mode.SingleRecording:
                if (_jsonFile != null)
                {
                    MapData singleMap = _library.ReadJsonFileV2(_jsonFile.text);
                    StartCoroutine(_mapController.DelayLoadSingleMap(singleMap));
                }
                break;
        }
    }

    private void OnValidate()
    {
        switch( _mode )
        {
            case Mode.Single:
                _frameController.enabled = false;
                _gridCalculator.enabled = false;
                _mapRotator.enabled = false;
                break;

            case Mode.Normal:
                _frameController.enabled = false;
                _gridCalculator.enabled = true;
                _mapRotator.enabled = true;
                break;

            case Mode.Recording:
                _frameController.enabled = true;
                _gridCalculator.enabled = true;
                _mapRotator.enabled = true;
                break;

            case Mode.SingleRecording:
                _frameController.enabled = true;
                _gridCalculator.enabled = true;
                _mapRotator.enabled = false;
                break;

            default:
                break;
        }
    }
}
