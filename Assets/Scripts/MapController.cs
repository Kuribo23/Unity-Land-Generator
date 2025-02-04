using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Comparer for comparing two keys, handling equality as beeing greater
/// Use this Comparer e.g. with SortedLists or SortedDictionaries, that don't allow duplicate keys
/// </summary>
/// <typeparam name="TKey"></typeparam>
public class DuplicateKeyComparer<TKey>:IComparer<TKey> where TKey : IComparable
{
    #region IComparer<TKey> Members

    public int Compare(TKey x, TKey y)
    {
        int result = x.CompareTo(y);

        if (result >= 0)
            return 1; // Handle equality as being greater. Note: this will break Remove(key) or
        else          // IndexOfKey(key) since the comparer never returns 0 to signal key equality
            return result;
    }

    #endregion
}

public class MapController : MonoBehaviour
{
    [Header("Grid Calculator")]
    public GridCalculator _gridCalculator;

    [Header("ScriptableObject")]
    public List<MapData> _maps = new List<MapData>();

    [Header("Map Sprite Renderer")]
    public SpriteRenderer _mapRenderer;

    [Header("Prop 1x1 Sprite Renderers")]
    public SpriteRenderer _prop1x1Renderer1;
    public SpriteRenderer _prop1x1Renderer2;
    public SpriteRenderer _prop1x1Renderer3;
    public SpriteRenderer _prop1x1Renderer4;
    public SpriteRenderer _prop1x1Renderer5;
    public SpriteRenderer _prop1x1Renderer6;
    [Header("Prop 2x1 Sprite Renderers")]
    public SpriteRenderer _prop2x1Renderer;
    [Header("Prop 2x2 Sprite Renderers")]
    public SpriteRenderer _prop2x2Renderer;
    [Header("Prop Monster Den Sprite Renderers")]
    public SpriteRenderer _propMonsterDenRenderer;

    [Header("The chosen one")]
    public MapData _chosen;

    [Header("Weather Effect")]
    public Animator _weatherAnimator;

    [Header("Frame")]
    public SpriteRenderer _frameRenderer;

    [Header("Artifacts")]
    public List<SpriteRenderer> _artifactSlotRenderer;
    public List<SpriteRenderer> _artifactRenderers;

    [Header("Slime")]
    public Animator _slimeAnimator;

    [Header("Monster")]
    public Animator _monsterAnimator;

    [Header("Screen Capture")]
    public FrameCaptureController _frameCaptureController;

    SortedList<float, SpriteRenderer> mSortedSprites = new SortedList<float, SpriteRenderer>(new DuplicateKeyComparer<float>());
    private string mMonDenDirection;
    private List<Vector2> mSafeSpots;

