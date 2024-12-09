using UnityEngine;

public abstract class Powerup : ScriptableObject
{
    public float duration;

    [HideInInspector]
    public float timer;

    public abstract void Apply(PowerupManager target);

    public abstract void Remove(PowerupManager target);

    public void UpdateTimer(PowerupManager target)
    {
        if (timer <= 0)
        {
            return;
        }

        UpdatePowerup(target);

        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            Remove(target);
            target.ExpirePowerup(this);
        }
    }

    public virtual void UpdatePowerup(PowerupManager target)
    {

    }
}
