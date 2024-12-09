using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public Dictionary<Powerup, int> inventory;
    public List<Powerup> activePowerups;
    
    public float damageMultiplier = 1;
    public float speedMultiplier = 1;
    
    public Powerup healthBoost, speedBoost, damageBoost;
    
    public delegate void InventoryAction(PowerupManager target);
    public event InventoryAction OnInventoryChanged;

    public Pawn Pawn;
    public void Awake() 
    {
        Pawn = GetComponent<Pawn>();
        inventory = new Dictionary<Powerup, int>();
        activePowerups = new List<Powerup>();
    }
    public void Add(Powerup PowerupAdding) 
    {
        inventory[PowerupAdding] = inventory.ContainsKey(PowerupAdding) ? inventory[PowerupAdding] + 1 : 1;
        OnInventoryChanged?.Invoke(this);
    }

    public void Remove(Powerup PowerupRemoving) 
    {
        if (inventory.ContainsKey(PowerupRemoving)) 
        {
            inventory[PowerupRemoving]--;

            if (inventory[PowerupRemoving] <= 0) 
            {
                inventory.Remove(PowerupRemoving);
            }
            OnInventoryChanged?.Invoke(this);
        }
    }

    public void ExpirePowerup(Powerup powerup)
    {
        if(activePowerups.Contains(powerup))
            activePowerups.Remove(powerup);
    }

    private void Update()
    {
        for(int i = 0; i <activePowerups.Count; i++)
        {
            var powerup = activePowerups[i];

            if (powerup == null) continue;

            powerup.UpdateTimer(this);
            DebugPlus.LogOnScreen($"{powerup.name} :{powerup.timer} / {powerup.duration}");
        }
    }

    public void ActivatePowerup(Powerup powerup)
    {
        if (inventory.ContainsKey(powerup))
        {
            powerup.Apply(this);
            if (powerup.duration > 0)
            {
                var instance = Instantiate(powerup);
                instance.timer = instance.duration; 
                activePowerups.Add(instance);
            }
            Remove(powerup);
        }
    }

    public void ActivatePowerup(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.HealthBoost:
                ActivatePowerup(healthBoost);
                break;
            case PowerupType.SpeedBoost:
                ActivatePowerup(speedBoost);
                break;
            case PowerupType.DamageBoost:
                ActivatePowerup(damageBoost);
                break;
        }
    }
    
    public int GetPowerupCount(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.HealthBoost:
                return inventory.ContainsKey(healthBoost) ? inventory[healthBoost] : 0;
            case PowerupType.SpeedBoost:
                return inventory.ContainsKey(speedBoost) ? inventory[speedBoost] : 0;
            case PowerupType.DamageBoost:
                return inventory.ContainsKey(damageBoost) ? inventory[damageBoost] : 0;
        }
        return 0;
    }
    
}

public enum PowerupType
{
    HealthBoost, //0
    SpeedBoost,  //1
    DamageBoost  //2
}
