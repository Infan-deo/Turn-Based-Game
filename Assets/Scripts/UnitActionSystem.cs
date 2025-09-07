using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public event EventHandler OnSelectedUnitChanged;

    public static UnitActionSystem Instance { get; private set; }

    [SerializeField] private PlayerUnitMovement _selectedUnit;
    [SerializeField] LayerMask UnitsLayerMask;

    private void Awake() {
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
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
            _selectedUnit.MovePlayer(MouseWorld.Getposition());
        }
    }

    public bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, UnitsLayerMask))
        {
            if (hitInfo.transform.TryGetComponent<PlayerUnitMovement>(out PlayerUnitMovement Unit))
            {
                SetSelectedUnit(Unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(PlayerUnitMovement unit)
    {
        _selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public PlayerUnitMovement GetSelectedUnit()
    {
        return _selectedUnit;
    }
}
