
using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    public Action onGrenadeBehaviourComplete;

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float movespeed = 5f;
        transform.position += moveDir * movespeed * Time.deltaTime;
        float reachedtargetDistance = 0.2f;
        if (Vector3.Distance(targetPosition, transform.position) < reachedtargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out Unit targetUnit))
                {
                    targetUnit.Damage(30, targetUnit);
                }
            }
            Destroy(gameObject);
            onGrenadeBehaviourComplete();
        }
    }
    public void Setup(GridPosition gridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }
}
