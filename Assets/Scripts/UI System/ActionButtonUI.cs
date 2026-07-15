using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using UnityEngine.Localization.Components;


public class ActionButtonUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Button button;
    [SerializeField] private Outline outline;

    private BaseAction baseAction;
    public LocalizeStringEvent localizeStringEvent;

    public static Action<BaseAction> OnButtonClicked;
    

    private void Awake()
    {
        if (outline == null)
            outline = GetComponent<Outline>();

    }

    void Start()
    {
        BaseAction.OnLocalizationChanged += UpdateText;
    }
   

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        UpdateText();
        button.onClick.AddListener(() =>
        {
            SetSelectedAction();
        });
    }

    public void SetSelectedAction()
    {
        UnitActionSystem.Instance._selecetedActionButtonUI = this;
        UnitActionSystem.Instance.SetSelectedAction(baseAction);
    }

    public void UpdateText()
    {

        if (baseAction != null)
            textMeshProUGUI.text = baseAction.GetLocalizedActionName();
    }

    public void UpdateSelectedVisual()
    {
        BaseAction SelectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        if (SelectedBaseAction == baseAction)
        {
            ChangeOutlineColor(Color.green);
        }
        else
        {
            ChangeOutlineColor(Color.black);
        }
    }


    public void ChangeOutlineColor(Color color)
    {
        outline.effectColor = color;
    }


}
