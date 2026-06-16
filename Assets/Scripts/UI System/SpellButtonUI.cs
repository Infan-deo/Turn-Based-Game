using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class SpellButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _spellName;
    [SerializeField] private TextMeshProUGUI _spellDescription;
    [SerializeField] private TextMeshProUGUI _spellPointsNeeded;
    [SerializeField] private Image _spellImage;
    [SerializeField] private Button _spellBtn;

    private SpellInfo spellInfo;

    private void Start()
    {
        _spellBtn.onClick.AddListener(SpellBtnPressed);
    }

    public void SetSpellButtonInfo(SpellInfo spellInfo)
    {
        this.spellInfo = spellInfo;

        UpdateInfo();
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale locale)
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if (spellInfo == null)
            return;

        _spellName.text =
            spellInfo.spellName.GetLocalizedString();

        _spellDescription.text =
            spellInfo.spellDescription.GetLocalizedString();

        _spellPointsNeeded.text =
            spellInfo.ConsumablePoints.ToString();

        _spellImage.sprite =
            spellInfo.Spellimage;
    }

    public void SpellBtnPressed()
    {
        EventBus<SelectedSpellEvent>.Raise(new SelectedSpellEvent(spellInfo));
    }
}