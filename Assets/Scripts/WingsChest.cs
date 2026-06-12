using UnityEngine;
using System.Collections;
using UnityEngine.UI; // REQUIRED to change UI Images!

public class WingChest : MonoBehaviour
{
    [Header("Chest Parts")]
    public Transform chestLid;
    public Transform spawnPoint;

    [Header("Physics Settings")]
    [Tooltip("If true, the lid will fly off using physics. If false, it opens smoothly.")]
    public bool lidFallsOffWithPhysics = true; 

    [Header("Spawn Settings")]
    public GameObject wingsPrefab; 

    [Header("Canvas UI Reference")]
    public GameObject canvasWingsText; 

    [Header("Inventory UI Reference")]
    [Tooltip("Drag the grey Wing Image slot from your main Canvas here.")]
    public GameObject inventoryWingSlotImage; 

    // --- NEW: ADD THIS FOR THE ARTWORK ---
    [Header("Wings Sprite Art")]
    [Tooltip("Drag your actual Steampunk Wings icon/sprite texture here.")]
    public Sprite wingsIconSprite; 

    private bool playerIsClose = false;
    private bool opened = false;
    private bool isOpening = false;

    private float openAngle = 90f; 
    private float openSpeed = 5f;
    private Quaternion openRotation;
    
    private PlayerHealth targetPlayerHealth; 

    void Start()
    {
        if (chestLid != null && !lidFallsOffWithPhysics)
            openRotation = Quaternion.Euler(openAngle, 0f, 0f);

        if (canvasWingsText != null)
            canvasWingsText.SetActive(false);

        if (inventoryWingSlotImage != null)
            inventoryWingSlotImage.SetActive(false);
    }

    void Update()
    {
        if (isOpening && chestLid != null && !lidFallsOffWithPhysics)
        {
            chestLid.localRotation = Quaternion.Slerp(chestLid.localRotation, openRotation, Time.deltaTime * openSpeed);
            if (Quaternion.Angle(chestLid.localRotation, openRotation) < 1f)
            {
                chestLid.localRotation = openRotation;
                isOpening = false;
            }
        }

        if (playerIsClose && !opened && !isOpening && targetPlayerHealth != null)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                TryOpenWingChest();
            }
        }
    }

    private void TryOpenWingChest()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.crystalCount < 3)
        {
            Debug.Log("Not enough gems to open the steampunk wings chest!");
            return;
        }

        GameManager.Instance.RemoveCrystals(3);
        
        opened = true;
        isOpening = true;

        if (canvasWingsText != null)
            canvasWingsText.SetActive(false);

        if (lidFallsOffWithPhysics && chestLid != null)
        {
            PopLidOff();
        }

        UnlockAndPopWings();
    }

    private void PopLidOff()
    {
        chestLid.transform.SetParent(null); 

        Rigidbody lidRb = chestLid.gameObject.GetComponent<Rigidbody>();
        if (lidRb == null)
        {
            lidRb = chestLid.gameObject.AddComponent<Rigidbody>();
        }

        lidRb.AddForce(new Vector3(0f, 4f, -3f), ForceMode.Impulse);
        lidRb.AddTorque(new Vector3(Random.Range(-5f, 5f), 0f, 0f), ForceMode.Impulse);
    }

    private void UnlockAndPopWings()
    {
        targetPlayerHealth.UnlockFlightAccess();

        // Show the slot object
        if (inventoryWingSlotImage != null)
        {
            inventoryWingSlotImage.SetActive(true);

            // --- NEW: SWAP THE GREY BOX FOR THE ART ---
            Image uiImageComponent = inventoryWingSlotImage.GetComponent<Image>();
            if (uiImageComponent != null && wingsIconSprite != null)
            {
                uiImageComponent.sprite = wingsIconSprite; // Replaces the grey box with your art!
                uiImageComponent.color = Color.white;    // Ensures it isn't tinted dark/grey
            }
        }

        if (wingsPrefab != null && spawnPoint != null)
        {
            GameObject spawnedWings = Instantiate(wingsPrefab, spawnPoint.position, Quaternion.identity);
            
            Rigidbody rb = spawnedWings.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(new Vector3(0f, 6f, 1.5f), ForceMode.Impulse);
            }
        }
        
        Debug.Log("Steampunk wings popped up and UI image updated!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule")
        {
            playerIsClose = true;
            targetPlayerHealth = other.GetComponent<PlayerHealth>();

            if (!opened && canvasWingsText != null)
            {
                canvasWingsText.SetActive(true); 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule")
        {
            playerIsClose = false;
            targetPlayerHealth = null; 

            if (canvasWingsText != null)
            {
                canvasWingsText.SetActive(false); 
            }
        }
    }
}