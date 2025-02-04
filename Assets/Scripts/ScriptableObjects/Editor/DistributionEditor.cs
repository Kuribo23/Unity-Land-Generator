using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(DistributionLibrary))]
public class DistributionEditor : Editor
{
    private DistributionLibrary mDistribution = null;

    SerializedObject mSO;

    private void OnEnable()
    {
        mSO = serializedObject;
        mDistribution = mSO.targetObject as DistributionLibrary;
    }

    public override void OnInspectorGUI()
    {
        mSO.Update();
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.LabelField("Calculate Quantities");
        if (GUILayout.Button("Calculate Quantities"))
        {
            mDistribution.DistributeSlots();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Pick a slot");
        if (GUILayout.Button("Pick a slot"))
        {
            int chosenSlot = mDistribution.PickASlotNumber();
            Debug.Log("You chosen: " + chosenSlot);
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Distribute Artifacts");
        if (GUILayout.Button("Distribute Artifacts"))
        {
            mDistribution.DistributeArtifacts();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Pick an artifact count");
        if (GUILayout.Button("Pick an artifact count"))
        {
            int randomNum = Helper.Randomize(1.0f, 4.0f);
            Debug.Log("Your number is " + randomNum);
            int result = mDistribution.PickAnArtifactCount(randomNum);
            Debug.Log("You get " + result + " artifact(s)!!");
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Reset Artifact Rarities");
        if (GUILayout.Button("Reset Artifact  Rarities"))
        {
            mDistribution.SetArtifactRaritiesQuantities(5, 2, 3);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Pick an artifact rarity!");
        if (GUILayout.Button("Pick an artifact rarity!"))
        {
            int result = mDistribution.PickAnArtifactRarity();
            Debug.Log("You chosen: " + result);
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Distribute Maps");
        if (GUILayout.Button("Distribute Maps"))
        {
            mDistribution.DistributeMap();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Pick a map!");
        if (GUILayout.Button("Pick a map!"))
        {
            int result = mDistribution.PickAMapNumber();
            Debug.Log("You chosen map: " + result);
        }

        EditorGUILayout.EndVertical();
        mSO.ApplyModifiedProperties();
    }    
}
#endif
