using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
public class SpellAction : BaseAction
{
    private enum State
    {
        Aiming,
        Spellperform,
        CoolOff
    }
    [SerializeField] private int maxShootDistance = 7;

    [SerializeField] private float WaitforSpellAnimationComplete;

    public List<SpellInfo> spellInfos;

    private SpellInfo selectedSpellInfo;
    EventBinding<SelectedSpellEvent> selectedSpellEvent;
    public bool IsSpellSelected;
    public Transform spellParent;

    public Action OnSpellActionStarted;
    public Action OnSpellActionCompleted;

    public SpellType selectedspellType => selectedSpellInfo.spellBehaviour.spellType;
    Unit targetUnit;
    float spelltimer;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private void OnEnable()
    {
        selectedSpellEvent = new EventBinding<SelectedSpellEvent>(OnSpellSelected);
        EventBus<SelectedSpellEvent>.Register(selectedSpellEvent);
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
    }

    private void OnDisable()
    {
        EventBus<SelectedSpellEvent>.Deregister(selectedSpellEvent);
    }

    public void OnSpellSelected(SelectedSpellEvent @event)
    {
        selectedSpellInfo = @event.SelectedSpellInfo;
        UnitActionSystem.Instance.SetSelectedAction(this);
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
        spelltimer -= Time.deltaTime;
        if (spelltimer <= 0f)
        {
            OnSpellActionTimeCompleted();
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

                if (targetUnit.IsEnemy() == Unit.IsEnemy())
                {
                    continue;
                }

                if (selectedSpellInfo != null)
                {
                    if (selectedspellType == SpellType.PROJECTILE)
                    {

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
        spelltimer = selectedSpellInfo.spellDuration;
        
        if (selectedSpellInfo.spellBehaviour != null)
        {
            StartCoroutine(
            selectedSpellInfo.spellBehaviour.Execute(
            Unit,
            targetUnit,
            selectedSpellInfo, this));
        }

        ActionStart(onActionComplete);

    }


    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override int GetActionPointsCost()
    {
        return selectedSpellInfo.ConsumablePoints;
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
}
