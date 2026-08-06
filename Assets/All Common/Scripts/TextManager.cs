using System.Reflection;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.PropertyVariants;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    private Text _textLegacy;
    private TextMeshProUGUI _textMeshProUGUI;
    bool isTmpText;

    public bool useCustomFont;

    [ShowIf(nameof(useCustomFont))]
    public TMP_FontAsset fontAsset;


    private void OnValidate()
    {
        if (TryGetComponent(out Text text))
        {
            _textLegacy = text;
        }
        else if (TryGetComponent(out TextMeshProUGUI textTMP))
        {
            _textMeshProUGUI = textTMP;
            isTmpText = true;
        }
        else
        {
            Debug.Log("Text Component missing");
        }
    }
    private void Start()
    {
        if (useCustomFont)
        {
            _textMeshProUGUI.font = fontAsset;
            _textMeshProUGUI.UpdateFontAsset();
        }
    }
    // [Button("SetLocalizeFont")]
    // public void SetLocalizeFont()
    // {
    //     GameObjectLocalizer localizer = GetComponent<GameObjectLocalizer>();
    //
    //     if (localizer == null)
    //     {
    //         localizer = gameObject.AddComponent<GameObjectLocalizer>();
    //     }
    //
    //     if (isTmpText)
    //     {
    //         TMP_Text tmp = GetComponent<TMP_Text>();
    //
    //         if (tmp == null)
    //             return;
    //
    //         SerializedObject so = new SerializedObject(localizer);
    //
    //         SerializedProperty trackedObjects =
    //             so.FindProperty("m_TrackedObjects");
    //
    //         Debug.Log($"Array Size: {trackedObjects.arraySize}");
    //
    //         for (int i = 0; i < trackedObjects.arraySize; i++)
    //         {
    //             SerializedProperty element = trackedObjects.GetArrayElementAtIndex(i);
    //
    //             Debug.Log($"Element {i}");
    //
    //             SerializedProperty copy = element.Copy();
    //             SerializedProperty end = copy.GetEndProperty();
    //
    //             // while (copy.NextVisible(true) && !SerializedProperty.EqualContents(copy, end))
    //             // {
    //             //     Debug.Log(copy.propertyPath);
    //             // }
    //         }
    //     }
    //     else
    //     {
    //         TextMesh textMesh = GetComponent<TextMesh>();
    //
    //         if (textMesh == null)
    //             return;
    //
    //         // Add font/material localization for TextMesh
    //     }
    // }
}
