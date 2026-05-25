using UnityEngine;
using UnityEngine.UI;


public class GameSceneManager : MonoBehaviour
{
    public Button settingsBtn;
    private void Start() {
        settingsBtn.onClick.AddListener(OpenSettingsPage);
    }
    public void OpenSettingsPage()
    {
        CommonGameManager.Instance.OpenSettings();
    }
}
