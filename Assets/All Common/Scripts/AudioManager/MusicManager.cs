using System;
using Ami.BroAudio;
using BayatGames.SaveGameFree;
using UnityEngine;


public class MusicManager : Singleton<MusicManager>
{
    public SoundID CommonUIBtnSound;
    private BgmData currentBGM;
    private float previousMusicVolume = 1f;
    private float previousSFXVolume = 1f;
    bool isbgmmute = false;

    private void OnEnable()
    {
        previousMusicVolume = SaveGame.Load<float>("MusicVolume");
        previousSFXVolume = SaveGame.Load<float>("SFXVolume");
        if (SaveGame.Load<bool>("isMusicMute"))
        {
            BroAudio.SetVolume(BroAudioType.Music, 0);
            isbgmmute = false;
        }
        else
        {
            BroAudio.SetVolume(BroAudioType.Music, previousMusicVolume);
            isbgmmute = true;
        }

        if (SaveGame.Load<bool>("isSFXMute"))
        {
            BroAudio.SetVolume(BroAudioType.SFX, 0);
        }
        else
        {
            BroAudio.SetVolume(BroAudioType.SFX, previousSFXVolume);
        }
    }

    public void Play(BgmData bgm)
    {
        if (currentBGM == bgm)
            return;
        
        BroAudio.SetVolume(BroAudioType.Music, 0);

        StopCurrent();

        currentBGM = bgm;

        // BroAudio play here
        // AudioManager.Play(bgm.sound);
        BroAudio.Play(bgm.sound).AsBGM();
    }

    private void StopCurrent()
    {
        if (currentBGM == null)
            return;

        // Stop current audio here
    }
}