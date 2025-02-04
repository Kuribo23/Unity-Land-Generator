using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(MapLibrary))]
public class LibraryEditor : Editor
{
    private MapLibrary mMapLibrary = null;

    SerializedObject mSO;

    private void OnEnable()
    {
        mSO = serializedObject;
        mMapLibrary = mSO.targetObject as MapLibrary;
    }

    public override void OnInspectorGUI()
    {
        mSO.Update();
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.LabelField("Init all maps");
        if (GUILayout.Button("Init all maps"))
        {
            mMapLibrary.InitAllTerrains();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Clear all randomizations");
        if (GUILayout.Button("Clear all randomizations"))
        {
            mMapLibrary.ClearAllRandomizations();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Generate JSONs");
        if (GUILayout.Button("GENERATE AS MANY AS YOU WANT LAH!"))
        {
            //mMapLibrary.GenerateRandomJSONs();
            //mMapLibrary.GenerateRandomJSONsV2();
            mMapLibrary.GenerateRandomJSONsV4();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Distribute Artifacts");
        if (GUILayout.Button("GIVE THEM ARTIFACTS LAH!"))
        {
            //mMapLibrary.GenerateRandomJSONs();
            //mMapLibrary.GenerateRandomJSONsV2();
            mMapLibrary.DistributeArtifacts();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Jumble it up");
        if (GUILayout.Button("JUMBLE THE OUTPUT FOLDER LAH!"))
        {
            //mMapLibrary.GenerateRandomJSONs();
            //mMapLibrary.GenerateRandomJSONsV2();
            mMapLibrary.JumbleFolder();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Delete all Images");
        if (GUILayout.Button("Delete all Images"))
        {
            mMapLibrary.DeletaAllImagesAndGifs();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Find all the monsties webm! IN MF");
        if (GUILayout.Button("Find all the monsties webm! IN MF"))
        {
            //mMapLibrary.RenameWebmFiles();
            mMapLibrary.FindStillFrame();
            //mMapLibrary.RenameSubDirectories();
            //mMapLibrary.FindAllDenWebm();
            //mMapLibrary.GetThoseMF2and4Jsons();
        }

        EditorGUILayout.EndVertical();
        mSO.ApplyModifiedProperties();
    }    
}
#endif
