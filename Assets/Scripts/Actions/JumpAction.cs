using System;
using System.Collections.Generic;
using UnityEngine;

public class JumpAction : BaseAction
{
    public override string GetActionName()
    {
        return "jump";
    }

    public override EnemyAIAction GetEnemyAiAction(GridPosition gridPosition)
    {
         return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,

        };
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

    public override void TakeAction(GridPosition gridPosition, Action<bool> onActionComplete)
    {
        
        ActionStart(OnSpinComplete);
    }

    private void OnSpinComplete(bool obj)
    {
        
    }
}
