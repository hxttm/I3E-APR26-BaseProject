using UnityEngine;

public class WingsJems : MonoBehaviour
{
    [Header("Canvas UI Reference")]
    public GameObject canvasWingsObject; // Drag your Wings Canvas text here!
    public TMPro.TextMeshProUGUI promptText; // Optional: Drag the text component here to change the words dynamically!

    [Header("Inventory Settings")]
    public GameObject wingsInventoryIcon; // Drag your UI icon here!

    private bool playerIsClose = false;
    private bool unlocked = false;
    private bool collected = false;

    void Start()
    {
        if (canvasWingsObject != null) canvasWingsObject.SetActive(false);
        if (wingsInventoryIcon != null) wingsInventoryIcon.SetActive(false);
    }

    void Update()
    {
        if (!playerIsClose) return;

        // STEP 1: Press O to unlock/open for 3 jems
        if (!unlocked && Input.GetKeyDown(KeyCode.O))
        {
            TryUnlockWings();
        }
        // STEP 2: Press Q to pick up into inventory
        else if (unlocked && !collected && Input.GetKeyDown(KeyCode.Q))
        {
            CollectWings();
        }
    }

    private void TryUnlockWings()
    {
        if (GameManager.Instance == null || GameManager.Instance.crystalCount < 3) return;

        GameManager.Instance.RemoveCrystals(3);
        unlocked = true;

        // Change text prompt to tell the player to press Q
        if (promptText != null)
        {
            promptText.text = "Press 'Q' to take Wings";
        }
    }

    private void CollectWings()
    {
        collected = true;

        if (canvasWingsObject != null) canvasWingsObject.SetActive(false);
        if (wingsInventoryIcon != null) wingsInventoryIcon.SetActive(true);

        // Tell WingsPickUp that the player has the wings and can now use 'F' to fly!
        WingsPickUp pickup = GetComponent<WingsPickUp>();
        if (pickup != null)
        {
            pickup.UnlockFlightAccess();
        }

        // Hide the world model mesh, keeping the scripts alive
        if (GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            if (canvasWingsObject != null) canvasWingsObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            if (canvasWingsObject != null) canvasWingsObject.SetActive(false);
        }
    }
}