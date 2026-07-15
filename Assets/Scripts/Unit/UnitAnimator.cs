using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    public Animator animator;
    public Transform bulletProjectilePrefab;
    public Transform ShootPointTranform;
    public Transform rifleTransform;
    public Transform swordTransform;
    public Transform shieldTransform;
    public AnimationEventController animationEventController;





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
        if (TryGetComponent(out Unit Unit))
        {
            Unit.OnUnitAttacked += Unit_OnUnitAttacked;

        }
        if (TryGetComponent(out ParryController parryController))
        {
            parryController.OnParryExcuted += ParryController_OnParryExcuted;  
        }
        animationEventController.OnShildParryEndEvent += ParryController_OnParryCompleted;
    }

    private void ParryController_OnParryCompleted()
    {
        UnequipAll();
        EquipRifle();
        // animator.applyRootMotion = false;
    }

    private void ParryController_OnParryExcuted()
    {
        UnequipAll();
        EquipShield();
        // animator.applyRootMotion = true;
        animator.SetTrigger("ShieldParry");
    }

    private void Unit_OnUnitAttacked(object sender, EventArgs e)
    {
        animator.SetTrigger("Hit");
        if (TryGetComponent(out Unit Unit))
        {
            if (Unit.GetIsUnitDead())
            {
                if (!UnitAttackManager.Instance.CanSpawnRagdoll())
                {
                    animator.applyRootMotion = true;
                    animator.SetBool("IsDead", true);
                }
            }
        }
    }

    private void SpellAction_OnSpellActionCompleted()
    {
        EquipRifle();
    }

    private void SpellAction_OnSpellActionStarted(SpellType type)
    {
        UnequipAll();

        if (type == SpellType.CASTING)
        {
            animator.SetTrigger("CastSpell");
        }
        else if (type == SpellType.PROJECTILE)
        {
            animator.SetTrigger("CastSpell2");
        }
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
        SetTransformActive(rifleTransform, false);
        SetTransformActive(swordTransform, true);
    }
    public void EquipRifle()
    {
        SetTransformActive(rifleTransform, true);
        SetTransformActive(swordTransform, false);
    }

    public void EquipShield()
    {
        SetTransformActive(shieldTransform, true);
    }
    public void UnequipAll()
    {
        SetTransformActive(rifleTransform, false);
        SetTransformActive(swordTransform, false);
        SetTransformActive(shieldTransform, false);
    }

    private void SetTransformActive(Transform targetTransform, bool isActive)
    {
        if (targetTransform != null && targetTransform.gameObject != null)
        {
            targetTransform.gameObject.SetActive(isActive);
        }
    }
}
