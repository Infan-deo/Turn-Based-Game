using BayatGames.SaveGameFree;
using TMPro;
using UnityEngine;

public class LocaleSelector : MonoBehaviour
{
    public TMP_Dropdown tMP_Dropdown;

    private void Start()
    {
        tMP_Dropdown.onValueChanged.AddListener(SelectLanguagueDropDown);
        if (SaveGame.Exists("LocalizeDropDownIndex"))
        {
            tMP_Dropdown.value = SaveGame.Load<int>("LocalizeDropDownIndex");
            print("hiloacal");
        }
        else
        {
            tMP_Dropdown.value = 0;
            print("hiloacal1");
        }
        // if (PlayerPrefs.HasKey("LocalizeDropDownIndex"))
        // {
        //     tMP_Dropdown.value = PlayerPrefs.GetInt("LocalizeDropDownIndex");
        // }
    }


    public void SelectLanguagueDropDown(int index)
    {
        LocalizationManagerCustom.Instance.ChangeLanguage(index);
        // PlayerPrefs.SetInt("LocalizeDropDownIndex", index);
        SaveGame.Save<int>("LocalizeDropDownIndex", index);
    }
}