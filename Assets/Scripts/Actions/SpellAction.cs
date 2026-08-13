using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class SpellAction : BaseAction
{
    [SerializeField] private int maxShootDistance = 12;

    public List<SpellInfo> spellInfos;

    private SpellInfo selectedSpellInfo;
    EventBinding<SelectedSpellEvent> selectedSpellEvent;
    public bool IsSpellSelected;
    private Transform spellParent;

    public Action<SpellType> OnSpellActionStarted;
    public Action OnSpellActionCompleted;

    public SpellType selectedspellType => selectedSpellInfo.spellBehaviour.spellType;
    Unit targetUnit;

    [SerializeField] private LayerMask obstaclesLayerMask;

    private void OnEnable()
    {
        selectedSpellEvent = new EventBinding<SelectedSpellEvent>(OnSpellSelected);
        EventBus<SelectedSpellEvent>.Register(selectedSpellEvent);
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        spellParent = UnitActionSystem.Instance.SpellParent;
        CreateProjectilePool();
    }


    private void OnDisable()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
        EventBus<SelectedSpellEvent>.Deregister(selectedSpellEvent);
    }

    public void OnSpellSelected(SelectedSpellEvent @event)
    {
        selectedSpellInfo = @event.SelectedSpellInfo;
        // UnitActionSystem.Instance.RefreshSelectedAction();       
        IsSpellSelected = true;
    }


    public override string GetActionName()
    {
        return "Spell";
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }

    public void OnSpellActionTimeCompleted()
    {
        ActionComplete();
    }

    public override EnemyAIAction GetEnemyAiAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public void Aim()
    {
        Vector3 aimDir = (targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;
        transform.DOLookAt(transform.position + aimDir, 0.2f);
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


                if (selectedSpellInfo != null)
                {
                    if (selectedspellType == SpellType.PROJECTILE)
                    {
                        Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                        Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                        float unitShoulderHeight = 1.7f;

                        if (Physics.Raycast(
                                Unit.GetWorldPosition() + Vector3.up * unitShoulderHeight,
                                shootDir,
                                Vector3.Distance(Unit.GetWorldPosition(), targetUnit.GetWorldPosition()),
                                obstaclesLayerMask))
                        {
                            continue;
                        }
                    }

                    if (selectedspellType == SpellType.SHIELD)
                    {
                        if (targetUnit == Unit && !targetUnit.hasShield)
                        {
                            validGridPositionList.Add(testGridPosition);
                        }

                        continue; // Don't execute the code below for shield
                    }

                    // All other spell types
                    if (targetUnit.IsEnemy() == Unit.IsEnemy())
                    {
                        continue;
                    }
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        if (UnitActionSystem.Instance.GetSelectedAction() is not SpellAction) return;
        DisplaySpells();
    }

    public void DisplaySpells()
    {
        EventBus<SpellUIEvent>.Raise(new SpellUIEvent
        {
            spellInfos1 = spellInfos
        });
    }

    public override void TakeAction(GridPosition gridPosition, Action<bool> onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);


        if (selectedSpellInfo.spellBehaviour != null)
        {
            StartCoroutine(
                selectedSpellInfo.spellBehaviour.Execute(
                    Unit,
                    targetUnit,
                    selectedSpellInfo, this, () => ActionComplete()));
        }

        ActionStart(onActionComplete);
    }


    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override int GetActionPointsCost()
    {
        return selectedSpellInfo?.ConsumablePoints ?? 5;
    }

    public void AddSpell(SpellInfo spellInfo)
    {
        spellInfos.Add(spellInfo);
    }

    public void RemoveSpell(SpellInfo spellInfo)
    {
        spellInfos.Remove(spellInfo);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public SpellInfo GetSelectedSpellInfo()
    {
        return selectedSpellInfo;
    }

    public Transform GetSpellParent()
    {
        return spellParent;
    }

    private void CreateProjectilePool()
    {
        foreach (SpellInfo spellInfo in spellInfos)
        {
            if (spellInfo.spellBehaviour.spellType == SpellType.PROJECTILE)
            {
                PoolManager.Instance.CreatePool(spellInfo.spellBehaviour.projectileSpellPrefab, 5);
            }
        }
    }
}

public struct SpellUIEvent : IEvent
{
    public List<SpellInfo> spellInfos1;
}

public struct SelectedSpellEvent : IEvent
{
    public SpellInfo SelectedSpellInfo;

    public SelectedSpellEvent(SpellInfo spellInfo)
    {
        SelectedSpellInfo = spellInfo;
    }
}