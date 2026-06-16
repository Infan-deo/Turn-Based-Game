using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;

public enum SpellType
{
   NONE,
   PROJECTILE,
   CASTING
}

[CreateAssetMenu(fileName = "SpellInfo", menuName = "Scriptable Objects/SpellInfo")]
public class SpellInfo : ScriptableObject
{
   public LocalizedString spellName;
   public LocalizedString spellDescription;
   public Sprite Spellimage;
   public int ConsumablePoints;
   public float spellDuration;  
   public SpellBehaviour spellBehaviour;

}
