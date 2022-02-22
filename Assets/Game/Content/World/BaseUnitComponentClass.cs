using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseUnitComponentClass : MonoBehaviour
{
    public float health { get { return _health; } set { _health = value; } }
    float _health;

    public uint maxHealth = 100;
    public bool unbreakable = false;
    public string displayName = "--auto--";

    public GameUnitInfo unitInfo { get; }

    public List<DamageFactor> damageFactors = new List<DamageFactor>
    {
        new DamageFactor(DamageType.True, 1f ),
        new DamageFactor(DamageType.Soft,1f ),
        new DamageFactor(DamageType.Hard, 1f ),
        new DamageFactor(DamageType.Impulse, 1f )
    };

    public float ApplyDamage(uint value, DamageType type)
    {
        if (!unbreakable)
            _health -= value * damageFactors.First((f) => { return f.type == type; }).factor;
        
        return _health;
    }
}
