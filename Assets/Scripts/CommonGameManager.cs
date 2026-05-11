using UnityEngine;

public class CommonGameManager : Singleton<CommonGameManager>
{
    public GameObject popUpCanvas;
    public GameObject SettingsPanel;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(popUpCanvas);
    }

    public void ToggleSettingsPage()
    {
        SettingsPanel.SetActive(!SettingsPanel.activeSelf);
    }


}
