using System;
using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

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
            totalSpinAmount = 0;
            ActionComplete();
        }


    }
    public override void TakeAction(GridPosition gridPosition, Action<bool> OnSpinComplete)
    {
        ActionStart(OnSpinComplete);
    }
    public override List<GridPosition> GetvalidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPOsition = Unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPOsition
        };

    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }

    public override EnemyAIAction GetEnemyAiAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,

        };
    }
}
