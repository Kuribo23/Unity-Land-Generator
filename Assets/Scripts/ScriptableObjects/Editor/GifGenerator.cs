using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(MapRotator))]
public class GifGenerator : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement baseElement = new VisualElement();

        InspectorElement.FillDefaultInspector(baseElement, serializedObject, this);

        Button generateButton = new Button();
        generateButton.text = "Generate Gif";
        generateButton.RegisterCallback<ClickEvent>(GenerateGif);
        baseElement.Add(generateButton);

        Button generateWebMButton = new Button();
        generateWebMButton.text = "Generate WebM";
        generateWebMButton.RegisterCallback<ClickEvent>(GenerateWebM);
        baseElement.Add(generateWebMButton);

        Button generateAPngButton = new Button();
        generateAPngButton.text = "Generate APng";
        generateAPngButton.RegisterCallback<ClickEvent>(GenerateAPng);
        baseElement.Add(generateAPngButton);

        return baseElement;
    }

    private void GenerateGif(ClickEvent evt)
    {
        Helper.StartGifBatchProcess();
    }

    private void GenerateWebM(ClickEvent evt)
    {
        Helper.StartWebMBatchProcess();
    }

    private void GenerateAPng(ClickEvent evt)
    {
        Helper.StartAPngBatchProcess();
    }
}
