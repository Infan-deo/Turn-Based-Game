using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TurnText;
    [SerializeField] private Button endTurnButton;

    [SerializeField] private GameObject enemyTurnVisualGameobject;

    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UpdateText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateText()
    {
        TurnText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
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
