using UnityEngine;

public class UnitAttackManager : Singleton<UnitAttackManager>
{
    public BaseAction UsedActionToAttack;
    
    public void UnitAttacked(Unit targetedUnit, BaseAction UsedActionToAttack)
    {
        this.UsedActionToAttack = UsedActionToAttack;
    }

    public void OnUnitDied(Unit unit)
    {

        if (UsedActionToAttack is ShootAction or GrenadeAction)
        {
            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(unit.transform.position);
            LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, unit);
            UnitManager.Instance.RemoveAnUnitFormFriendlyList(unit);
            Destroy(unit.gameObject);
        }
        else
        {
            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(unit.transform.position);
            LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, unit);
            UnitManager.Instance.RemoveAnUnitFormFriendlyList(unit);
            Destroy(unit.gameObject, 5.0f);
        }

        //Remove it

    }

    public bool CanSpawnRagdoll()
    {
        if (UsedActionToAttack is ShootAction or GrenadeAction)
        {
            return true;
        }
        return false;
    }



}
