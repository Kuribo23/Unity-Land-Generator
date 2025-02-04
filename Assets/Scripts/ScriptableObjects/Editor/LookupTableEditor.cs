using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(MapLookupTable))]
public class LookupTableEditor : Editor
{
    private MapLookupTable mLUT = null;

    SerializedObject mSO;

    private void OnEnable()
    {
        mSO = serializedObject;
        mLUT = mSO.targetObject as MapLookupTable;
    }

    public override void OnInspectorGUI()
    {
        mSO.Update();
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.LabelField("Generate LookupTables");
        if (GUILayout.Button("Generate LookupTables"))
        {
            mLUT.GenerateLookupTable();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Generate Reverse LookupTables");
        if (GUILayout.Button("Generate Reverse LookupTables"))
        {
            mLUT.GenerateReverseLookup();
        }

        EditorGUILayout.EndVertical();
        mSO.ApplyModifiedProperties();
    }
}
#endif
