using System;
using UnityEngine;

public class ParryManager : Singleton<ParryManager>
{
    public bool canParry;
    public bool isParryStarted;
    public float parryCoolDownTime;
    public float parryCoolDownTimestatic;
    public float parryOverallTime;

    public Action OnPerformParry;
    public Action OnParryTimingEnd;

    private void Update()
    {
        if (!isParryStarted)
        {
            return;
        }

        parryOverallTime -= Time.deltaTime;

        if (parryOverallTime <= 0f)
        {
            isParryStarted = false;
            OnParryTimingEnd?.Invoke();
            return;
        }

        if (parryCoolDownTime > 0f)
        {
            parryCoolDownTime -= Time.deltaTime;
        }

        if (parryCoolDownTime <= 0f &&
            InputManagerTBG.Instance.GetParryInputDownThisFrame())
        {
            PerformParry();
        }
    }

    private void PerformParry()
    {
        parryCoolDownTime = parryCoolDownTimestatic;
        OnPerformParry?.Invoke();
    }



    public void StartParry(float overalltime, float cooldowntime)
    {
        isParryStarted = true;

        parryCoolDownTimestatic = cooldowntime;
        parryCoolDownTime = cooldowntime;

        parryOverallTime = overalltime;
    }
}
