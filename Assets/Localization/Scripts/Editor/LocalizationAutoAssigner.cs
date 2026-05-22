using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class LocalizationAutoAssigner
{
    [MenuItem("Localization/Assign Localized Text")]
    public static void Assign()
    {
        int count = 0;

        TMP_Text[] tmpTexts = Resources.FindObjectsOfTypeAll<TMP_Text>();
        Text[] legacyTexts = Resources.FindObjectsOfTypeAll<Text>();

        foreach (TMP_Text text in tmpTexts)
        {
            if (ShouldSkip(text.gameObject))
                continue;

            if (text.GetComponent<LocalizedText>() != null)
                continue;

            LocalizedText localizedText = text.gameObject.AddComponent<LocalizedText>();

            localizedText.localizationKey = GenerateKey(text.gameObject.name);

            EditorUtility.SetDirty(text.gameObject);

            count++;
        }

        foreach (Text text in legacyTexts)
        {
            if (ShouldSkip(text.gameObject))
                continue;

            if (text.GetComponent<LocalizedText>() != null)
                continue;

            LocalizedText localizedText = text.gameObject.AddComponent<LocalizedText>();

            localizedText.localizationKey = GenerateKey(text.gameObject.name);

            EditorUtility.SetDirty(text.gameObject);

            count++;
        }

        Debug.Log($"Assigned LocalizedText to {count} objects");
    }

    private static bool ShouldSkip(GameObject obj)
    {
        if (EditorUtility.IsPersistent(obj))
            return false;

        if (obj.hideFlags == HideFlags.NotEditable ||
            obj.hideFlags == HideFlags.HideAndDontSave)
            return true;

        return false;
    }

    private static string GenerateKey(string objectName)
    {
        return objectName
            .Replace(" ", "_")
            .ToLower();
    }
}
