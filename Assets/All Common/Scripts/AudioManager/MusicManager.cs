using Ami.BroAudio;
using UnityEngine;


public class MusicManager : Singleton<MusicManager>
{
    private BgmData currentBGM;
    public void Play(BgmData bgm)
    {
        if (currentBGM == bgm)
            return;

        StopCurrent();

        currentBGM = bgm;

        // BroAudio play here
        // AudioManager.Play(bgm.sound);
        BroAudio.Play(bgm.sound).AsBGM();

        Debug.Log("Playing: " + bgm.name);
    }

    private void StopCurrent()
    {
        if (currentBGM == null)
            return;

        // Stop current audio here
    }
}
