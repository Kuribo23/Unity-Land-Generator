using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DistributionLibrary", menuName = "ScriptableObjects/DistributionLibrary")]
public class DistributionLibrary : ScriptableObject
{
    [Header("Slot Total Number")]
    public int _totalCount = 0;
    public int _remainingCount = 0;

    [Header("Slot Distribution")]
    public List<float> _slotPercentage = new List<float>();
    public List<int> _slotDistribution = new List<int>();

    [Header("Arttifact Total Number")]
    public int _totalArtifacts = 0;
    public int _remainingArtifacts = 0;

    [Header("Artifact Distribution")]
    public List<float> _artifactPercentage = new List<float>();
    public List<int> _artifactDistribution = new List<int>();

    [Header("Arttifact Rarity Total Number")]
    public int _totalArtifactRarities = 0;
    public int _remainingArtifactRarities = 0;

    [Header("Arttifact Rarity Distrbution")]
    public List<float> _artifactRarityPercentage = new List<float>();
    public List<int> _artifactRarityDistribution = new List<int>();

    [Header("Map Total Number")]
    public int _totalMap = 0;
    public int _remainingMapCount = 0;

    [Header("Map Distribution")]
    public List<float> _mapPercentage = new List<float>();
    public List<int> _mapDistribution = new List<int>();

    private void DistributeNumbers(ref int total, ref int remaining, ref List<float> percentages, ref List<int> distribution)
    {
        if (total == 0)
            return;

        distribution.Clear();

        int currTotal = 0;
        for (int i = 0; i < percentages.Count; i++)
        {
            float percent = percentages[i];
            float quantity = percent * (float)total;
            int qtyInt = Mathf.RoundToInt(quantity);
            //check for last one
            if (i + 1 != percentages.Count)
            {
                currTotal += qtyInt;
                distribution.Add(qtyInt);
            }
            else //the last one
            {
                int remainingCount = total - currTotal;
                int correctAmt = Mathf.Min(remainingCount, qtyInt);
                //If zero is the highest, which I don't know why???
                if (correctAmt == 0 && distribution[i - 1] > 1)
                {
                    distribution[i - 1]--;
                    distribution.Add(1);
                }
                else
                {
                    distribution.Add(correctAmt);
                }
            }
        }

        remaining = total;
    }

    #region Slots
    public void DistributeSlots() => DistributeNumbers(ref _totalCount, ref _remainingCount, ref _slotPercentage, ref _slotDistribution);
    
    public int PickASlotNumber()
    {
        float totalPercent = 0.0f;
        for (int i = 0; i < _slotDistribution.Count; i++)
        {
            if (_slotDistribution[i] > 0)
                totalPercent += _slotPercentage[i];
        }

        if (totalPercent == 0.0f)
            return 1;

        float probability = Random.Range(0.0f, totalPercent);
        for (int i = 0; i < _slotDistribution.Count; i++)
        {
            if (_slotDistribution[i] > 0)
            {
                probability -= _slotPercentage[i];
                if (probability < 0.0f)
                {
                    _slotDistribution[i]--;
                    _remainingCount--;
                    return i + 1;
                }
            }
        }

        return 1;
    }
    #endregion

    #region Artifacts
    public void DistributeArtifacts() => DistributeNumbers(ref _totalArtifacts, ref _remainingArtifacts, ref _artifactPercentage, ref _artifactDistribution);

    public int PickAnArtifactCount(int numSlots)
    {
        float totalPercent = 0.0f;
        for (int i = 0; i < _artifactDistribution.Count; i++)
        {
            if (_artifactDistribution[i] > 0 && numSlots >= i)
                totalPercent += _artifactPercentage[i];
        }

        if (totalPercent == 0.0f)
        {
            Debug.Log("Total percent is 0");
            return 0;
        }            

        float probability = Random.Range(0.0f, totalPercent);
        for (int i = 0; i < _artifactDistribution.Count; i++)
        {
            if (_artifactDistribution[i] > 0 && numSlots >= i)
            {
                probability -= _artifactPercentage[i];
                if (probability < 0.0f)
                {
                    _artifactDistribution[i]--;
                    _remainingArtifacts--;
                    Debug.Log("Current Remaining: " + _remainingArtifacts);
                    return i;
                }
            }
        }

        Debug.Log("Returning 0: ");
        return 0;
    }

    public void DistributeArtifactRarities() => DistributeNumbers(ref _totalArtifactRarities, ref _remainingArtifactRarities, ref _artifactRarityPercentage, ref _artifactRarityDistribution);

    public void SetArtifactRaritiesQuantities(int normal, int rare, int legendary)
    {
        _artifactRarityDistribution[0] = normal;
        _artifactRarityDistribution[1] = rare;
        _artifactRarityDistribution[2] = legendary;
    }

    public int PickAnArtifactRarity()
    {
        float totalPercent = 0.0f;
        for (int i = 0; i < _artifactRarityDistribution.Count; i++)
        {
            if (_artifactRarityDistribution[i] > 0)
                totalPercent += _artifactRarityPercentage[i];
        }

        if (totalPercent == 0.0f)
            return 0;

        float probability = Random.Range(0.0f, totalPercent);
        for (int i = 0; i < _artifactRarityDistribution.Count; i++)
        {
            if (_artifactRarityDistribution[i] > 0)
            {
                probability -= _artifactRarityPercentage[i];
                if (probability < 0.0f)
                {
                    _artifactRarityDistribution[i]--;
                    _remainingArtifactRarities--;
                    return i;
                }
            }
        }

        return 0;
    }
    #endregion

    #region Maps
    public void DistributeMap() => DistributeNumbers(ref _totalMap, ref _remainingMapCount, ref _mapPercentage, ref _mapDistribution);

    public int PickAMapNumber()
    {
        float totalPercent = 0.0f;
        for (int i = 0; i < _mapDistribution.Count; i++)
        {
            if (_mapDistribution[i] > 0)
                totalPercent += _mapPercentage[i];
        }

        if (totalPercent == 0.0f)
            return 0;

        float probability = Random.Range(0.0f, totalPercent);
        for (int i = 0; i < _mapDistribution.Count; i++)
        {
            if (_mapDistribution[i] > 0)
            {
                probability -= _mapPercentage[i];
                if (probability < 0.0f)
                {
                    _mapDistribution[i]--;
                    _remainingMapCount--;
                    return i;
                }
            }
        }

        return 0;
    }
    #endregion
}
