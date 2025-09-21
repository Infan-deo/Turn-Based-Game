using DG.Tweening;
using UnityEngine;

public class Unit : MonoBehaviour
{

    GridPosition gridPosition;
    MoveAction moveAction;
    SpinAction spinAction;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        if (moveAction == null)
        {
            Debug.LogError("No MoveAction component found on " + gameObject.name);
        }
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }


    private void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPOsition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }


    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
}
