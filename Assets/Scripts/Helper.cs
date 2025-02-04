using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public static class Helper
{
    public static int COUNT = 0;

    public const string PARCEL_NAME = "Aethereal Parcel #{0:D05}";
    public const string PARCEL_FILENAME = "{0:D05}";
    public const string PARCEL_FILENAME_PREFIX = "Aethereal_Parcel_";
    public const string PROP_PREFIX = "tm_env_prop_{0}_{1}_";
    public const string TERRAIN_PREFIX = "tm_env_prop_{0}_layout_";
    public const string MONSTER_DEN_PREFIX = "tm_env_prop_{0}_monster_den_";

    //MAP PATHS
    public const string MAP_FOLDERPATH = "terrains/{0}/map";

    //PROP PATHS
    //Props
    public const string PROP_1X1_FOLDERPATH = "terrains/{0}/1x1";
    public const string PROP_2X1_FOLDERPATH = "terrains/{0}/2x1";
    public const string PROP_2X2_FOLDERPATH = "terrains/{0}/2x2";
    public const string PROP_MONSTER_DEN_FOLDERPATH = "terrains/{0}/monster_den";

    //JSON ATTRIBUTE NAMES    
    public const string JSON_IMAGE_PNG_PATH = "https://cdn.tatsu.gg/static/aethereal-parcel/token/{0:D05}.png";
    public const string JSON_IMAGE_ANIMATION_PATH = "https://cdn.tatsu.gg/static/aethereal-parcel/token/{0:D05}.webm";
    public const string JSON_ATTRIBUTE_ARTIFACT_SLOTS = "Artifact Slots";
    public const string JSON_ATTRIBUTE_EXP_GAIN = "EXP Gain";
    public const string JSON_ATTRIBUTE_ARTIFACT_1 = "Artifact 1";
    public const string JSON_ATTRIBUTE_ARTIFACT_2 = "Artifact 2";
    public const string JSON_ATTRIBUTE_ARTIFACT_3 = "Artifact 3";
    public const string JSON_ATTRIBUTE_ARTIFACT_4 = "Artifact 4";
    public const string JSON_ATTRIBUTE_SLIME = "Slime";
    public const string JSON_ATTRIBUTE_MONSTER_DEN = "Monster Den";
    public const string JSON_ATTRIBUTE_CUTE_MONSTER = "Monster";
    public const string JSON_ATTRIBUTE_FRAME = "Frame";
    public const string JSON_ATTRIBUTE_WEATHER = "Weather";
    public const string JSON_ATTRIBUTE_TERRAIN = "Terrain";
    public const string JSON_ATTRIBUTE_SLOT_1X1 = "1x1 Slot {0}";
    public const string JSON_ATTRIBUTE_SLOT_2X1 = "2x1 Slot {0}";
    public const string JSON_ATTRIBUTE_SLOT_2X2 = "2x2 Slot {0}";
    public const string JSON_ATTRIBUTE_SLOT_1X1_1 = "1x1 Slot 1";
    public const string JSON_ATTRIBUTE_SLOT_1X1_2 = "1x1 Slot 2";
    public const string JSON_ATTRIBUTE_SLOT_1X1_3 = "1x1 Slot 3";
    public const string JSON_ATTRIBUTE_SLOT_1X1_4 = "1x1 Slot 4";
    public const string JSON_ATTRIBUTE_SLOT_1X1_5 = "1x1 Slot 5";
    public const string JSON_ATTRIBUTE_SLOT_1X1_6 = "1x1 Slot 6";    
    public const string JSON_ATTRIBUTE_SLOT_2X1_1 = "2x1 Slot 1";    
    public const string JSON_ATTRIBUTE_SLOT_2X2_1 = "2x2 Slot 1";
    public const string JSON_ATTRIBUTE_NONE = "None";
    public const string JSON_DEN_PRESENT = "Present";
    public const string JSON_DEN_NOT_PRESENT = "Not Present";
    public const string JSON_OUTPUT_PATH = "JSONS/{0}.json";
    
    public const string JSON_OUTPUT_DIST_PATH = "JSONS/{0}//{1}/{2}.json";

    //OUTPUT
    public const string OUTPUT_DIST_FOLDER = "OUTPUTS/{0}/{1}/"; //0 is the number of artifact slots, 1 is individual folder    
    public const string OUTPUT_DIST_FOLDER_FRAMES = "OUTPUTS/{0}/{1}/frames/";
    public const string OUTPUT_DIST_FOLDER_SINGLE = "OUTPUTS/SINGLE/";
    public const string OUTPUT_FOLDER = "OUTPUTS";
    public const string OUTPUT_JUMBLED_FOLDER = "OUTPUTS_JUMBLED";

    public static string GetNextName()
    {
        return string.Format(PARCEL_NAME, COUNT++);
    }

    public static string GetName(int counter)
    {
        return string.Format(PARCEL_NAME, counter);
    }

    public static int Randomize(int start, int end)
    {
        float probability = Random.Range((float)start, (float)end);
        return Mathf.RoundToInt(probability);
    }

    public static int Randomize(float start, float end)
    {
        float probability = Random.Range(start, end);
        return Mathf.RoundToInt(probability);
    }

    public static int GetParcelNumber(string parcelName)
    {
        int parcelNum = 0;
        string numStr = parcelName.Substring(parcelName.LastIndexOf('#') + 1);
        parcelNum = int.Parse(numStr);
        return parcelNum;
    }

    public static void StartGifBatchProcess()
    {
        string currentDir = Directory.GetCurrentDirectory();
        Process proc = new Process();
        proc.StartInfo.FileName = currentDir + "\\ImageConversion\\PngToGif_2.bat";
        proc.StartInfo.WorkingDirectory = currentDir + ".\\ImageConversion";
        proc.Start();
    }

    public static void StartSingleGifProcess()
    {
        string currentDir = Directory.GetCurrentDirectory();
        Process proc = new Process();
        proc.StartInfo.FileName = currentDir + "\\ImageConversion\\SingleGifWebm.bat";
        proc.StartInfo.WorkingDirectory = currentDir + ".\\ImageConversion";
        proc.Start();
    }

    public static void StartWebMBatchProcess()
    {
        string currentDir = Directory.GetCurrentDirectory();
        Process proc = new Process();
        proc.StartInfo.FileName = currentDir + "\\ImageConversion\\PngToWebm_2.bat";
        proc.StartInfo.WorkingDirectory = currentDir + ".\\ImageConversion";
        proc.Start();
    }

    public static void StartAPngBatchProcess()
    {
        string currentDir = Directory.GetCurrentDirectory();
        Process proc = new Process();
        proc.StartInfo.FileName = currentDir + "\\ImageConversion\\PngToAPng_2.bat";
        proc.StartInfo.WorkingDirectory = currentDir + ".\\ImageConversion";
        proc.Start();
    }
}
