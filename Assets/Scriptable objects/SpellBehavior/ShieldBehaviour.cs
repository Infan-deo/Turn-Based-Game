using System;
using System.Collections;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Spells/Behaviours/ShieldBehaviour")]
public class ShieldBehaviour : SpellBehaviour
{
    public override string GetSpellTypeName()
    {
        return "shield";
    }

    public override IEnumerator Execute(Unit caster, Unit target, SpellInfo spellInfo, SpellAction spellAction,
        Action OnSpellActionComplted)
    {
        CameraManager.Instance.SetActionCameraAtUnitCameraPoint(target, 0);
        CameraManager.Instance.ShowActionCamera();
        yield return new WaitForSeconds(0.75f);
        spellAction.OnSpellActionStarted?.Invoke(spellAction.selectedspellType);
        yield return new WaitForSeconds(1f);

        target.CreateShield(); //Shield shader is controlled by
        yield return new WaitForSeconds(1.5f);
        spellAction.OnSpellActionCompleted?.Invoke();
        yield return new WaitForSeconds(1.0f);
        CameraManager.Instance.HideActionCamera();
        yield return new WaitForSeconds(0.5f);
        OnSpellActionComplted?.Invoke();
    }
}