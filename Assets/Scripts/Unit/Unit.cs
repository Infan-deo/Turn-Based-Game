using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 6;
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    public event EventHandler OnUnitAttacked;
    [SerializeField] private bool isEnemy;   
    [SerializeField] private Transform[] unitCameraTransformPoint;
    public List<Transform> SpellPoints;
    GridPosition gridPosition;
    HealthSystem healthSystem;
    BaseAction[] baseActionArray;
    private bool _isUnitDead;

    int actionpoints = 2;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();

    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        healthSystem.OnDead += HealthSystem_OnDead;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
        actionpoints = ACTION_POINTS_MAX;
        
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        UnitAttackManager.Instance.OnUnitDied(this);
        _isUnitDead =true;
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }



    private void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPOsition(this, oldGridPosition, newGridPosition);
        }


    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in GetBaseActionArray())
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }



    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return actionpoints >= baseAction.GetActionPointsCost();
    }

    private void SpendActionPoints(int amount)
    {
        actionpoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    public int GetActionPoints()
    {
        return actionpoints;
    }
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (isEnemy && !TurnSystem.Instance.IsPlayerTurn() || !isEnemy && TurnSystem.Instance.IsPlayerTurn())
        {
            actionpoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
        UnitManager.Instance.SetUnitRagdollFallDir(UnitActionSystem.Instance.GetSelectedUnit().GetWorldPosition());
    }
    public void Damage(int damageAmount, BaseAction baseAction)
    {
        UnitAttackManager.Instance.UnitAttacked(this, baseAction);
        healthSystem.Damage(damageAmount);
        OnUnitAttacked?.Invoke(this, EventArgs.Empty);
        UnitManager.Instance.SetUnitRagdollFallDir(UnitActionSystem.Instance.GetSelectedUnit().GetWorldPosition());
    }


   
    public Transform[] GetUnitCameraTransform()
    {
        return unitCameraTransformPoint;
    }
    public bool GetIsUnitDead()
    {
        return _isUnitDead;
    }
    public void SetIsUnitDead(bool _isUnitDead)
    {
        this._isUnitDead = _isUnitDead;
    }
}
