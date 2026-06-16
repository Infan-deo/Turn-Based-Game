using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Threading.Tasks;
using System;
// using Cysharp.Threading.Tasks;



public class ProceduralSpellUIBuilder : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UIDocument uiDocument;

    [Header("Visuals")]
    [SerializeField] private Texture2D headerBanner;
    [SerializeField] private Texture2D spellFrame;
    [SerializeField] private Texture2D cardBackground;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip selectSound;

    [Header("Colors")]
    [SerializeField] private Color cardTint = new(0.15f, 0.15f, 0.2f);
    [SerializeField] private Color hoverTint = new(0.3f, 0.3f, 0.45f);
    [SerializeField] private Color selectedTint = new(0.5f, 0.4f, 0.15f);

    private List<SpellInfo> spellInfos;

    private VisualElement root;
    private ScrollView scrollView;

    private SpellInfo selectedSpell;

    public System.Action<SpellInfo> OnSpellSelected;

    public void GetSpellInfo(List<SpellInfo> spellInfos)
    {
        this.spellInfos = spellInfos;
    }

    private void Awake()
    {
        root = uiDocument.rootVisualElement;
    }
    EventBinding<SpellUIEvent> sceneEventBinding;

    // void OnEnable()
    // {
    //     sceneEventBinding = new EventBinding<SpellUIEvent>(HandlePlayerEvent);
    //     EventBus<SpellUIEvent>.Register(sceneEventBinding);

    //     // Can Add or Remove Actions to/from the EventBinding
    // }

    // private void HandlePlayerEvent(SpellUIEvent @event)
    // {
    //     spellInfos = @event.spellInfos1;
    //     DisplayUI();
    // }

    // void OnDisable()
    // {
    //     EventBus<SpellUIEvent>.Deregister(sceneEventBinding);
    // }

    public async void DisplayUI()
    {
        root.Clear();

        root.AddToClassList("spell-root");

        CreateHeader();

        scrollView = new ScrollView();
        scrollView.AddToClassList("spell-scroll");

        root.Add(scrollView);

        foreach (var spell in spellInfos)
        {
            CreateSpellCard(spell);
        }
    }

    private void CreateSpellCard(SpellInfo spell)
    {
        VisualElement card = new();
        card.AddToClassList("spell-card");

        VisualElement iconFrame = new();
        iconFrame.AddToClassList("spell-icon-frame");

        Image icon = new();
        icon.AddToClassList("spell-icon");

        if (spell.Spellimage != null)
        {
            icon.image = spell.Spellimage.texture;
        }

        iconFrame.Add(icon);

        VisualElement content = new();
        content.AddToClassList("spell-content");

        Label name = new(spell.spellName.GetLocalizedString());
        name.AddToClassList("spell-name");

        Label description = new(spell.spellName.GetLocalizedString());
        description.AddToClassList("spell-description");

        content.Add(name);
        content.Add(description);

        VisualElement cost = new();
        cost.AddToClassList("spell-cost");

        Label costTitle = new("COST");
        costTitle.AddToClassList("cost-title");

        Label costValue =
            new(spell.ConsumablePoints.ToString());

        costValue.AddToClassList("cost-value");

        cost.Add(costTitle);
        cost.Add(costValue);

        card.Add(iconFrame);
        card.Add(content);
        card.Add(cost);


        card.RegisterCallback<MouseEnterEvent>(_ =>
        {
            card.AddToClassList("spell-card-hover");

            card.style.scale =
                new Scale(Vector3.one * 1.03f);

            if (hoverSound)
                audioSource.PlayOneShot(hoverSound);
        });

        card.RegisterCallback<MouseLeaveEvent>(_ =>
        {
            card.RemoveFromClassList("spell-card-hover");

            card.style.scale =
                new Scale(Vector3.one);
        });
    }

    public void DisableUI()
    {
        root.style.display = DisplayStyle.None;
    }

    private void CreateHeader()
    {
        VisualElement header = new();

        header.AddToClassList("header");

        if (headerBanner != null)
        {
            header.style.backgroundImage =
                new StyleBackground(headerBanner);
        }

        Label title = new("SPELLS");
        title.AddToClassList("header-title");

        Label subtitle =
            new("Choose a spell to learn");

        subtitle.AddToClassList("header-subtitle");

        header.Add(title);
        header.Add(subtitle);

        root.Add(header);
    }



}