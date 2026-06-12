using UnityEngine;
using System.Collections.Generic;
using TMPro; // 1. CRITICAL: You must include this line at the top!

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    [Header("UI References")]
    public GameObject healthBarContainer; // Your hearts layout
    public TextMeshProUGUI healthText;    // 2. ADD THIS: Your TextMeshPro UI element

    private List<GameObject> heartObjects = new List<GameObject>();

    [Header("Damage Cooldown")]
    public float damageCooldown = 1.0f; 
    private float nextDamageTime = 0f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        InitializeHearts();
        UpdateHealthUI(); // This will update both hearts and text at start
    }

    private void InitializeHearts()
    {
        if (healthBarContainer != null)
        {
            heartObjects.Clear();
            for (int i = 0; i < healthBarContainer.transform.childCount; i++)
            {
                GameObject child = healthBarContainer.transform.GetChild(i).gameObject;
                if (!child.name.ToLower().Contains("bg") && !child.name.ToLower().Contains("background"))
                {
                    heartObjects.Add(child);
                }
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;
        if (Time.time < nextDamageTime) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        UpdateHealthUI(); // Updates the UI right after taking damage

        nextDamageTime = Time.time + damageCooldown;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        // 3. NEW CODE: Update the TextMesh Pro text on screen!
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth + "/" + maxHealth;
        }

        // Keep your existing heart toggling code below
        if (heartObjects == null || heartObjects.Count == 0) return;

        for (int i = 0; i < heartObjects.Count; i++)
        {
            if (heartObjects[i] != null)
            {
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