using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Behaviours/CastingSpell")]
public class CastingSpellBehaviour : SpellBehaviour
{
    
    public override IEnumerator Execute(
      Unit caster,
      Unit targetUnit,
      SpellInfo spellInfo, SpellAction spellAction)
    {

        spellAction.Aim();
        yield return new WaitForSeconds(0.25f);
        caster.GetCinemachineCamera().Priority = 50;
        caster.GetCinemachineCamera().gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        spellAction.OnSpellActionStarted?.Invoke();
        yield return new WaitForSeconds(1.5f);
        caster.GetCinemachineCamera().Priority = 0;
        caster.GetCinemachineCamera().gameObject.SetActive(false);
        targetUnit.GetCinemachineCamera().Priority = 50;
        targetUnit.GetCinemachineCamera().gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Transform SpellPrefabTransform = Instantiate(Spellprefab, spellAction.spellParent);
        SpellPrefabTransform.gameObject.SetActive(true);
        float yPosition = SpellPrefabTransform != null ? SpellPrefabTransform.position.y : (targetUnit != null ? targetUnit.transform.position.y : 0f);
        if (targetUnit != null && SpellPrefabTransform != null)
        {
            SpellPrefabTransform.position = new Vector3(targetUnit.transform.position.x, yPosition, targetUnit.transform.position.z);
        }
        yield return new WaitForSeconds(1.5f);
        targetUnit.GetCinemachineCamera().gameObject.SetActive(false);
        targetUnit.Damage(SpellDamage);
        yield return new WaitForSeconds(1.5f);
        Destroy(SpellPrefabTransform.gameObject);
        spellAction.OnSpellActionCompleted?.Invoke();
        yield break;
    }

    public override string GetSpellTypeName()
    {
        return "Casting";
    }
}
