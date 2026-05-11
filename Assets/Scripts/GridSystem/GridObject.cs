using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem<GridObject> _gridSystem;
    private GridPosition _gridPosition;

    [SerializeField] private List<Unit> unitList;

    private IInteractable interactable;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString += unit + "\n";
        }
        return _gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }

    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return unitList[0];
        }
        else
        {
            return null;
        }
    }

    public IInteractable GetInteratable()
    {
        return interactable;
    }
    public void SetInteratable(IInteractable interactable)
    {
        this.interactable = interactable;
    }
}
