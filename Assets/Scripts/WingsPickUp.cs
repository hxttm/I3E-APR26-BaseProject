using UnityEngine;
using TMPro;

public class WingPickup : MonoBehaviour
{
    private bool playerNearby = false;
    private GameObject popupPanel;
    private TMP_Text popupText;

    void Start()
    {
        // Find the chest's UI canvas dynamically so we don't have to assign it manually
        CanvasOpen chestCanvas = FindFirstObjectByType<CanvasOpen>(); 
        // Note: Replace "CanvasOpen" with the script/type your chest UI uses if needed, 
        // or look for your UI elements by tag.
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.Q))
        {
            WearWings();
        }
    }

    void WearWings()
    {
        // 1. Tell GameManager to show them in the inventory UI
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EquipWingsToInventory();
        }

        // 2. Tell the Player script they can now fly
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.UnlockFlying();
            }
        }

        // Hide UI prompt safely before destroying
        var chestUI = FindFirstObjectByType<WingChest>();
        if (chestUI != null && chestUI.popupPanel != null)
        {
            chestUI.popupPanel.SetActive(false);
        }

        // 3. Destroy the item on the ground
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            
            // Reuse your existing popup UI layout to tell them to press Q
            WingChest chestUI = FindFirstObjectByType<WingChest>();
            if (chestUI != null && chestUI.popupPanel != null)
            {
                chestUI.popupPanel.SetActive(true);
                chestUI.popupText.text = "Press Q to wear Wings";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            WingChest chestUI = FindFirstObjectByType<WingChest>();
            if (chestUI != null && chestUI.popupPanel != null)
            {
                chestUI.popupPanel.SetActive(false);
            }
        }
    }
}