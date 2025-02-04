using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PermutatorObject", menuName = "ScriptableObjects/PermutatorObject")]
public class PermutatorObject : ScriptableObject
{
    [SerializeField] private DistributionLibrary _distributionLibrary;

    public List<MapData> _mapData = new List<MapData>();
    public List<List<List<int>>> _combinedList = new List<List<List<int>>>();

    //The 2nd Tabulated list
    public List<List<int>> tabulatedList = new List<List<int>>();
   // private HashSet<List<int>> testSet = new HashSet<List<int>>();

    //For storing Ranges of values per map
    private List<Tuple<long, long>> mMapRanges = new List<Tuple<long, long>>();

    public ulong Permutate()
    {
        _combinedList.Clear();

        ulong counter = 0;
        SortedList slots1x1 = new SortedList();

        for (int a = 0; a < _mapData.Count; ++a)
        {
            MapData land = _mapData[a];
            slots1x1.Clear();

            List<List<int>> landList = new List<List<int>>();

            for (int t = 0; t <= land._terrainSprites.Count; ++t)
            {
                //slot 1
                for (int b = 0; b <= land._props_1x1.Count; ++b)
                {
                    if (b != 0)
                        slots1x1.Add(b, b);
                    //slot 2
                    for (int c = 0; c <= land._props_1x1.Count; ++c)
                    {
                        if (slots1x1.Contains(c))
                            continue;

                        if (c != 0)
                            slots1x1.Add(c, c);
                        //slot 3
                        for (int d = 0; d <= land._props_1x1.Count; ++d)
                        {
                            if (slots1x1.Contains(d))
                                continue;

                            if (d != 0)
                                slots1x1.Add(d, d);
                            //slot 4
                            for (int e = 0; e <= land._props_1x1.Count; ++e)
                            {
                                if (slots1x1.Contains(e))
                                    continue;

                                if (e != 0)
                                    slots1x1.Add(e, e);
                                //slot 5
                                for (int f = 0; f <= land._props_1x1.Count; ++f)
                                {
                                    if (slots1x1.Contains(f))
                                        continue;

                                    if (f != 0)
                                        slots1x1.Add(f, f);
                                    //slot 6
                                    for (int g = 0; g <= land._props_1x1.Count; ++g)
                                    {
                                        if (slots1x1.Contains(g))
                                            continue;

                                        if (g != 0)
                                            slots1x1.Add(g, g);
                                        //2x1
                                        for (int h = 0; h <= land._props_2x1.Count; ++h)
                                        {
                                            //2x2
                                            for (int i = 0; i <= land._props_2x2.Count; ++i)
                                            {
                                                List<int> newList = new List<int>();
                                                newList.Add(t);
                                                newList.Add(b);
                                                newList.Add(c);
                                                newList.Add(d);
                                                newList.Add(e);
                                                newList.Add(f);
                                                newList.Add(g);
                                                newList.Add(h);
                                                newList.Add(i);
                                                landList.Add(newList);
                                                ++counter;
                                            }
                                        }
                                        slots1x1.Remove(g);
                                    }
                                    slots1x1.Remove(f);
                                }
                                slots1x1.Remove(e);
                            }
                            slots1x1.Remove(d);
                        }
                        slots1x1.Remove(c);
                    }
                    slots1x1.Remove(b);
                }
            }
            _combinedList.Add(landList);
        }

        return counter;
    }
    public long Tabulate(int total)
    {
        tabulatedList.Clear();
        //testSet.Clear();

        long totalCombination = CalculateMaxNumbers();
        System.Random rand = new System.Random();
        SortedList indexes = new SortedList();
        for (int i = 0; i < total; ++i)
        {
            int mapIndex = _distributionLibrary.PickAMapNumber();
            long min = mMapRanges[mapIndex].Item1;
            long max = mMapRanges[mapIndex].Item2;
            long randomIndex = LongRandom(min, max, rand);
            while (indexes.Contains(randomIndex))
            {
                randomIndex = LongRandom(min, max, rand);
            }
            List<int> newList = GetListFromIndex(randomIndex);

            tabulatedList.Add(newList);
            //testSet.Add(newList);
            indexes.Add(randomIndex, randomIndex);
        }

        return totalCombination;
    }
    private long LongRandom(long min, long max, System.Random rand)
    {
        long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
        result = (result << 32);
        result = result | (long)rand.Next((Int32)min, (Int32)max);
        return result;
    }

