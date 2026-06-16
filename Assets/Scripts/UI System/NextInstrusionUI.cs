using System;
using TMPro;
using UnityEngine;
using UnityUtils;

public class NextInstrusionUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI chooseActionText;
    [SerializeField] private TextMeshProUGUI ChooseEnemyText;
    [SerializeField] private TextMeshProUGUI CustomText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        HideAll();
        ShowChooseActionText();

    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        HideAll();
        ShowChooseEnemyText();
    }

    public void HideAll()
    {
        CustomText.gameObject.SetActive(false);
        chooseActionText.gameObject.SetActive(false);
        ChooseEnemyText.gameObject.SetActive(false);
    }

    public void ShowChooseActionText()
    {
        chooseActionText.gameObject.SetActive(true);
    }
    public void ShowChooseEnemyText()
    {
        ChooseEnemyText.gameObject.SetActive(true);
    }
    public void ShowChooseCustomText()
    {
        CustomText.gameObject.SetActive(true);
    }
}
