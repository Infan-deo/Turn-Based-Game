using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    private Text _textLegacy;
    private TextMeshProUGUI _textMeshProUGUI;
    bool isTmpText;

    public bool useCustomFont;

    [ShowIf(nameof(useCustomFont))]
    public TMP_FontAsset fontAsset;


    private void Awake()
    {
        if (TryGetComponent(out Text text))
        {
            _textLegacy = text;
        }
        else if (TryGetComponent(out TextMeshProUGUI textTMP))
        {
            _textMeshProUGUI = textTMP;
            isTmpText = true;
        }
        else
        {
            Debug.Log("Text Component missing");
        }
    }
    private void Start()
    {
        if (useCustomFont)
        {
            _textMeshProUGUI.font = fontAsset;
            _textMeshProUGUI.UpdateFontAsset();
        }
    }
    public void SetFont(TMP_FontAsset a)
    {
        if (isTmpText)
        {
            if (useCustomFont)
            {
                _textMeshProUGUI.font = fontAsset;
                _textMeshProUGUI.UpdateFontAsset();
            }
            else
            {
                _textMeshProUGUI.font = a;
                _textMeshProUGUI.UpdateFontAsset();
            }
        }
    }

}
