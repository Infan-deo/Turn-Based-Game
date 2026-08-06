using System.Collections;
using Ami.BroAudio;
using BayatGames.SaveGameFree;
using TBGame;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPage : MonoBehaviour
{
    [Header("Buttons")] [SerializeField] private Button musicMuteBtn;
    [SerializeField] private Button sfxMuteBtn;
    [SerializeField] private Button MainMenu;
    [SerializeField] private Button Exit;

    [Header("Sliders")] [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Sprites")] [SerializeField] private Sprite musicMutesprite;
    [SerializeField] private Sprite musicUnmutesprite;
    [SerializeField] private Sprite sfxMutesprite;
    [SerializeField] private Sprite sfxUnmutesprite;

    private Image musicBtnImg;
    private Image sfxBtnImg;

    private bool isMusicMute;
    private bool isSFXMute;

    private float previousMusicVolume = 1f;
    private float previousSFXVolume = 1f;

    IEnumerator Start()
    {
        musicBtnImg = musicMuteBtn.GetComponent<Image>();
        sfxBtnImg = sfxMuteBtn.GetComponent<Image>();
        // Load saved values
        // float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        if (SaveGame.Exists("MusicVolume"))
        {
            print("Music Volume: " + SaveGame.Load<float>("MusicVolume"));
        }

        float musicVolume = SaveGame.Load<float>("MusicVolume");
        float sfxVolume = SaveGame.Load<float>("SFXVolume");
        // float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;


        // Add listeners
        musicSlider.onValueChanged.AddListener(SetBGM);
        sfxSlider.onValueChanged.AddListener(SetSFX);

        musicMuteBtn.onClick.AddListener(ToggleMusicMute);
        sfxMuteBtn.onClick.AddListener(ToggleSFXMute);

        MainMenu.onClick.AddListener(GotoMainMenu);
        MainMenu.onClick.AddListener(GotoMainMenu);
        yield return new WaitForSeconds(0.1f);
        // Apply volumes
        SetBGM(musicVolume);
        SetSFX(sfxVolume);
        SetSFXMute(SaveGame.Load<bool>("isSFXMute"));
        if (SaveGame.Load<bool>("isSFXMute"))
        {
            isSFXMute = true;
        }
        else
        {
           isSFXMute = false;
        }

        SetMusicMute(SaveGame.Load<bool>("isMusicMute"));
        if (SaveGame.Load<bool>("isMusicMute"))
        {
            isMusicMute = true;
        }
        else
        {
            isMusicMute = false;
        }

    }


    public void SetBGM(float volume)
    {
        if (isMusicMute)
            return;
        print(volume);
        previousMusicVolume = volume;

        BroAudio.SetVolume(BroAudioType.Music, volume);

        // PlayerPrefs.SetFloat("MusicVolume", volume);
        SaveGame.Save<float>("MusicVolume", volume);
    }

    public void SetSFX(float volume)
    {
        if (isSFXMute)
            return;

        previousSFXVolume = volume;

        BroAudio.SetVolume(BroAudioType.SFX, volume);

        // PlayerPrefs.SetFloat("SFXVolume", volume);
        SaveGame.Save<float>("SFXVolume", volume);
    }

    private void ToggleMusicMute()
    {
        isMusicMute = !isMusicMute;
        SaveGame.Save("isMusicMute", isMusicMute);
        SetMusicMute(isMusicMute);
    }

    void SetMusicMute(bool ismute)
    {
        if (ismute)
        {
            BroAudio.SetVolume(BroAudioType.Music, 0f);
            musicBtnImg.sprite = musicMutesprite;
        }
        else
        {
            BroAudio.SetVolume(BroAudioType.Music, previousMusicVolume);
            musicBtnImg.sprite = musicUnmutesprite;
        }
    }

    private void ToggleSFXMute()
    {
        isSFXMute = !isSFXMute;
        SaveGame.Save("isSFXMute", isSFXMute);
        SetSFXMute(isSFXMute);
    }

    void SetSFXMute(bool isSFXMute1)
    {
        if (isSFXMute1)
        {
            BroAudio.SetVolume(BroAudioType.SFX, 0f);
            sfxBtnImg.sprite = sfxMutesprite;
        }
        else
        {
            BroAudio.SetVolume(BroAudioType.SFX, previousSFXVolume);
            sfxBtnImg.sprite = sfxUnmutesprite;
        }
    }

    private void GotoMainMenu()
    {
        if (SceneController.Instance.GetcurrentSceneIndex() == 0) return;
        StartCoroutine(GotoMainMenuTransistion());
    }

    IEnumerator GotoMainMenuTransistion()
    {
        CircleFadeTransition.Instance.CircleFadeIn();
        yield return new WaitForSeconds(1.5f);
        CommonGameManager.Instance.CloseSettings();
        SceneController.Instance.LoadMainMenuScene();
    }
}