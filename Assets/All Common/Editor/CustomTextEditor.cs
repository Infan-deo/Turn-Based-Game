using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

[CustomEditor(typeof(Text))]
public class CustomTMPEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var component = (Text)target;

        if (!component.TryGetComponent(out TextManager _))
        {
            if (GUILayout.Button("TextManager"))
            {
                component.gameObject.AddComponent<TextManager>();
                Debug.Log("TextManager component added!");
            }
        }

        if (!component.TryGetComponent(out LocalizedText _))
        {
            if (GUILayout.Button("Localize"))
            {
                component.gameObject.AddComponent<LocalizedText>();
                Debug.Log("LocalizedText component added!");
            }
        }
    }
}