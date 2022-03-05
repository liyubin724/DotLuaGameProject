using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class LearnEditorWindow : EditorWindow
{
    [MenuItem("Learn UIToolkit/LearnEditorWindow")]
    public static void ShowExample()
    {
        LearnEditorWindow wnd = GetWindow<LearnEditorWindow>();
        wnd.titleContent = new GUIContent("LearnEditorWindow");
    }

    [SerializeField]
    private VisualTreeAsset m_UXMLTree;

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/LearnEditorWindow.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/LearnEditorWindow.uss");
        VisualElement labelWithStyle = new Label("Hello World! With Style");
        labelWithStyle.styleSheets.Add(styleSheet);
        root.Add(labelWithStyle);

        root.Add(m_UXMLTree.Instantiate());

        Label label = new Label("These controls were created using C# code");
        root.Add(label);

        Button button = new Button();
        button.name = "button3";
        button.text = "This is button3"
    }
}