using UnityEditor;
using UnityEngine;

public class LocalizationEditorWindow : EditorWindow
{
    private const string GOOGLE_SHEET_PREF_KEY = "LOCALIZATION_GOOGLE_SHEET_URL";

    private string googleSheetURL;

    private LocalizationDatabase database;

    private int selectedLanguage;

    [MenuItem("Tools/Localization Manager")]
    public static void ShowWindow()
    {
        GetWindow<LocalizationEditorWindow>("Localization Manager");
    }

    private void OnEnable()
    {
        database = Resources.Load<LocalizationDatabase>("LocalizationDatabase");

        // Load saved URL
        googleSheetURL = EditorPrefs.GetString(GOOGLE_SHEET_PREF_KEY, "");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.Label("Google Sheet", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

        googleSheetURL = EditorGUILayout.TextField("Sheet URL", googleSheetURL);

        // Save automatically when changed
        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetString(GOOGLE_SHEET_PREF_KEY, googleSheetURL);
        }

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Import From Google Sheets", GUILayout.Height(30)))
        {
            LocalizationGoogleSheetImporter.Import(googleSheetURL);

            database = Resources.Load<LocalizationDatabase>("LocalizationDatabase");
        }

        GUI.enabled = !string.IsNullOrWhiteSpace(googleSheetURL);

        if (GUILayout.Button("Open Google Sheet", GUILayout.Height(30)))
        {
            Application.OpenURL(googleSheetURL);
        }

        GUI.enabled = true;

        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        if (database != null)
        {
            GUILayout.Label("Languages", EditorStyles.boldLabel);

            selectedLanguage = EditorGUILayout.Popup(
                "Preview Language",
                selectedLanguage,
                database.languages.ToArray());

            GUILayout.Space(10);

            if (GUILayout.Button("Assign LocalizedText To All Text Components"))
            {
                LocalizationAutoAssigner.Assign();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Preview Selected Language"))
            {
                PreviewLanguage();
            }
        }
    }

    private void PreviewLanguage()
    {
        LocalizedText[] localizedTexts =
            FindObjectsByType<LocalizedText>(FindObjectsSortMode.None);

        foreach (var localizedText in localizedTexts)
        {
            TMPro.TMP_Text tmp =
                localizedText.GetComponent<TMPro.TMP_Text>();

            UnityEngine.UI.Text legacy =
                localizedText.GetComponent<UnityEngine.UI.Text>();

            string localizedValue = database.GetText(
                localizedText.localizationKey,
                database.languages[selectedLanguage]);

            if (tmp != null)
                tmp.text = localizedValue;

            if (legacy != null)
                legacy.text = localizedValue;
        }
    }
}