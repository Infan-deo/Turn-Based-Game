using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public abstract class BaseAction : MonoBehaviour
{

    protected Unit Unit;
    protected bool isActive;
    protected Action<bool> onActionComplete;
    public static Action OnLocalizationChanged;

    public LocalizedString localizedString;
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    private string localizedActionName;




    protected virtual void Awake()
    {
        Unit = GetComponent<Unit>();
    }

    protected virtual IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        localizedString.StringChanged += UpdateLocalizedActionName;

        localizedString.RefreshString();
    }

    private void OnDestroy()
    {
        localizedString.StringChanged -= UpdateLocalizedActionName;

    }

    private void UpdateLocalizedActionName(string value)
    {
        localizedActionName = value;
      
        OnLocalizationChanged?.Invoke();
    }
    public string GetLocalizedActionName()
    {

        return localizedActionName;
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
