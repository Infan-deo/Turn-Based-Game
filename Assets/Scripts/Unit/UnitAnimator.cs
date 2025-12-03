using System;
using Unity.Mathematics;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    public Animator animator;
    public Transform bulletProjectilePrefab;
    public Transform ShootPointTranform;

    public Action<Vector3> OnTargetPositionObserved;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;

        }
    }


    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }
    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");
        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, ShootPointTranform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();
        Vector3 targetPostionShootAtPosition = e.targetedUnit.GetWorldPosition();
        targetPostionShootAtPosition.y = ShootPointTranform.position.y;       
        bulletProjectile.Setup(targetPostionShootAtPosition);
    }
}
