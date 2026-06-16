using System;
using Unity.Mathematics;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    public Animator animator;
    public Transform bulletProjectilePrefab;
    public Transform ShootPointTranform;
    public Transform rifleTransform;
    public Transform swordTransform;




    private void Awake()
    {
        if (TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;

        }
        if (TryGetComponent(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;

        }
        if (TryGetComponent(out SpellAction SpellAction))
        {
            SpellAction.OnSpellActionStarted += SpellAction_OnSpellActionStarted;
            SpellAction.OnSpellActionCompleted += SpellAction_OnSpellActionCompleted;

        }
    }

    private void SpellAction_OnSpellActionCompleted()
    {
         EquipRifle();
    }

    private void SpellAction_OnSpellActionStarted()
    {
       
        UnequipAll();
        animator.SetTrigger("CastSpell");
    }

    private void Start()
    {
        EquipRifle();
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        EquipSword();
        animator.SetTrigger("Slash");
    }
    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
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

    public void EquipSword()
    {
        rifleTransform.gameObject.SetActive(false);
        swordTransform.gameObject.SetActive(true);
    }
    public void EquipRifle()
    {
        rifleTransform.gameObject.SetActive(true);
        swordTransform.gameObject.SetActive(false);
    }
    public void UnequipAll()
    {
        rifleTransform.gameObject.SetActive(false);
        swordTransform.gameObject.SetActive(false);
    }
}
