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

    private void HealthSystem_OnDead(object sender, Unit e)
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, Quaternion.identity);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone,e.GetWorldPosition());
    }
}
