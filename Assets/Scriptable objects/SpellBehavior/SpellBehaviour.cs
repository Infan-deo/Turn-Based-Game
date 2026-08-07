using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SpellBehaviour : ScriptableObject
{
    public int SpellDamage;
    public int SpellID;
    public SpellType spellType;
    public abstract string GetSpellTypeName();

    private bool IsCastingOrShield =>
        spellType == SpellType.CASTING ||
        spellType == SpellType.SHIELD;

    [ShowIf(nameof(IsCastingOrShield))] public Transform Spellprefab;

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
        SpellAction spellAction,
        Action OnSpellActionComplted);
}