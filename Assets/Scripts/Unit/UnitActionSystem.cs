using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitActionSystem : MonoBehaviour
{
    public event EventHandler OnSelectedUnitChanged;

    public static UnitActionSystem Instance { get; private set; }

    [SerializeField] private Unit _selectedUnit;
    [SerializeField] LayerMask UnitsLayerMask;

    [SerializeField] private bool isBusy;

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

    private void Update()
    {
        if (isBusy)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.Getposition());
            if (_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy(true);
                _selectedUnit.GetMoveAction().MovePlayer(mouseGridPosition,SetBusy);
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            SetBusy(true);
            _selectedUnit.GetSpinAction().Spin(SetBusy);
        }
    }

    public bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, UnitsLayerMask))
        {
            if (hitInfo.transform.TryGetComponent<Unit>(out Unit Unit))
            {
                SetSelectedUnit(Unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }

    private void SetBusy(bool state)
    {
        isBusy = state;
    }
}
