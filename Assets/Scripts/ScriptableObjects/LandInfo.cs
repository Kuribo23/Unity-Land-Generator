using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LandInfoSO", menuName = "ScriptableObjects/LandInfo")]
public class LandInfo : ScriptableObject
{
    public int _landVariantCount = 4;
    public int _1x1SlotsCount = 15;
    public int _2x1SlotsCount = 1;
    public int _2x2SlotsCount = 1;
}
