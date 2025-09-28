using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UnitActionSystem : MonoBehaviour
{
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler OnActionStarted;
    public event EventHandler<bool> OnBusyStateChanged;

    public static UnitActionSystem Instance { get; private set; }

    [SerializeField] private Unit _selectedUnit;
    [SerializeField] LayerMask UnitsLayerMask;

    BaseAction selectedBaseAction;

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

    private void Start()
    {
        SetSelectedUnit(_selectedUnit);

    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (TryHandleUnitSelection()) return;


        HandleSelectedAction();

    }

    public bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, UnitsLayerMask))
            {
                if (hitInfo.transform.TryGetComponent<Unit>(out Unit Unit))
                {
                    if (Unit == _selectedUnit)
                    {
                        return false;
                    }
                    SetSelectedUnit(Unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.Getposition());
            if (!selectedBaseAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }

            if (!_selectedUnit.TrySpendActionPointsToTakeAction(selectedBaseAction))
            {
                return;
            }
            OnActionStarted?.Invoke(this, EventArgs.Empty);
            SetBusy(true);
            selectedBaseAction.TakeAction(mouseGridPosition, SetBusy);
        }
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        selectedBaseAction = unit.GetMoveAction();
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedBaseAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }

    private void SetBusy(bool state)
    {
        isBusy = state;
        OnBusyStateChanged?.Invoke(this, state);
    }
    public BaseAction GetSelectedAction()
    {
        return selectedBaseAction;
    }
}
