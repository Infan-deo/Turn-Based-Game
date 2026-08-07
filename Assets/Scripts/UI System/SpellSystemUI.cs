using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellSystemUI : MonoBehaviour
{
    [SerializeField] private Transform SpellButtonPrefab;
    [SerializeField] private Transform SpellButtonContainerTransform;
    private List<SpellInfo> spellInfos;
    EventBinding<SpellUIEvent> sceneEventBinding;
    private List<SpellButtonUI> SpellButtonUIs = new();
    public Transform SpellUIPanel;
    public Transform NoSpellAvail;
    public Button CloseButton;

    [SerializeField] private TextMeshProUGUI remainingPoints;
    public Transform remainingPointsParent;

    EventBinding<SelectedSpellEvent> SpellSystemUI_selectedSpellEvent;


    [Header("ShowSpellAndReplace")] public Transform ShowSpellAndReplace;
    public Image SelectedSpellImg;
    public Button Replacebtn;

    private IEnumerator Start()
    {
        CloseButton.onClick.AddListener(CloseUI);
        Replacebtn.onClick.AddListener(() =>
        {
            CreateSpellActionButtons();
            DisplayUI();
        });
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        yield return new WaitForSeconds(0.5f);
        UpdateRemainingPoints();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateRemainingPoints();
    }

    void OnEnable()
    {
        sceneEventBinding = new EventBinding<SpellUIEvent>(DisplaySpells);
        EventBus<SpellUIEvent>.Register(sceneEventBinding);
        SpellSystemUI_selectedSpellEvent = new EventBinding<SelectedSpellEvent>(OnSpellSelected);
        EventBus<SelectedSpellEvent>.Register(SpellSystemUI_selectedSpellEvent);
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        if (UnitActionSystem.Instance.GetSelectedAction() is not SpellAction)
        {
            HideShowSpellAndReplace();
        }
        else
        {
            UpdateRemainingPoints();
        }
    }

    void OnDisable()
    {
        EventBus<SpellUIEvent>.Deregister(sceneEventBinding);
        EventBus<SelectedSpellEvent>.Deregister(SpellSystemUI_selectedSpellEvent);
    }

    private void DisplaySpells(SpellUIEvent @event)
    {
        spellInfos = @event.spellInfos1;
        NoSpellAvail.gameObject.SetActive(spellInfos.Count == 0);
        if (spellInfos.Count > 0)
        {
            CreateSpellActionButtons();
            DisplayUI();
        }
    }

    public void OnSpellSelected(SelectedSpellEvent @event)
    {
        if (@event.SelectedSpellInfo.ConsumablePoints <= int.Parse(remainingPoints.text))
        {
            CloseUI();
            DisplayShowSpellAndReplace(@event);
        }
        else
        {
            // print("");
            remainingPointsParent.DOShakePosition(
                duration: 1f,
                strength: new Vector3(4f, 0f, 0f),
                randomness: 90,
                vibrato: 10,
                fadeOut: true,
                randomnessMode: ShakeRandomnessMode.Harmonic
            );
        }
    }


    public void DisplayUI()
    {
        HideShowSpellAndReplace();
        SpellUIPanel.gameObject.SetActive(true);
    }

    public void CloseUI()
    {
        SpellUIPanel.gameObject.SetActive(false);
    }


    public void CreateSpellActionButtons()
    {
        foreach (Transform item in SpellButtonContainerTransform)
        {
            Destroy(item.gameObject);
        }

        SpellButtonUIs.Clear();
        foreach (var spellInfo in spellInfos)
        {
            Transform SpellButtonTransform = Instantiate(SpellButtonPrefab, SpellButtonContainerTransform);
            SpellButtonUI spellButtonUI = SpellButtonTransform.GetComponent<SpellButtonUI>();
            SpellButtonUIs.Add(spellButtonUI);
            spellButtonUI.SetSpellButtonInfo(spellInfo);
        }
    }

    public void DisplayShowSpellAndReplace(SelectedSpellEvent shieldEvent)
    {
        SelectedSpellImg.sprite = shieldEvent.SelectedSpellInfo.Spellimage;
        ShowSpellAndReplace?.gameObject.SetActive(true);
    }

    public void HideShowSpellAndReplace()
    {
        ShowSpellAndReplace?.gameObject.SetActive(false);
    }

    public void UpdateRemainingPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        remainingPoints.text = selectedUnit.GetActionPoints().ToString();
    }
}