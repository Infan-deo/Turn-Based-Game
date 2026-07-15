using System;
using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;

public class SwordAction : BaseAction
{

    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;
    public static Action OnAnySwordHit;
    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit
    }
    public int maxSwordDistance = 1;
    private State state;

    private float stateTimer;
    private Unit targetUnit;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                Aim(); ;
                break;
            case State.SwingingSwordAfterHit:
                break;

        }
        if (stateTimer <= 0f)
        {
            NextState();
        }

    }
    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                targetUnit.Damage(100,this);
                SFXGameManager.Instance.PlayKnifeSlayingSound();
                OnAnySwordHit?.Invoke();
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;

        }

    }
    private void Aim()
    {
        Vector3 aimDir = (targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
    }
    public override void TakeAction(GridPosition gridPosition, Action<bool> OnActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;
        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(OnActionComplete);
    }
    public override List<GridPosition> GetvalidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = Unit.GetGridPosition();

        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // Not valid
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position is empty
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == Unit.IsEnemy())
                {
                    continue;
                }



                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;

    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public int GetMaxSwordDistance()
    {
        return maxSwordDistance;
    }

    public override EnemyAIAction GetEnemyAiAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200,

        };
    }

}
