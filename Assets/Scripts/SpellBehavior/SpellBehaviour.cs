using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SpellBehaviour : ScriptableObject
{
    public int SpellDamage;
    public SpellType spellType;
    public abstract string GetSpellTypeName();

    [ShowIf(nameof(spellType), SpellType.CASTING)]
    public Transform Spellprefab;

    [ShowIf(nameof(spellType), SpellType.PROJECTILE)]
    public Transform projectileSpellPrefab;
    [ShowIf(nameof(spellType), SpellType.PROJECTILE)]
    public Transform CastingSpellPrefab;
    [ShowIf(nameof(spellType), SpellType.PROJECTILE)]
    public Transform ExplosionSpellPrefab;

    public abstract IEnumerator Execute(
        Unit caster,
        Unit target,
        SpellInfo spellInfo,
        SpellAction spellAction);
}