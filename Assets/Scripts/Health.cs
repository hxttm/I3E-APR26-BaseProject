using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("UI References")]
    public GameObject healthBarContainer; 
    public TextMeshProUGUI healthText;    

    private List<GameObject> heartObjects = new List<GameObject>();

    [Header("Damage Cooldown")]
    public float damageCooldown = 1.0f; 
    private float nextDamageTime = 0f;

    private bool isDead = false;

    [Header("Inventory States")]
    // This is the variable the PlayerFlight script looks at!
    public bool hasWingsInInventory = false; 

    void Start()
    {
        currentHealth = maxHealth;
        InitializeHearts();
        UpdateHealthUI();
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
        
        UpdateHealthUI();
        nextDamageTime = Time.time + damageCooldown;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth + "/" + maxHealth;
        }

        if (heartObjects == null || heartObjects.Count == 0) return;

        for (int i = 0; i < heartObjects.Count; i++)
        {
            if (heartObjects[i] != null)
            {
                heartObjects[i].SetActive(i < currentHealth);
            }
        }
    }

    // Called by WingChest when opened successfully
    public void UnlockFlightAccess()
    {
        hasWingsInInventory = true;
        Debug.Log("Wings added to inventory! PlayerFlight script is now unlocked.");
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player Died!");
    }
}