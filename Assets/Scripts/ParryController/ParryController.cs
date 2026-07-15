using System;
using UnityEngine;

public class ParryController : MonoBehaviour
{
    public Action OnParryExcuted;
    public Action OnParryCompleted;
    public Transform camerapoint;
    public IParryable parryable;          // defender
    private IParryable incomingAttack;    // attacker
    public bool isSwitchedToParryMode;
    public bool isUnsuccessfullParry;

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
        print("hi");

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
        isUnsuccessfullParry = true;
        CanParry = false;
        Invoke(nameof(setCanParry), .5f);
        if (this.parryable == null)
        {
            print("fuck it");
            return;
        }
        this.parryable.OnParryFailed();
    }

    void setCanParry()
    {
        CanParry = true;
    }


   public void SwitchToNormalMode()
{
    isSwitchedToParryMode = false;
    OnParryCompleted?.Invoke();

    incomingAttack.OnParryObjectHit -= CheckIsParryed;
    ParryManager.Instance.OnPerformParry -= ExcuteParry;
    ParryManager.Instance.OnParryTimingEnd -= SwitchToNormalMode;
}


}
