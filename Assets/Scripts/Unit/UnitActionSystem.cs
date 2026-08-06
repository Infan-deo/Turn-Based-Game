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

    public Transform SpellParent;

    public ActionButtonUI _selecetedActionButtonUI;


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
        if (!TurnSystem.Instance.IsPlayerTurn())
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
        if (InputManagerTBG.Instance.GetMouseButtonDownThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManagerTBG.Instance.GetMousePosition());
            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, UnitsLayerMask))
            {
                if (hitInfo.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == _selectedUnit)
                    {
                        return false;
                    }
                    if (unit.IsEnemy())
                    {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void HandleSelectedAction()
    {
        if (InputManagerTBG.Instance.GetMouseButtonDownThisFrame())
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
            if (selectedBaseAction is SpellAction)
            {
                SpellAction spellaction = selectedBaseAction.GetComponent<SpellAction>();
                if (!spellaction.IsSpellSelected)
                {
                    // call spell system ui
                    spellaction.DisplaySpells();
                    return;
                }
            }
            
            SetBusy(true);
            selectedBaseAction.TakeAction(mouseGridPosition, SetBusy);
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        selectedBaseAction = unit.GetAction<MoveAction>();
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedBaseAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RefreshSelectedAction()
    {
        _selecetedActionButtonUI.SetSelectedAction();
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
