using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPowerup", menuName = "Powerup/HealthPowerup")]

public class HealthPowerup : Powerup
{
    public int amt;

    public override void Apply(PowerupManager target)
    {
        //add x health to the health component of the target
        target.GetComponent<Health>().Heal(amt);
    }

    public override void Remove(PowerupManager target)
    {
        
    }
}