    private List<int> GetListFromIndex(long index)
    {
        List<int> newList = new List<int>();
        long currentIndex = index;
        long previous = 0;
        for (int a = 0; a < _mapData.Count; ++a)
        {
            MapData land = _mapData[a];
            long slot1x1SpriteCount = (long)land._props_1x1.Count + 1;
            long slot2x1SpriteCount = (long)land._props_2x1.Count + 1;
            long slot2x2SpriteCount = (long)land._props_2x2.Count + 1;
            long currentCount = 1;
            for (int i = 0; i < 6; ++i)
            {
                currentCount *= slot1x1SpriteCount;
                --slot1x1SpriteCount;
                if (slot1x1SpriteCount == 0)
                    break;
            }
            currentCount *= slot2x1SpriteCount;
            currentCount *= slot2x2SpriteCount;

            long totalCount = currentCount * land._terrainSprites.Count;
            if (currentIndex >= previous && currentIndex < previous + totalCount)
            {
                currentIndex -= previous;

                int bIndex = (int)(currentIndex / currentCount);

                currentIndex %= currentCount;
                long slotSize = GetSlot1To5Size(1, land._props_1x1.Count + 1) * slot2x1SpriteCount * slot2x2SpriteCount;
                int slot1Index = (int)(currentIndex / slotSize);

                currentIndex %= slotSize;
                slotSize = GetSlot1To5Size(2, land._props_1x1.Count + 1) * slot2x1SpriteCount * slot2x2SpriteCount;
                int slot2Index = (int)(currentIndex / slotSize);

                currentIndex %= slotSize;
                slotSize = GetSlot1To5Size(3, land._props_1x1.Count + 1) * slot2x1SpriteCount * slot2x2SpriteCount;
                int slot3Index = (int)(currentIndex / slotSize);

                currentIndex %= slotSize;
                slotSize = GetSlot1To5Size(4, land._props_1x1.Count + 1) * slot2x1SpriteCount * slot2x2SpriteCount;
                int slot4Index = (int)(currentIndex / slotSize);

                currentIndex %= slotSize;
                slotSize = GetSlot1To5Size(5, land._props_1x1.Count + 1) * slot2x1SpriteCount * slot2x2SpriteCount;
                int slot5Index = (int)(currentIndex / slotSize);

                currentIndex %= slotSize;
                slotSize = GetSlot1To5Size(6, land._props_1x1.Count + 1) * slot2x1SpriteCount * slot2x2SpriteCount;
                int slot6Index = (int)(currentIndex / slotSize);

                currentIndex %= slotSize;
                int slot2x1 = (int)(currentIndex / slot2x2SpriteCount);

                int slot2x2 = (int)(currentIndex % slot2x2SpriteCount);
                newList.Clear();
                newList.Add(a);
                newList.Add(bIndex);
                newList.Add(slot1Index);
                newList.Add(slot2Index);
                newList.Add(slot3Index);
                newList.Add(slot4Index);
                newList.Add(slot5Index);
                newList.Add(slot6Index);
                newList.Add(slot2x1);
                newList.Add(slot2x2);

                break;
            }            

            previous += totalCount;
        }

        return newList;
    }

    private long GetSlot1To5Size(int slotNumber, long spriteCount)
    {
        switch (slotNumber)
        {
            case 1:
                return (spriteCount - 1) * (spriteCount - 2) * (spriteCount - 3) * (spriteCount - 4) * (spriteCount - 5);
            case 2:
                return (spriteCount - 2) * (spriteCount - 3) * (spriteCount - 4) * (spriteCount - 5);
            case 3:
                return (spriteCount - 3) * (spriteCount - 4) * (spriteCount - 5);
            case 4:
                return (spriteCount - 4) * (spriteCount - 5);
            case 5:
                return (spriteCount - 5);
            case 6:
                return 1;
            default:
                return 0;
        }
    }

    private long CalculateMaxNumbers()
    {
        long count = 0;
        long minRange = 0;
        for (int a = 0; a < _mapData.Count; ++a)
        {
            MapData land = _mapData[a];
            long slot1x1SpriteCount = (long)land._props_1x1.Count + 1;
            long slot2x1SpriteCount = (long)land._props_2x1.Count + 1;
            long slot2x2SpriteCount = (long)land._props_2x2.Count + 1;
            long currentCount = 1;
            for (int i = 0; i < 6; ++i)
            {
                currentCount *= slot1x1SpriteCount;
                --slot1x1SpriteCount;
                if (slot1x1SpriteCount == 0)
                    break;
            }
            currentCount *= slot2x1SpriteCount;
            currentCount *= slot2x2SpriteCount;
            currentCount *= (long)land._terrainSprites.Count;
            count += currentCount;
            mMapRanges.Add(new Tuple<long, long>(minRange, count));
            minRange = count;
        }

        return count;
    }
}
