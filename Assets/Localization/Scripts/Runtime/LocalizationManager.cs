using System;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationManagerCustom : Singleton<LocalizationManagerCustom>
{


    public static Action OnLanguageChanged;

    public string CurrentLanguage;





    // currentLanguage = language;
    public void ChangeLanguage(int index)
    {
        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[index];

        PlayerPrefs.SetInt("LANGUAGE_INDEX", index);

        // OnLanguageChanged?.Invoke();
    }



    private void Start()
    {
        if (PlayerPrefs.HasKey("LANGUAGE_INDEX"))
        {
            // currentLanguage = PlayerPrefs.GetString("LANGUAGE");
            int index = PlayerPrefs.GetInt("LANGUAGE_INDEX");
             LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[index];


        }
    }
}
