using Ami.BroAudio;
using UnityEngine;

public class SFXGameManager : Singleton<SFXGameManager>
{
    public SoundID ExplosionSound = default;
    public SoundID ShootingSound = default;
    public SoundID WalkSound = default;
    public SoundID DoorOpenSound = default;
    public SoundID DoorCloseSound = default;
    public SoundID KnifeSlayingSound = default;
    public SoundID ShieldFormingSound = default;
    public SoundID BlueFireSound = default;
    public SoundID firecasting = default;
    public SoundID spellCasting = default;
    public SoundID fireballImpact = default;


    public void PlayExplosionSound()
    {
        BroAudio.Play(ExplosionSound).AsDominator();
    }

    public void PlayShootingSound()
    {
        BroAudio.Play(ShootingSound);
    }

    public void PlayDoorOpenSound()
    {
        BroAudio.Play(DoorOpenSound);
    }

    public void PlayDoorCloseSound()
    {
        BroAudio.Play(DoorCloseSound);
    }

    public void PlayKnifeSlayingSound()
    {
        BroAudio.Play(KnifeSlayingSound);
    }

    public void PlayShieldFormingSound()
    {
        BroAudio.Play(ShieldFormingSound);
    }

    public void PlayBlueFireSound()
    {
        BroAudio.Play(BlueFireSound);
    }

    public void Playfirecasting()
    {
        BroAudio.Play(firecasting);
    }

    public void PlayFireballImpact()
    {
        BroAudio.Play(fireballImpact);
    }

    public void PlaySpellCasting()
    {
        BroAudio.Play(spellCasting);
    }
}