using TBGame;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    SceneController _sceneManager;

    [Inject]
    public void Inject(SceneController _sceneManager)
    {
        this._sceneManager = _sceneManager;
    }

    public Button PlayBtn;
    public Button Settingsbtn;
    public Button Exitbtn;

    private void Start()
    {
        PlayBtn.onClick.AddListener(PlayBtnPressed);
        // PlayBtn.onClick.AddListener(PlayBtnPressed);
        Exitbtn.onClick.AddListener(ExitBtnPressed);
    }

    void PlayBtnPressed()
    {
        _sceneManager.LoadGameScene();
    }

    void SettingsBtnPressed()
    {
        
    }

    void ExitBtnPressed()
    {
        Application.Quit();
    }

}
