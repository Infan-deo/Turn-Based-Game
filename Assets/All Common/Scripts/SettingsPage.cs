using Ami.BroAudio;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPage : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button musicMuteBtn;
    [SerializeField] private Button sfxMuteBtn;

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Sprites")]
    [SerializeField] private Sprite musicMutesprite;
    [SerializeField] private Sprite musicUnmutesprite;
    [SerializeField] private Sprite sfxMutesprite;
    [SerializeField] private Sprite sfxUnmutesprite;

    private Image musicBtnImg;
    private Image sfxBtnImg;

    private bool isMusicMute;
    private bool isSFXMute;

    private float previousMusicVolume = 1f;
    private float previousSFXVolume = 1f;

    private void Start()
    {
        musicBtnImg = musicMuteBtn.GetComponent<Image>();
        sfxBtnImg = sfxMuteBtn.GetComponent<Image>();
        // Load saved values
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        // Apply volumes
        SetBGM(musicVolume);
        SetSFX(sfxVolume);

        // Add listeners
        musicSlider.onValueChanged.AddListener(SetBGM);
        sfxSlider.onValueChanged.AddListener(SetSFX);

        musicMuteBtn.onClick.AddListener(ToggleMusicMute);
        sfxMuteBtn.onClick.AddListener(ToggleSFXMute);
    }

    public void SetBGM(float volume)
    {
        if (isMusicMute)
            return;

        previousMusicVolume = volume;

        BroAudio.SetVolume(BroAudioType.Music, volume);

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFX(float volume)
    {
        if (isSFXMute)
            return;

        previousSFXVolume = volume;

        BroAudio.SetVolume(BroAudioType.SFX, volume);

        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void ToggleMusicMute()
    {
        isMusicMute = !isMusicMute;

        if (isMusicMute)
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

        if (isSFXMute)
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
}