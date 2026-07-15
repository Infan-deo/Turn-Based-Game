using Ami.BroAudio;
using UnityEngine;

[CreateAssetMenu(fileName = "ParryInfo", menuName = "Scriptable Objects/ParryInfo")]
public class ParryInfo : ScriptableObject
{
    public float parryOverallTiming;
    public float parryCoolDownTiming;
    public float parryDurationOffset = 0.5f;
    public SoundID GoingToAttackSound;

}
