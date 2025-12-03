using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjPrefab;
    private GridSystem<GridObject> gridSystem;
    public static LevelGrid Instance { get; private set; }
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    public event EventHandler OnAnyUnitMovedGridPostion;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        // gridSystem.CreateDebugObjects(gridDebugObjPrefab);
    }
    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }
    public List<Unit> GetUnitsAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();

    }
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public void UnitMovedGridPOsition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPOsition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPOsition, unit);
        OnAnyUnitMovedGridPostion?.Invoke(this, EventArgs.Empty);
    }

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public int Getwidth() => gridSystem.Getwidth();
    public int GetHeight() => gridSystem.GetHeight();

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }


}
