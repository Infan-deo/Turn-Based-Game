using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    protected Unit Unit;
    protected bool isActive;
    protected Action<bool> onActionComplete;
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

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

    protected void ActionStart(Action<bool> onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }
    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete.Invoke(false);
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return Unit;
    }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new();
        List<GridPosition> validActionGridPositionList = GetvalidGridPositionList();

        foreach (GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAiAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);

        }
        if (enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);

            return enemyAIActionList[0];
        }
        else
        {
            // No possible Enemy AI actions
            return null;
        }

    }

    public abstract EnemyAIAction GetEnemyAiAction(GridPosition gridPosition);

}
