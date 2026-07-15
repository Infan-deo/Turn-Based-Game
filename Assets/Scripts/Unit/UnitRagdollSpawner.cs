using System;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;

    private HealthSystem healthSystem;


    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        if (UnitAttackManager.Instance.CanSpawnRagdoll())
        {
            SpawnRagdollWhenDie();
        }
    }

    private void SpawnRagdollWhenDie()
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, Quaternion.identity);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone, UnitManager.Instance.GetUnitRagDollFallDir());
    }
}
