using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using ZakhanSpellsPack;

[CreateAssetMenu(menuName = "Scriptable Objects/Spells/Behaviours/ProjectileSpell")]
public class ProjectileSpellBehaviour : SpellBehaviour
{
    Unit caster;
    Unit targetUnit;
    SpellInfo spellInfo;
    SpellAction spellAction;
    Projectile projectile;
    Transform spellPrefabTransform;
    public float projectileSpeed = 5f;
    Transform spellProjectilePrefabTransform;

    public override string GetSpellTypeName()
    {
        return "Projectile";
    }

    public void SetSequenceCameras(Unit caster, Unit targetUnit)
    {
        Transform cameraPoint = caster.GetUnitCameraTransform()[0];
        CameraManager.Instance.SetSequenceChildCameraPoint(0, cameraPoint);
        Transform cameraPoint2 = targetUnit.GetUnitCameraTransform()[1];
        CameraManager.Instance.SetSequenceChildCameraPoint(1, cameraPoint2);
    }

    public override IEnumerator Execute(Unit caster, Unit targetUnit, SpellInfo spellInfo, SpellAction spellAction,
        Action OnSpellActionComplted)
    {
        this.caster = caster;
        this.targetUnit = targetUnit;
        this.spellInfo = spellInfo;
        this.spellAction = spellAction;

        spellAction.Aim();
        yield return new WaitForSeconds(0.25f);
        CameraManager.Instance.SetActionCameraAtUnitCameraPoint(caster, 1);
        CameraManager.Instance.ShowActionCamera();
        yield return new WaitForSeconds(0.75f);
        spellAction.OnSpellActionStarted?.Invoke(spellAction.selectedspellType);
        yield return new WaitForSeconds(1f);
        CameraManager.Instance.SetActionCameraAtUnitCameraPoint(caster, 2);
        Transform SpellCastPoint = caster.SpellPoints[0];
        spellPrefabTransform = Instantiate(CastingSpellPrefab, SpellCastPoint);
        spellPrefabTransform.position = SpellCastPoint.position;
        caster.GetComponent<UnitAnimator>().animationEventController.OnSpellReleasedEvent += CreateProjectile;

        yield return new WaitForSeconds(1.5f);
        spellAction.OnSpellActionCompleted?.Invoke();
        yield return new WaitForSeconds(2.0f);
       
        CameraManager.Instance.HideActionCamera();
        yield return new WaitForSeconds(1.5f);
        OnSpellActionComplted?.Invoke();

        yield break;
    }

    public void OnProjectileDestroyed(Unit targetUnit)
    {
        PoolManager.Instance.Return(projectileSpellPrefab, spellProjectilePrefabTransform);
        targetUnit.Damage(SpellDamage, spellAction);
        projectile.OnProjectileDestroyed -= OnProjectileDestroyed;
        this.caster = null;
        this.targetUnit = null;
        this.spellInfo = null;
        this.spellAction = null;
    }

    public void CreateProjectile()
    {
        Transform SpellCastPoint = caster.SpellPoints[1];
        Destroy(spellPrefabTransform.gameObject);
        Debug.Log("Creating projectile");
        spellProjectilePrefabTransform = PoolManager.Instance.Get(projectileSpellPrefab);
        projectile = spellProjectilePrefabTransform.GetComponent<Projectile>();
        projectile.SetSpellProjectilePrefab(projectileSpellPrefab);

        projectile.transform.SetPositionAndRotation(
            SpellCastPoint.position,
            SpellCastPoint.rotation);

        Vector3 targetPosition = targetUnit.GetWorldPosition() + Vector3.up * 1.5f;
        Vector3 direction = (targetPosition - SpellCastPoint.position).normalized;

        float distance = Vector3.Distance(SpellCastPoint.position, targetPosition);
        projectile.SetVelocity(direction * projectileSpeed,projectileSpeed/2);
        //        projectile.transform.DOMove(
        //     projectile.transform.position + direction * projectileSpeed,
        //     1f
        // ).SetEase(Ease.Linear)

        projectile.OnProjectileDestroyed += OnProjectileDestroyed;
        caster.GetComponent<UnitAnimator>().animationEventController.OnSpellReleasedEvent -= CreateProjectile;
    }
}