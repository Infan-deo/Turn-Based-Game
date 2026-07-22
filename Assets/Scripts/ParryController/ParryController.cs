using System;
using UnityEngine;

public class ParryController : MonoBehaviour
{
    public Action OnParryExcuted;
    public Action OnParryCompleted;    
    public IParryable parryable;          // defender
    private IParryable incomingAttack;    // attacker
    public bool isParrying;
    public bool CanParry = true;

    private void OnEnable()
    {
        if (TryGetComponent(out UnitAnimator unitAnimator))
        {
            unitAnimator.animationEventController.OnShildParryEndEvent += () =>
            {
                isParrying = false;
                CanParry = true;
            };
        }
    }

    //called by unitActionSystem
    public void SwitchToParryMode(IParryable defender, IParryable attacker)
    {
        parryable = defender;
        incomingAttack = attacker;

        incomingAttack.OnParryObjectHit += CheckIsParryed;

        ParryManager.Instance.StartParry(
            incomingAttack.GetParryOverallTiming(),
            incomingAttack.GetParryCoolDownTiming()
        );

        ParryManager.Instance.OnPerformParry += ExcuteParry;
        ParryManager.Instance.OnParryTimingEnd += SwitchToNormalMode;
    }

    private void CheckIsParryed()
    {
        if (isParrying)
        {
            OnSuccessfullParry();
        }
        else
        {
            OnUnsuccessfullParry();
        }
    }

    //called by parrymanager when parrybutton is pressed
    public void ExcuteParry()
    {
        if (!CanParry) return;
        OnParryExcuted?.Invoke();
        isParrying = true;
    }

    public void OnSuccessfullParry()
    {
        this.parryable.OnParrySuccess();
    }

    public void OnUnsuccessfullParry()
    {
        
        CanParry = false;
        Invoke(nameof(setCanParry), .5f);        
        this.parryable.OnParryFailed();
    }

    void setCanParry()
    {
        CanParry = true;
    }

    public void SwitchToNormalMode()
    {
        OnParryCompleted?.Invoke();
        incomingAttack.OnParryObjectHit -= CheckIsParryed;
        ParryManager.Instance.OnPerformParry -= ExcuteParry;
        ParryManager.Instance.OnParryTimingEnd -= SwitchToNormalMode;
    }


}
