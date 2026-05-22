using UnityEditor;
using UnityEngine;
using TMPro;

[InitializeOnLoad]
public static class TMPAutoSetup
{
    static TMPAutoSetup()
    {
        ObjectFactory.componentWasAdded += ComponentWasAdded;
    }

    private static void ComponentWasAdded(Component component)
    {
        // Check if added component is TMP text
        if (component is TextMeshProUGUI tmp)
        {
            GameObject go = tmp.gameObject;

            // Add TextManager automatically
            if (!go.TryGetComponent(out TextManager _))
            {
                go.AddComponent<TextManager>();
            }

            // Add LocalizedText automatically
            if (!go.TryGetComponent(out LocalizedText _))
            {
                go.AddComponent<LocalizedText>();
            }

            Debug.Log($"Auto setup completed for {go.name}");
        }
    }
}