    // Start is called before the first frame update
    void Start()
    {
        //DelayStart();
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1);
        Randomize();
    }

    public IEnumerator DelayLoadSingleMap(MapData mapData)
    {
        yield return new WaitForSeconds(1);
        LoadSingleMap(mapData);
        yield return new WaitForSeconds(5);
        Helper.StartSingleGifProcess();
    }

    public void SetMapData(MapData newData)
    {
        SetMapDataInternal(newData);
        _frameCaptureController.SetFrameName(_chosen);
        _frameCaptureController.startRecording = true;
    }

    public void LoadSingleMap(MapData newData)
    {
        SetMapDataInternal(newData);
        //Find the single SINGLE PATH
        _frameCaptureController.SetSingleName();
        _frameCaptureController.startRecording = true;
    }

    private void SetMapDataInternal(MapData newData)
    {
        ClearMap();
        _chosen = newData;

        SetSlots();
        SetSpritesToSlots();
        SetSafeSpots();

        //For testing
        _mapRenderer.sprite = _chosen._randomMap;
        _frameRenderer.sprite = _chosen._selectedFrameSprite;
        _artifactSlotRenderer.ForEach(x => x.enabled = false);
        for (int i = 0; i < _chosen._artifactSlots; i++)
        {
            _artifactSlotRenderer[i].enabled = true;
        }
        for (int i = 0; i < _chosen._randomArtifactSprites.Count; i++)
        {
            _artifactRenderers[i].sprite = _chosen._randomArtifactSprites[i];
        }
        //List<RuntimeAnimatorController> animators = _chosen._weatherAnimators;
        //for (int i = 0; i < animators.Count; i++)
        //{
        //    _weatherAnimators[i].runtimeAnimatorController = animators[i];
        //}
        _weatherAnimator.runtimeAnimatorController = _chosen._selectedWeatherAnimator;

        RuntimeAnimatorController rimuru = _chosen._chosenSlimeAnimator;
        if (rimuru != null)
        {
            _slimeAnimator.runtimeAnimatorController = rimuru;

            if (mSafeSpots != null && mSafeSpots.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, mSafeSpots.Count - 1);
                Vector3 worldPos = _gridCalculator.GridToWorldSpace(mSafeSpots[randomIndex]);
                _slimeAnimator.gameObject.transform.position = worldPos;
                mSortedSprites.Add(worldPos.y,
                    _slimeAnimator.GetComponent<SpriteRenderer>());
                mSafeSpots.RemoveAt(randomIndex);
            }
            else
            {
                mSortedSprites.Add(_slimeAnimator.gameObject.transform.position.y,
                    _slimeAnimator.GetComponent<SpriteRenderer>());
            }
        }
        else
            _slimeAnimator.runtimeAnimatorController = null;

        if (_chosen._hasDen)
        {
            _monsterAnimator.runtimeAnimatorController = _chosen._monsterAnimator;
            if (mSafeSpots != null && mSafeSpots.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, mSafeSpots.Count - 1);
                Vector3 worldPos = _gridCalculator.GridToWorldSpace(mSafeSpots[randomIndex]);
                _monsterAnimator.gameObject.transform.position = worldPos;
                mSortedSprites.Add(worldPos.y,
                    _monsterAnimator.GetComponent<SpriteRenderer>());
                mSafeSpots.RemoveAt(randomIndex);
            }
            else
            {
                mSortedSprites.Add(_monsterAnimator.gameObject.transform.position.y,
                    _monsterAnimator.GetComponent<SpriteRenderer>());
            }
        }
        else
            _monsterAnimator.runtimeAnimatorController = null;

        mSortedSprites.Add(_slimeAnimator.gameObject.transform.position.y,
            _slimeAnimator.GetComponent<SpriteRenderer>());
        SetZorder();
    }

    public void Randomize()
    {
        ClearMap();
        // int random = UnityEngine.Random.Range(0, _maps.Count - 1);
        int random =3;
        _chosen = _maps[random];
        _chosen.RandomizeMapV2();
        _chosen = _maps[3];

        SetSlots();
        SetSpritesToSlots();
        SetSafeSpots();

        _mapRenderer.sprite = _chosen._randomMap;
        _frameRenderer.sprite = _chosen._selectedFrameSprite;
        _artifactSlotRenderer.ForEach(x => x.enabled = false);
        for(int i = 0; i < _chosen._artifactSlots; i++)
        {
            _artifactSlotRenderer[i].enabled = true;
        }
        for(int i = 0; i < _chosen._randomArtifactSprites.Count; i++)
        {
            _artifactRenderers[i].sprite = _chosen._randomArtifactSprites[i];
        }
        //List<RuntimeAnimatorController> animators = _chosen._weatherAnimators;
        //for(int i = 0; i < animators.Count; i++)
        //{
        //    _weatherAnimators[i].runtimeAnimatorController = animators[i];
        //}
        _weatherAnimator.runtimeAnimatorController = _chosen._selectedWeatherAnimator;

        RuntimeAnimatorController rimuru = _chosen._chosenSlimeAnimator;
        if (rimuru != null)
        {
            _slimeAnimator.runtimeAnimatorController = rimuru;

            if (mSafeSpots != null && mSafeSpots.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, mSafeSpots.Count - 1);
                Vector3 worldPos = _gridCalculator.GridToWorldSpace(mSafeSpots[randomIndex]);
                _slimeAnimator.gameObject.transform.position = worldPos;
                mSortedSprites.Add(worldPos.y,
                    _slimeAnimator.GetComponent<SpriteRenderer>());
                mSafeSpots.RemoveAt(randomIndex);
            }
            else
            {
                mSortedSprites.Add(_slimeAnimator.gameObject.transform.position.y,
                    _slimeAnimator.GetComponent<SpriteRenderer>());
            }
        }

        if(_chosen._hasDen)
        {
            _monsterAnimator.runtimeAnimatorController = _chosen._monsterAnimator;
            if (mSafeSpots != null && mSafeSpots.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, mSafeSpots.Count - 1);
                Vector3 worldPos = _gridCalculator.GridToWorldSpace(mSafeSpots[randomIndex]);
                _monsterAnimator.gameObject.transform.position = worldPos;
                mSortedSprites.Add(worldPos.y,
                    _monsterAnimator.GetComponent<SpriteRenderer>());
                mSafeSpots.RemoveAt(randomIndex);
            }
            else
            {
                mSortedSprites.Add(_monsterAnimator.gameObject.transform.position.y,
                    _monsterAnimator.GetComponent<SpriteRenderer>());
            }
        }
        else
            _monsterAnimator.runtimeAnimatorController = null;

        SetZorder();

        _frameCaptureController.SetFrameName(_chosen);
        _frameCaptureController.startRecording = true;
    }

    private void ClearMap()
    {
        _mapRenderer.sprite = null;
        _frameRenderer.sprite = null;
        _artifactSlotRenderer.ForEach(x => x.enabled = false);
        _prop1x1Renderer1.sprite = null;
        _prop1x1Renderer2.sprite = null;
        _prop1x1Renderer3.sprite = null;
        _prop1x1Renderer4.sprite = null;
        _prop1x1Renderer5.sprite = null;

        _prop2x1Renderer.sprite = null;

        _prop2x2Renderer.sprite = null;

        _propMonsterDenRenderer.sprite = null;
        if(mSafeSpots != null)
        {
            mSafeSpots.Clear();
            mSafeSpots = null;
        }

        _weatherAnimator.runtimeAnimatorController = null;
    }

    //private void OnValidate()
    //{
    //    if (_chosen != null)
    //    {
    //        //_chosen.RandomizeMap();
    //        if(_chosen._randomMap == null)
    //            _chosen.RandomizeMapV2(false);
    //        _mapRenderer.sprite = _chosen._randomMap;
    //    }    
    //    else
    //    {
    //        _mapRenderer.sprite = null;
    //    }
    //}

    private void SetSlots()
    {
        TextAsset overlayAsset = _chosen._corrOverlayJSON;
        OverlayLoader loader = new OverlayLoader();
        OverlayJson overlayData = loader.LoadOverlay(overlayAsset.text);
        List<Vector2> slots1x1 = overlayData.Slot1x1Pos();
        mSortedSprites.Clear();
        Vector3 worldPos;
        if (slots1x1 != null)
        {
            worldPos = _gridCalculator.GridToWorldSpace(slots1x1[0]);
            _prop1x1Renderer1.gameObject.transform.position = worldPos;
            mSortedSprites.Add(worldPos.y, _prop1x1Renderer1);

            worldPos = _gridCalculator.GridToWorldSpace(slots1x1[1]);
            _prop1x1Renderer2.gameObject.transform.position = worldPos;
            mSortedSprites.Add(worldPos.y, _prop1x1Renderer2);

            worldPos = _gridCalculator.GridToWorldSpace(slots1x1[2]);
            _prop1x1Renderer3.gameObject.transform.position = worldPos;
            mSortedSprites.Add(worldPos.y, _prop1x1Renderer3);

            worldPos = _gridCalculator.GridToWorldSpace(slots1x1[3]);
            _prop1x1Renderer4.gameObject.transform.position = worldPos;
            mSortedSprites.Add(worldPos.y, _prop1x1Renderer4);

            worldPos = _gridCalculator.GridToWorldSpace(slots1x1[4]);
            _prop1x1Renderer5.gameObject.transform.position = worldPos;
            mSortedSprites.Add(worldPos.y, _prop1x1Renderer5);

            worldPos = _gridCalculator.GridToWorldSpace(slots1x1[5]);
            _prop1x1Renderer6.gameObject.transform.position = worldPos;
            mSortedSprites.Add(worldPos.y, _prop1x1Renderer6);
        }

        Vector2 slots2x1 = overlayData.Slot2x1Pos();
        worldPos = _gridCalculator.GridToWorldSpace(slots2x1);
        _prop2x1Renderer.gameObject.transform.position = worldPos;
        if (!overlayData.IsFacingNE())
            _prop2x1Renderer.flipX = true;
        else
            _prop2x1Renderer.flipX = false;
        mSortedSprites.Add(worldPos.y, _prop2x1Renderer);

        Vector2 slots4x4y = overlayData.Slot4x4yPos();
        worldPos = _gridCalculator.GridToWorldSpace(slots4x4y);
        _propMonsterDenRenderer.gameObject.transform.position = worldPos;
        mSortedSprites.Add(worldPos.y, _propMonsterDenRenderer);

        Vector2 slots4x4p = overlayData.Slot4x4pPos();
        worldPos = _gridCalculator.GridToWorldSpace(slots4x4p);
        _prop2x2Renderer.gameObject.transform.position = worldPos;
        mSortedSprites.Add(worldPos.y, _prop2x2Renderer);

        mMonDenDirection = overlayData.MonDenDirection();
    }

    private void SetSpritesToSlots()
    {
        _prop1x1Renderer1.sprite = _chosen._randomProps_1x1[0];        
        _prop1x1Renderer2.sprite = _chosen._randomProps_1x1[1];
        _prop1x1Renderer3.sprite = _chosen._randomProps_1x1[2];
        _prop1x1Renderer4.sprite = _chosen._randomProps_1x1[3];
        _prop1x1Renderer5.sprite = _chosen._randomProps_1x1[4];
        _prop1x1Renderer6.sprite = _chosen._randomProps_1x1[5];

        _prop2x1Renderer.sprite = _chosen._randomProps_2x1[0];

        _prop2x2Renderer.sprite = _chosen._randomProps_2x2[0];

        SetMonsterDen();
    }

    private void SetSafeSpots()
    {
        TextAsset safespotsAssets = _chosen._corrSafeSpotJSON;
        if (safespotsAssets != null)
        {
            SafeSpotLoader safeSpotLoader = new SafeSpotLoader();
            SafeSpotJsonData overlayData = safeSpotLoader.LoadSafeSpots(safespotsAssets.text);

            mSafeSpots = new List<Vector2>();
            foreach (var safeSpot in overlayData.SafeSpotsPos())
            {
                mSafeSpots.Add(safeSpot);
            }
            //mSafeSpots = overlayData.SafeSpotsPos();
        }
    }

    private void SetMonsterDen()
    {
        if (!_chosen._hasDen)
            return;

        switch (mMonDenDirection.ToLower())
        {
            case "any":
                int probInt = Helper.Randomize(0.0f, ((float)_chosen._monDenDict.Count - 1));
                string key = _chosen._monDenDict.ElementAt(probInt).Key;
                _propMonsterDenRenderer.sprite = _chosen._monDenDict[key];
                break;

            case "se":
                _propMonsterDenRenderer.sprite = _chosen._monDenDict["se"];
                break;

            case "sw":
                _propMonsterDenRenderer.sprite = _chosen._monDenDict["sw"];
                break;

            default:
                probInt = Helper.Randomize(0.0f, ((float)_chosen._monDenDict.Count - 1));
                key = _chosen._monDenDict.ElementAt(probInt).Key;
                _propMonsterDenRenderer.sprite = _chosen._monDenDict[key];
                break;
        }
    }

    private void SetZorder()
    {
        int zOrderCounter = 0;
        foreach (SpriteRenderer sr in mSortedSprites.Values)
        {
            sr.sortingOrder = mSortedSprites.Count - zOrderCounter;
            ++zOrderCounter;
        }
    }
}
