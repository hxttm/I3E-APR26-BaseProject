using UnityEngine;
using System.Collections;

public class WingsChest : MonoBehaviour
{
    [Header("Canvas UI Reference")]
    public GameObject canvasWingsObject; // Drag your Wings Canvas/Text object here!

    [Header("Inventory Settings")]
    public GameObject wingsInventoryIcon; // Drag the UI Icon that sits under your Crystal Counter here!

    private bool playerIsClose = false;
    private bool unlocked = false;

    void Start()
    {
        // Hide the prompt text when the game starts
        if (canvasWingsObject != null)
            canvasWingsObject.SetActive(false);

        // Keep the wings inventory icon hidden until bought!
        if (wingsInventoryIcon != null)
            wingsInventoryIcon.SetActive(false);
    }

    void Update()
    {
        // Check for player press if they are inside the zone and haven't unlocked it yet
        if (playerIsClose && !unlocked)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                TryUnlockWings();
            }
        }
    }

    private void TryUnlockWings()
    {
        if (GameManager.Instance == null) return;

        // Check if player has at least 3 jems
        if (GameManager.Instance.crystalCount < 3)
        {
            Debug.Log("Not enough jems to unlock wings!");
            return;
        }

        // Deduct exactly 3 jems
        GameManager.Instance.RemoveCrystals(3);
        unlocked = true;

        // Hide the popup text completely
        if (canvasWingsObject != null)
            canvasWingsObject.SetActive(false);

        // ACTIVATE the wings icon right under your crystal counter!
        if (wingsInventoryIcon != null)
        {
            wingsInventoryIcon.SetActive(true);
        }

        // Trigger the flight unlock on your actual character script
        WingsPickUp pickup = GetComponent<WingsPickUp>();
        if (pickup == null) pickup = GetComponentInParent<WingsPickUp>();
        
        if (pickup != null)
        {
            Debug.Log("Flight Mechanics Activated!");
        }

        // Optional: Destroy the physical floating wings mesh if needed, 
        // but leave this parent object alive if it holds your scripts!
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule")
        {
            playerIsClose = true;
            if (!unlocked && canvasWingsObject != null)
            {
                canvasWingsObject.SetActive(true); // Pop up the text!
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule")
        {
            playerIsClose = false;
            if (canvasWingsObject != null)
            {
                canvasWingsObject.SetActive(false); // Hide the text!
            }
        }
    }
}