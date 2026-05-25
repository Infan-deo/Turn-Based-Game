using UnityEngine;
using UnityEngine.UI;

public class CommonGameManager : Singleton<CommonGameManager>
{
    public GameObject popUpCanvas;
    public GameObject SettingsPanel;
    public GameObject Blockpanel;
    public Button popupClosebtn;


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(popUpCanvas);
    }
    private void Start()
    {
        popupClosebtn.onClick.AddListener(() =>
        {
            CloseSettings();
        });
    }

    public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
        SetBlockPanelState(true);
    }
    public void CloseSettings()
    {
        SettingsPanel.SetActive(false);
        SetBlockPanelState(false);
    }

    public void SetBlockPanelState(bool state)
    {
        if (Blockpanel != null)
            Blockpanel.SetActive(state);
    }




}
