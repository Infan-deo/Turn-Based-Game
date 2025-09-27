using System;
using UnityEngine;
using UnityEngine.Video;

public class SpinAction : BaseAction
{
   
    private float spinAddAmount;
    private float totalSpinAmount = 0;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360)
        {
            isActive = false;
            totalSpinAmount = 0;
            if (onActionComplete != null)
            {
                onActionComplete.Invoke(false); // or true, depending on your logic
            }
        }


    }
    public void Spin(Action<bool> OnSpinComplete)
    {
        isActive = true;
        onActionComplete = OnSpinComplete;
    }

    public override string GetActionName()
    {
        return "Spin";
    }
}
