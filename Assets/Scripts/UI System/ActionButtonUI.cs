using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Button button;

    public void SetBaseAction(BaseAction baseAction)
    {
        textMeshProUGUI.text = baseAction.GetActionName().ToUpper();
    }



}
