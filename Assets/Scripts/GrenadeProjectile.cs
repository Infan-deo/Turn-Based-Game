
using System;
using Ami.BroAudio;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    public Action onGrenadeBehaviourComplete;
    public static event EventHandler OnAnyGrenadeExploded;

    public Transform GrenadeExplosionVfxPrefab;

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    private float totalDistance;
    private Vector3 positionXZ;


    private void Update()
    {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;
        float movespeed = 10f;
        positionXZ += moveDir * movespeed * Time.deltaTime;

        float distance = Vector3.Distance(targetPosition, positionXZ);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 5f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);
        float reachedtargetDistance = 0.2f;
        if (Vector3.Distance(targetPosition, positionXZ) < reachedtargetDistance)
        {
            float damageRadius = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out Unit targetUnit))
                {
                    UnitManager.Instance.SetUnitRagdollFallDir(transform.position);
                    targetUnit.Damage(30,new GrenadeAction());
                }
                if (collider.TryGetComponent(out DestructableCrate destructableCrate))
                {
                    destructableCrate.DestroyCrate();
                }
            }
            Destroy(gameObject);
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(GrenadeExplosionVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
            SFXGameManager.Instance.PlayExplosionSound();
            onGrenadeBehaviourComplete();
        }
    }
    public void Setup(GridPosition gridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }

}
