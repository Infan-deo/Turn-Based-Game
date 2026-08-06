using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Spells/Behaviours/ShieldBehaviour")]
public class ShieldBehaviour : SpellBehaviour
{
    public int SpellDamage;
    public int SpellID;
    public SpellType spellType;


    public override string GetSpellTypeName()
    {
        return "shield";
    }

    public override IEnumerator Execute(Unit caster, Unit target, SpellInfo spellInfo, SpellAction spellAction,
        Action OnSpellActionComplted)
    {
        CameraManager.Instance.SetActionCameraAtUnitCameraPoint(caster, 1);
        CameraManager.Instance.ShowActionCamera();
        yield return new WaitForSeconds(0.75f);
        spellAction.OnSpellActionStarted?.Invoke(spellAction.selectedspellType);
        yield return new WaitForSeconds(1f);
        caster.CreateShield();
        
    }
}
