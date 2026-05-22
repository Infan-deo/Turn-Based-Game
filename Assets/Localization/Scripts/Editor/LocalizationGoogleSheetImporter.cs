using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using TamilEncoder;

public class LocalizationGoogleSheetImporter
{
    private const string DATABASE_PATH = "Assets/Localization/Resources/LocalizationDatabase.asset";




    public static TamilFontEncoding tamilEncoding = TamilFontEncoding.TSCII;


    public static void Import(string url)
    {
        string csvURL = GoogleSheetURLConverter.ConvertToCSVUrl(url);

        UnityWebRequest request = UnityWebRequest.Get(csvURL);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return;
        }

        // UTF8 decode
        byte[] data = request.downloadHandler.data;

        string csv = System.Text.Encoding.UTF8.GetString(data);

        // -----------------------------------------
        // Convert Unicode Tamil -> TSCII
        // -----------------------------------------

        csv = TamilEncoding.ConvertFromUnicode(csv, tamilEncoding);

        // -----------------------------------------
        // SAVE CSV
        // -----------------------------------------

        string resourcesFolder = "Assets/Localization/Resources";

        if (!AssetDatabase.IsValidFolder(resourcesFolder))
        {
            AssetDatabase.CreateFolder("Assets/Localization", "Resources");
        }

        string csvPath = $"{resourcesFolder}/Localization.csv";

        File.WriteAllText(
            csvPath,
            csv,
            new UTF8Encoding(true)
        );

        AssetDatabase.Refresh();

        // -----------------------------------------

        ParseCSV(csv);
    }



    public static string[] ParseCSVLine(string line)
    {
        var matches = Regex.Matches(
            line,
            "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

        string[] values = new string[matches.Count];

        for (int i = 0; i < matches.Count; i++)
        {
            values[i] = matches[i]
                .Value
                .TrimStart(',')
                .Trim('\"');
        }

        return values;
    }
    private static void ParseCSV(string csv)
    {
        LocalizationDatabase database = AssetDatabase.LoadAssetAtPath<LocalizationDatabase>(DATABASE_PATH);

        if (database == null)
        {
            database = ScriptableObject.CreateInstance<LocalizationDatabase>();

            AssetDatabase.CreateAsset(database, DATABASE_PATH);
        }

        database.entries.Clear();
        database.languages.Clear();

        string[] lines = csv.Split('\n');

        if (lines.Length <= 1)
            return;

        string[] headers = ParseCSVLine(lines[0]);

        for (int i = 1; i < headers.Length; i++)
        {
            database.languages.Add(headers[i].Trim());
        }

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] columns = ParseCSVLine(lines[i]);

            LocalizationEntry entry = new LocalizationEntry();
            entry.key = columns[0].Trim();

            for (int j = 1; j < columns.Length; j++)
            {
                LocalizationValue value = new LocalizationValue();

                value.language = headers[j].Trim();
                value.text = columns[j].Trim();

                entry.values.Add(value);
            }

            database.entries.Add(entry);
        }

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();

        Debug.Log("Localization Imported Successfully");
    }
}
