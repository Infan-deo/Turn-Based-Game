using System;
using System.Collections.Generic;
using UnityEngine;


public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private int maxMoveDistance = 4;


    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }


    public override void TakeAction(GridPosition targetPosition, Action<bool> OnMoveComplete)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(OnMoveComplete);
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }


    public override List<GridPosition> GetvalidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = Unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // Not valid
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    // Same grid position where the unit is already at
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position already occupied with another unit
                    continue;
                }


                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }
    public override EnemyAIAction GetEnemyAiAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = Unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,

        };
    }
}
