using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TurnText;
    [SerializeField] private Button endTurnButton;

    [SerializeField] private GameObject enemyTurnVisualGameobject;
    [SerializeField] private LocalizedString TurnLocalizedString;



    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        // UpdateText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
        TurnLocalizedString.StringChanged += UpdateTurnText;
        UpdateTurnText(TurnLocalizedString.GetLocalizedString());
    }


    private void UpdateTurnText(string value)
    {
        TurnText.text =
            $"{value} {TurnSystem.Instance.GetTurnNumber()}";
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // UpdateText();
        UpdateTurnText(TurnLocalizedString.GetLocalizedString());
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }



    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualGameobject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }
    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
