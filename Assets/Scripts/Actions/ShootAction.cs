using System;
using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;

public class ShootAction : BaseAction, IParryable
{
    private enum State
    {
        Aiming,
        Parrytiming,
        Shooting,
        CoolOff
    }

    private State state;

    private float stateTimer;
    private bool canShootBullet;
    public event EventHandler<OnShootEventArgs> OnShoot;
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    public event Action OnParryObjectHit;
    public event Action OnSuccessFullParry;
    public static event EventHandler OnAnyParryTimingStarted;

    [SerializeField] private LayerMask obstaclesLayerMask;
    public ParryInfo ShootActionParryInfo;


    public class OnShootEventArgs : EventArgs
    {
        public Unit targetedUnit;
        public Unit shootingUnit;
    }


    [SerializeField] private int maxShootDistance = 7;

    Unit targetUnit;


    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                Aim();
                break;
            case State.Parrytiming:
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.CoolOff:
                break;
        }
        if (stateTimer <= 0f)
        {
            NextState();
        }



    }

    private void Shoot()
    {
        OnParryObjectHit?.Invoke();
        OnAnyShoot?.Invoke(this, new OnShootEventArgs
        {
            targetedUnit = targetUnit,
            shootingUnit = Unit
        });
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetedUnit = targetUnit,
            shootingUnit = Unit
        });
        if (!isThisActionParryableNow())
        {
            targetUnit.Damage(40, this);
        }
        SFXGameManager.Instance.PlayShootingSound();
    }
    private void Aim()
    {
        Vector3 aimDir = (targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
        if (isThisActionParryableNow())
        {
            targetUnit.transform.forward = Vector3.Lerp(targetUnit.transform.forward, -aimDir, Time.deltaTime * rotateSpeed);
        }
        // targetUnit.transform.forward =Quaternion.RotateTowards()
    }

    private void NextState()
    {
        float shootingStateTime = 0.15f;
        switch (state)
        {
            case State.Aiming:

                if (isThisActionParryableNow())
                {
                    OnAnyParryTimingStarted?.Invoke(this,EventArgs.Empty);
                    state = State.Parrytiming;
                    float parryStateTime = GetParryOverallTiming() / 2;
                    stateTimer = parryStateTime;
                }
                else
                {
                    state = State.Shooting;
                    stateTimer = shootingStateTime;
                }
                break;
            case State.Parrytiming:

                state = State.Shooting;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.CoolOff;
                float coolOffStateTime = .75f;
                stateTimer = coolOffStateTime;
                break;
            case State.CoolOff:
                ActionComplete();
                break;
        }

    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetvalidGridPositionList()
    {
        GridPosition unitgridPosition = Unit.GetGridPosition();
        return GetvalidGridPositionList(unitgridPosition);
    }
    public List<GridPosition> GetvalidGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();


        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // Not valid
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Math.Abs(z);
                if (testDistance > maxShootDistance)
                {
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
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(Unit.GetWorldPosition() + Vector3.up * unitShoulderHeight,
                shootDir,
                Vector3.Distance(Unit.GetWorldPosition(), targetUnit.GetWorldPosition()),
                obstaclesLayerMask
                ))
                {
                    continue;
                }


                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action<bool> onActionComplete)
    {

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);


        state = State.Aiming;
        float aimimgStateTime = 1.5f;
        stateTimer = aimimgStateTime;
        canShootBullet = true;

        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }
    public override EnemyAIAction GetEnemyAiAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        int minimumHealthTargetPoints = Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + minimumHealthTargetPoints,

        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetvalidGridPositionList(gridPosition).Count;
    }

    public float GetParryOverallTiming()
    {
        return ShootActionParryInfo.parryOverallTiming;
    }

    public float GetParryCoolDownTiming()
    {
        return ShootActionParryInfo.parryCoolDownTiming;
    }

    public bool isThisActionParryableNow()
    {
        return !TurnSystem.Instance.IsPlayerTurn();
    }

    public ParryController GetParryController()
    {
        if (TryGetComponent(out ParryController parryController))
        {
            return parryController;
        }
        return null;

    }

    public void OnParryFailed()
    {
        Unit.Damage(40, this);
    }

    public ParryInfo GetParryInfo()
    {
        return ShootActionParryInfo;
    }

    public void OnParrySuccess()
    {
        //Success
        OnSuccessFullParry?.Invoke();
    }


}
