using UnityEngine;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    [Header("UI Reference")]
    public GameObject healthBarContainer;
    
    private List<GameObject> heartObjects = new List<GameObject>();

    [Header("Damage Cooldown")]
    public float damageCooldown = 1.0f; 
    private float nextDamageTime = 0f;

    private bool isDead = false;

    void Start()
    {
        // FORCE the current health to be equal to your max health at start
        currentHealth = maxHealth;
        
        InitializeHearts();
        UpdateHealthUI();
    }

    private void InitializeHearts()
    {
        if (healthBarContainer != null)
        {
            heartObjects.Clear();

            // Loop through the layout components inside your Health Bar panel
            for (int i = 0; i < healthBarContainer.transform.childCount; i++)
            {
                GameObject child = healthBarContainer.transform.GetChild(i).gameObject;
                
                // Skip background elements if there are any
                if (!child.name.ToLower().Contains("bg") && !child.name.ToLower().Contains("background"))
                {
                    heartObjects.Add(child);
                }
            }
            Debug.Log($"[UI Fix] Successfully mapped {heartObjects.Count} heart GameObjects!");
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;
        if (Time.time < nextDamageTime) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        UpdateHealthUI();
        Debug.Log("Ouch! Lost a heart. Current Hearts: " + currentHealth);

        nextDamageTime = Time.time + damageCooldown;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (heartObjects == null || heartObjects.Count == 0) return;

        for (int i = 0; i < heartObjects.Count; i++)
        {
            if (heartObjects[i] != null)
            {
                // If the item index is less than your current health value, keep it active!
                heartObjects[i].SetActive(i < currentHealth);
            }
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player Died!");
    }
}