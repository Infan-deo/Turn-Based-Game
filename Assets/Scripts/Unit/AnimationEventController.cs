using System;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    public Action OnSpellReleasedEvent;
    public Action OnShildParryEndEvent;
    public void OnSpellReleased()
    {
        OnSpellReleasedEvent?.Invoke();
    }
    public void OnShildParryEnd()
    {
        OnShildParryEndEvent?.Invoke();
    }


}
