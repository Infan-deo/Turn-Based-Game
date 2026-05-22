using System;
using UnityEngine;

public class LocalizationManagerCustom : Singleton<LocalizationManagerCustom>
{
    public LocalizationDatabase database;

    public static Action OnLanguageChanged;

    public string CurrentLanguage => currentLanguage;

    [SerializeField]
    private string currentLanguage = "English";

   

    public void SetLanguage(string language)
    {
        currentLanguage = language;

        PlayerPrefs.SetString("LANGUAGE", language);

        OnLanguageChanged?.Invoke();
    }

    public string GetText(string key)
    {
        return database.GetText(key, currentLanguage);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("LANGUAGE"))
        {
            currentLanguage = PlayerPrefs.GetString("LANGUAGE");
        }
    }
}
