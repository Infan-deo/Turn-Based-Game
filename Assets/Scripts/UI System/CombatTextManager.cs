using System;
using UnityEngine;

public class CombatTextManager : MonoBehaviour
{
    [SerializeField] private Transform combatTextUIPrefab;
    [SerializeField] private Transform combatTextUIParentTransform;
    [SerializeField] private Unit unit;


    private void OnEnable()
    {
        unit.OnCombatAction += Unit_OnCombatAction;
    }

    private void Unit_OnCombatAction(object sender, Unit.OnCombatActionEventArgs e)
    {
        SpawnFloatingText(e.Amount, e.isheal);
    }

    private void Start()
    {
        PoolManager.Instance.CreatePool(combatTextUIPrefab, 5);
    }

    public void SpawnFloatingText(int num, bool isheal)
    {
        Transform combatTextUIprefabTransform = PoolManager.Instance.Get(combatTextUIPrefab);
        CombatTextUI combatTextUI = combatTextUIprefabTransform.GetComponent<CombatTextUI>();
        combatTextUI.transform.SetPositionAndRotation(
            combatTextUIParentTransform.position,
            combatTextUIParentTransform.rotation);
        combatTextUI.Show(num, isheal,
            () => PoolManager.Instance.Return(combatTextUIPrefab, combatTextUIprefabTransform));
    }
}