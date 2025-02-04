using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR
[CustomEditor(typeof(MapData))]
public class MapEditor : Editor
{
    private MapData mMapData = null;
    SerializedObject mSO;        

    private void OnEnable()
    {
        mSO = serializedObject;
        mMapData = mSO.targetObject as MapData;
    }

    public override void OnInspectorGUI()
    {
        mSO.Update();

        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.LabelField("Populate Sprites");
        if (GUILayout.Button("Populate Sprites"))
        {
            mMapData.AddSprites();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Randomize");
        if (GUILayout.Button("Randomize"))
        {
            mMapData.RandomizeMapV3();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Clear Randomized Lists");
        if (GUILayout.Button("Clear Randomized Lists"))
        {
            mMapData.ClearRandomizedV2();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Save as JSON file");
        if (GUILayout.Button("Save as JSON file"))
        {
            //mMapData.WriteJsonFile(0);
            SaveJsonFile();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Read JSON file");
        if (GUILayout.Button("Read JSON file"))
        {
            //mMapData.ReadJsonTestFunc();
            LoadJsonFile();
        }

        EditorGUILayout.EndVertical();

        mSO.ApplyModifiedProperties();
    }

    private void SaveJsonFile()
    {
        var saveFile = EditorUtility.SaveFilePanel("Save as JSON file", "", "testJson.json", "json");
        if(!string.IsNullOrEmpty(saveFile))
        {
            mMapData.WriteJsonFile(0, saveFile);
            AssetDatabase.Refresh();
        }
    }

    private void LoadJsonFile()
    {
        var openFile = EditorUtility.OpenFilePanel("Load a JSON file", "", "json");
        if (!string.IsNullOrEmpty(openFile))
        {
            Debug.Log(openFile);
        }
        string allText = File.ReadAllText(openFile);
        mMapData.ReadJsonFile(allText);
    }
}
#endif
