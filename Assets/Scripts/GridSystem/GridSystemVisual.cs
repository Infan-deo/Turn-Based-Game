using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    public static GridSystemVisual Instance { get; private set; }

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

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
    }

    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.Getwidth(), LevelGrid.Instance.GetHeight()];
        for (int x = 0; x < LevelGrid.Instance.Getwidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform SingleVisualTranform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemVisualSingleArray[x, z] = SingleVisualTranform.GetComponent<GridSystemVisualSingle>();
            }
        }
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_SelectedActionChanged;
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPostion += LevelGrid_OnAnyUnitMovedGridPostion;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
        UpdateGridVisual();
    }



    public void HideAllGridPositions()
    {
        for (int x = 0; x < LevelGrid.Instance.Getwidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionsList)
    {
        foreach (GridPosition gridPosition in gridPositionsList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
    }


    // private void UpdateGridVisual()
    // {
    //     HideAllGridPosition();
    //     BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
    //     ShowGridPositionList(selectedAction.GetvalidGridPositionList());
    // }



    private void UpdateGridVisual()
    {
        HideAllGridPositions();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        float glow = 0f;
        Color32 color = Color.white;

        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                color = Color.green;
                break;
            case SpinAction spinAction:
                color = Color.cyan;
                glow = 1.5f;
                break;
            case ShootAction shootAction:
                color = Color.red;
                glow = 2.5f;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), new Color32(255, 0, 0, 100));
                break;
        }

        ShowGridPositionList
        (
            selectedAction.GetvalidGridPositionList(), color, glow
        );
    }


    public void ShowGridPositionList(List<GridPosition> gridPositionList, Color32 color, float glow)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].
                Show(color, glow);
        }
    }
    public void ShowGridPositionRange(GridPosition gridPosition, int range, Color32 color,float glow=0f)
    {
        List<GridPosition> gridPositionList = new();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // Not valid
                    continue;
                }
                int testDistance = Mathf.Abs(x) + Math.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, color, glow);
    }
    private void UnitActionSystem_SelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPostion(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            HideAllGridPositions();
        }
        else
        {
            UpdateGridVisual();
        }
    }


}
