using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Regeneration Settings")]
    public float regenDelay = 23f; 
    private Coroutine regenCoroutine;

    [Header("UI References")]
    public GameObject healthBarContainer; 
    public TextMeshProUGUI healthText;    

    private List<GameObject> heartObjects = new List<GameObject>();

    [Header("Damage Cooldown")]
    public float damageCooldown = 1.0f; 
    private float nextDamageTime = 0f;

    private bool isDead = false;

    [Header("Inventory States")]
    public bool hasWingsInInventory = false; 

    void Start()
    {
        currentHealth = maxHealth;
        InitializeHearts();
        UpdateHealthUI();

        regenCoroutine = StartCoroutine(RegenerateHealthRoutine());
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

        ApplyDamage(damageAmount);
        nextDamageTime = Time.time + damageCooldown;
    }

    public void DamageFromFall()
    {
        if (isDead) return;
        
        ApplyDamage(2); 
    }

    private void ApplyDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- REGEN ROUTINE WITH ON-SCREEN MESSAGE ---
    private IEnumerator RegenerateHealthRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(regenDelay);

            if (currentHealth < maxHealth && !isDead)
            {
                currentHealth += 1;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                
                UpdateHealthUI();

                // --- SEND MESSAGE TO YOUR GAME SCREEN ---
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowFallWarning("time passed, wounds healed");
                }
            }
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

    public void UnlockFlightAccess()
    {
        hasWingsInInventory = true;
        Debug.Log("Wings added to inventory! PlayerFlight script is now unlocked.");
    }

    private void Die()
    {
        isDead = true;
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        }
        Debug.Log("Player Died!");
    }
}