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
    public bool hasGreenOrbInInventory = false; 

    [Header("Throw Settings")]
    public GameObject orbProjectilePrefab; // Drag your 3D Orb prefab with the OrbProjectile script here
    public Transform throwSpawnPoint;       // Drag your Main Camera here
    
    // --- ADDED THIS SLOT SO THE PLAYER CAN TURN IT OFF WHEN THROWN ---
    public GameObject inventoryOrbSlot;     // Drag your UI Inventory Orb Image slot here

    void Start()
    {
        currentHealth = maxHealth;
        InitializeHearts();
        UpdateHealthUI();

        regenCoroutine = StartCoroutine(RegenerateHealthRoutine());
    }

    void Update()
    {
        // If the player has the orb and presses T, throw it!
        if (hasGreenOrbInInventory && Input.GetKeyDown(KeyCode.T))
        {
            ThrowOrb();
        }
    }

    private void ThrowOrb()
    {
        if (orbProjectilePrefab != null && throwSpawnPoint != null)
        {
            // Remove the orb from inventory data so you can only throw it once
            hasGreenOrbInInventory = false;

            // --- TURNS OFF THE VISUAL ICON AUTOMATICALLY ON THROW ---
            if (inventoryOrbSlot != null)
            {
                inventoryOrbSlot.SetActive(false);
            }

            // Spawn the orb at the camera position
            GameObject thrownOrb = Instantiate(orbProjectilePrefab, throwSpawnPoint.position, throwSpawnPoint.rotation);
            
            // Launch it straight forward from where the camera is looking
            OrbProjectile projectileScript = thrownOrb.GetComponent<OrbProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Launch(throwSpawnPoint.forward);
            }
        }
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

    public void UnlockPortalOrb()
    {
        hasGreenOrbInInventory = true;
        Debug.Log("Magical Green Orb added to inventory! Portal access is now available.");
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