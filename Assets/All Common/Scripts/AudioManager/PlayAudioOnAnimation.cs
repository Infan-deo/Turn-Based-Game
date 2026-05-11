using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;

public class PlayAudioOnAnimation : MonoBehaviour
{
    public List<SoundID> soundIDs;
    private IAudioPlayer currentBGM;


    public void PlayAudioAlong(int i)
    {
        if (soundIDs.Count > 0)
        {
            currentBGM = BroAudio.Play(soundIDs[i]);
        }
    }

    public void PlayAudioSeperate(int i)
    {
        if (soundIDs.Count > 0)
        {
            currentBGM?.Stop();
            currentBGM = BroAudio.Play(soundIDs[i]);
        }
    }
}
