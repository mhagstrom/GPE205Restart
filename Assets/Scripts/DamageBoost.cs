using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageBoost", menuName = "Powerups/DamageBoost")]
public class DamageBoost : Powerup
{
    public float damageMult;
    public override void Apply(PowerupManager target)
    {
        
    }

    public override void UpdatePowerup(PowerupManager target)
    {
        base.UpdatePowerup(target);

        target.damageMultiplier = damageMult;
    }

    public override void Remove(PowerupManager target)
    {
        target.damageMultiplier = 1f;
    }
}
