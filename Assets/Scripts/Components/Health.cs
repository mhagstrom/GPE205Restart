using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private int maxHealth = 100;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void OnDie()
    {
        Destroy (gameObject);
    }
    
    public void OnDamageTaken(int damage)
    {
        currentHealth -= damage;
        Debug.Log(damage + "damage taken! " + "Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            OnDie();
            Debug.Log("Dead!");
        }
    }
    
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
