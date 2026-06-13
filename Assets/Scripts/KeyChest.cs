using UnityEngine;
using TMPro; // Required for TextMeshPro references

public class KeyChest : MonoBehaviour
{
    private bool isPlayerNearby = false;
    private bool isChestOpened = false;

    [Header("Chest Requirements")]
    public int gemCost = 3;

    [Header("Chest Parts")]
    public Transform chestLid; // Drag 'GLid' here
    public Transform spawnPoint; 

    [Header("Physics Settings")]
    public bool lidFallsOffWithForce = true;

    [Header("Spawn Settings")]
    public GameObject orbPrefab; 

    [Header("Local UI Text Reference")]
    public TextMeshProUGUI promptText; // DRAG YOUR 'KText' OBJECT HERE!

    [Header("Inventory UI Reference")]
    public GameObject inventoryOrbSlot; // Drag your UI Inventory Orb Image slot here

    [Header("Orb Sprite Art")]
    public Sprite orbIconSprite;

    void Start()
    {
        // Hide the inventory icon at the start of the game
        if (inventoryOrbSlot != null)
        {
            inventoryOrbSlot.SetActive(false); 
        }

        // Force the local chest text to be completely blank on boot
        if (promptText != null)
        {
            promptText.text = "";
        }
    }

    void Update()
    {
        if (isPlayerNearby && !isChestOpened && Input.GetKeyDown(KeyCode.O))
        {
            TryOpenChest();
        }
    }

    private void TryOpenChest()
    {
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        bool hasEnoughGems = true; 

        if (hasEnoughGems)
        {
            isChestOpened = true;

            // Blow the lid off using physics
            if (chestLid != null)
            {
                Rigidbody lidRb = chestLid.gameObject.GetComponent<Rigidbody>();
                if (lidRb == null)
                {
                    lidRb = chestLid.gameObject.AddComponent<Rigidbody>();
                }
                lidRb.AddForce(-transform.forward * 4f + Vector3.up * 5f, ForceMode.Impulse);
                lidRb.AddTorque(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), ForceMode.Impulse);
            }

            if (player != null)
            {
                player.UnlockPortalOrb();
            }

            if (inventoryOrbSlot != null)
            {
                inventoryOrbSlot.SetActive(true); 
            }

            // Clear the text because the chest is successfully opened!
            if (promptText != null)
            {
                promptText.text = "";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isChestOpened)
        {
            isPlayerNearby = true;
            
            // Set the text content exactly when the player gets close!
            if (promptText != null)
            {
                promptText.text = "This chest contains the Key Orb\nPress \"o\" to open for 3 Gems\n(-3 Gems)";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            
            // Wipe the text box clean when walking away
            if (promptText != null && !isChestOpened)
            {
                promptText.text = "";
            }
        }
    }
}