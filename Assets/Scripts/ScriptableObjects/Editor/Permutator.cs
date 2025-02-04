using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(PermutatorObject))]
[CanEditMultipleObjects]
public class PermutatorEdit : Editor
{
    public List<LandInfo> _landInfos = new List<LandInfo>();

    private const int MAX_PERMUTATION = 89313600;
    private Label mLabel;
    private SortedList mIndexList = new SortedList();
    PermutatorObject po;

    private void OnEnable()
    {
        po = target as PermutatorObject;
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement ve = new VisualElement();

        InspectorElement.FillDefaultInspector(ve, serializedObject, this);

        Button button = new Button();
        button.name = "permutateButton";
        button.text = "PERMUTATE LAH";
        button.RegisterCallback<ClickEvent>(PermutateFunc);
        ve.Add(button);

        mLabel = new Label();
        mLabel.text = "Count = " + po._combinedList.Count;
        ve.Add(mLabel);

        return ve;
    }

    private void PermutateFunc(ClickEvent evt)
    {
        long finalCount = po.Tabulate(10000);
       // mLabel.text = "Count = " + po._combinedList.Count;
    }

}
