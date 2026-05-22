using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LocalizedText : MonoBehaviour
{
    public string localizationKey;

    private TMP_Text _tmpText;
    private Text _legacyText;

    private void Awake()
    {
        CacheComponents();
        Refresh();
    }

    private void OnEnable()
    {
        LocalizationManagerCustom.OnLanguageChanged += Refresh;
    }

    private void OnDisable()
    {
        LocalizationManagerCustom.OnLanguageChanged -= Refresh;
    }

    private void CacheComponents()
    {
        _tmpText = GetComponent<TMP_Text>();
        _legacyText = GetComponent<Text>();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        CacheComponents();

        if (!Application.isPlaying)
        {
            SetText(localizationKey);
        }
    }
#endif

    public void Refresh()
    {
        if (LocalizationManagerCustom.Instance == null)
            return;

        string localizedValue = LocalizationManagerCustom.Instance.GetText(localizationKey);

        SetText(localizedValue);
    }

    private void SetText(string value)
    {
        if (_tmpText != null)
        {
            _tmpText.text = value;
        }

        if (_legacyText != null)
        {
            _legacyText.text = value;
        }
    }
}
