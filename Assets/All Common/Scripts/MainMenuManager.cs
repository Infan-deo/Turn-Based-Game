using System.Collections;
using Ami.BroAudio;
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
        Settingsbtn.onClick.AddListener(SettingsBtnPressed);
        Exitbtn.onClick.AddListener(ExitBtnPressed);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        SetBGM(musicVolume);
        SetSFX(sfxVolume);
    }
    public void SetBGM(float volume)
    {

        BroAudio.SetVolume(BroAudioType.Music, volume);

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFX(float volume)
    {
        BroAudio.SetVolume(BroAudioType.SFX, volume);

        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    void PlayBtnPressed()
    {
        StartCoroutine(MakeTransistionAndLoad());
    }
    IEnumerator MakeTransistionAndLoad()
    {
        CircleFadeTransition.Instance.CircleFadeIn();
        yield return new WaitForSeconds(1f);
        _sceneManager.LoadGameScene();
    }

    void SettingsBtnPressed()
    {
        CommonGameManager.Instance.OpenSettings();
    }

  

    void ExitBtnPressed()
    {
        Application.Quit();
    }

}
