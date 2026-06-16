using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    List<Unit> unitList;
    List<Unit> friendlyUnitList;
    List<Unit> enemyUnitList;

    public static UnitManager Instance { get; private set; }

    private Vector3 _unitRagDollFallDir;

    // private BaseAction _deadByAction;





    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        unitList = new();
        friendlyUnitList = new();
        enemyUnitList = new();

    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }



    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);
        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            friendlyUnitList.Add(unit);
        }
    }
    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);
        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyUnitList.Remove(unit);
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public List<Unit> GetFriendlyList()
    {
        return friendlyUnitList;
    }
    public List<Unit> GetEnemyList()
    {
        return enemyUnitList;
    }

    public void RemoveAnUnitFormFriendlyList(Unit unit)
    {
        if (friendlyUnitList.Contains(unit))
        {
            friendlyUnitList.Remove(unit);
        }
    }

    public void SetUnitRagdollFallDir(Vector3 dir)
    {
        _unitRagDollFallDir = dir;
    }

    public Vector3 GetUnitRagDollFallDir()
    {
        return _unitRagDollFallDir;
    }

    // public BaseAction GetDeadByAction()
    // {
    //     return _deadByAction;
    // }

    // public void SetDeadByAction(BaseAction baseAction)
    // {
    //     _deadByAction = baseAction;
    // }
}
