using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "SpeedBoost", menuName = "Powerups/SpeedBoost")]
public class SpeedBoost : Powerup
{
    public float speedBoost = 1.5f;
   
    public override void UpdatePowerup(PowerupManager target)
    {
        base.UpdatePowerup(target);

        target.speedMultiplier = speedBoost;
    }
    
    public override void Apply(PowerupManager target)
    {

    }

    public override void Remove(PowerupManager target)
    {
        target.speedMultiplier = 1f;
    }
}
