using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Spells/Behaviours/CastingSpell")]
public class CastingSpellBehaviour : SpellBehaviour
{

    public void SetSequenceCameras(Unit caster, Unit targetUnit)
    {
        Transform cameraPoint = caster.GetUnitCameraTransform()[0];
        CameraManager.Instance.SetSequenceChildCameraPoint(0, cameraPoint);
        Transform cameraPoint2 = targetUnit.GetUnitCameraTransform()[1];
        CameraManager.Instance.SetSequenceChildCameraPoint(1, cameraPoint2);
    }

    public override IEnumerator Execute(
      Unit caster,
      Unit targetUnit,
      SpellInfo spellInfo, SpellAction spellAction,Action OnSpellActionComplted)
    {

        spellAction.Aim();
        yield return new WaitForSeconds(0.25f);
        SetSequenceCameras(caster, targetUnit);
        CameraManager.Instance.SetSequenceCameraState(true);
        yield return new WaitForSeconds(0.75f);
        spellAction.OnSpellActionStarted?.Invoke(spellAction.selectedspellType);
        SFXGameManager.Instance.PlaySpellCasting();
        yield return new WaitForSeconds(2.0f);
        SFXGameManager.Instance.PlayBlueFireSound();
        yield return new WaitForSeconds(1.0f);
        Transform SpellPrefabTransform = Instantiate(Spellprefab, spellAction.GetSpellParent());
        SpellPrefabTransform.gameObject.SetActive(true);
        float yPosition = SpellPrefabTransform != null ? SpellPrefabTransform.position.y : (targetUnit != null ? targetUnit.transform.position.y : 0f);
        if (targetUnit != null && SpellPrefabTransform != null)
        {
            SpellPrefabTransform.position = new Vector3(targetUnit.transform.position.x, yPosition, targetUnit.transform.position.z);
        }
        yield return new WaitForSeconds(0.5f);
        targetUnit.Damage(SpellDamage, spellAction);
        
        yield return new WaitForSeconds(1.5f);
        Destroy(SpellPrefabTransform.gameObject);
        spellAction.OnSpellActionCompleted?.Invoke();
        yield return new WaitForSeconds(1.5f);
        CameraManager.Instance.SetSequenceCameraState(false);        OnSpellActionComplted?.Invoke();

        yield break;
    }

    public override string GetSpellTypeName()
    {
        return "Casting";
    }
}
