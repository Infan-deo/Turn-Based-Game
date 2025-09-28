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
    }

    public void HideAllGridPosition()
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
    private void Update()
    {
        UpdateGridVisual();
    }

    private void UpdateGridVisual()
    {
        HideAllGridPosition();     
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        ShowGridPositionList(selectedAction.GetvalidGridPositionList());
    }


}
