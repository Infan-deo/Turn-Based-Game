using System;
using System.Collections.Generic;

using UnityEngine;

public class InteractAction : BaseAction
{
    public int maxInteractDistance = 1;


    public override string GetActionName()
    {
        return "Interact";
    }

    public override EnemyAIAction GetEnemyAiAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override List<GridPosition> GetvalidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = Unit.GetGridPosition();

        for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
        {
            for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
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


                IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);
                if (interactable == null)
                {
                    // Not valid
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action<bool> onActionComplete)
    {
        IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
        if (interactable != null)
        {
            interactable.Interact(OnInteractComplted);
        }
        ActionStart(onActionComplete);
    }

    public void OnInteractComplted()
    {
        ActionComplete();
    }
}
