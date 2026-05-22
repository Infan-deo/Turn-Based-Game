using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Localization/Database")]
public class LocalizationDatabase : ScriptableObject
{
    public List<LocalizationEntry> entries = new();
    public List<string> languages = new();

    private Dictionary<string, LocalizationEntry> _cache;

    public void Initialize()
    {
        _cache = new Dictionary<string, LocalizationEntry>();

        foreach (var entry in entries)
        {
            if (!_cache.ContainsKey(entry.key))
            {
                _cache.Add(entry.key, entry);
            }
        }
    }

    public string GetText(string key, string language)
    {
        if (_cache == null)
            Initialize();

        if (_cache.TryGetValue(key, out var entry))
        {
            return entry.GetValue(language);
        }

        Debug.LogWarning($"Missing Localization Key: {key}");
        return key;
    }
}

[Serializable]
public class LocalizationEntry
{
    public string key;
    public List<LocalizationValue> values = new();

    public string GetValue(string language)
    {
        foreach (var value in values)
        {
            if (value.language == language)
                return value.text;
        }

        return key;
    }
}

[Serializable]
public class LocalizationValue
{
    public string language;
    public string text;
}
