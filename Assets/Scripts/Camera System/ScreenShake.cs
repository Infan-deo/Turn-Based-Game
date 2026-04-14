using Unity.Cinemachine;
using UnityEngine;

public class ScreenShake : Singleton<ScreenShake>
{
    private CinemachineImpulseSource cinemachineImpu1seSource;
    protected override void Awake()
    {
        base.Awake();
        cinemachineImpu1seSource = GetComponent<CinemachineImpulseSource>();
    }


    public void ShakeCamera(float intensity = 1f)
    {
        cinemachineImpu1seSource.GenerateImpulse(intensity);
    }
}