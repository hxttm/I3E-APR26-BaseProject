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

    [Header("UI Inventory Reference")]
    public GameObject inventoryOrbSlot;     

    [Header("Throw Settings")]
    public GameObject orbProjectilePrefab; // Drag your 3D Orb prefab asset here
    public Transform throwSpawnPoint;       // Drag your Main Camera here

    void Start()
    {
        currentHealth = maxHealth;
        InitializeHearts();
        UpdateHealthUI();

        regenCoroutine = StartCoroutine(RegenerateHealthRoutine());
    }

    void Update()
    {
        // If the player has the orb and presses T, throw it out!
        if (hasGreenOrbInInventory && Input.GetKeyDown(KeyCode.T))
        {
            ThrowOrb();
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

    private void ThrowOrb()
    {
        if (orbProjectilePrefab != null && throwSpawnPoint != null)
        {
            // Remove the orb from inventory data so you can only throw it once
            hasGreenOrbInInventory = false;

            // Turns off the visual inventory slot icon automatically
            if (inventoryOrbSlot != null)
            {
                inventoryOrbSlot.SetActive(false);
            }

            // 1. Spawn and throw the physical orb out into the world visually
            GameObject thrownOrb = Instantiate(orbProjectilePrefab, throwSpawnPoint.position, throwSpawnPoint.rotation);
            OrbFly projectileScript = thrownOrb.GetComponent<OrbFly>();
            if (projectileScript != null)
            {
                projectileScript.Launch(throwSpawnPoint.forward);
            }

            // 2. NEW METHOD: Instantly find the portal plane and activate it if close enough
            PortalPlane portal = FindObjectOfType<PortalPlane>();
            if (portal != null)
            {
                // Calculate the distance between the player and the portal plane
                float distanceToPortal = Vector3.Distance(transform.position, portal.transform.position);

                // If you are within 15 units of the portal, it activates automatically!
                if (distanceToPortal <= 15f)
                {
                    portal.TurnPlaneGreen();
                }
                else
                {
                    Debug.Log("Orb thrown, but player is too far away from the portal to activate it. Distance: " + distanceToPortal);
                }
            }
        }
    }
}