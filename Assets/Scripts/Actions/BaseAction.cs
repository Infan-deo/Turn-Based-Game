using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    protected Unit Unit;
    protected bool isActive;
    protected Action<bool> onActionComplete;

    protected virtual void Awake()
    {
        Unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action<bool> onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetvalidGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }
    public abstract List<GridPosition> GetvalidGridPositionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }
   
}
