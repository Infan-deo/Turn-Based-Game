using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;

    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;

    [SerializeField] private HealthSystem healthSystem;



    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        UpdateActionPointsText();
        UpdateHealthBarImage();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBarImage();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBarImage()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
}
