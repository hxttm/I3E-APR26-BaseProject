using UnityEngine;
using StarterAssets; 

public class WingsPickUp : MonoBehaviour
{
    private bool hasWingsInInventory = false;
    private bool isFlying = false;
    private FirstPersonController fpController;

    void Start()
    {
        // Cache the player controller automatically via tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            fpController = player.GetComponent<FirstPersonController>();
        }
    }

    // Called by WingsJems when you press Q
    public void UnlockFlightAccess()
    {
        hasWingsInInventory = true;
        Debug.Log("Wings added to inventory. Press 'F' to fly!");
    }

    void Update()
    {
        // Only allow flying if the player has collected the wings
        if (hasWingsInInventory && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlight();
        }
    }

    private void ToggleFlight()
    {
        isFlying = !isFlying; // Switches between true and false every time F is hit

        if (fpController != null)
        {
            if (isFlying)
            {
                Debug.Log("Flight Activated! (F)");
                // fpController.SetFlightMode(true); // Call your flight mode variables inside StarterAssets here!
            }
            else
            {
                Debug.Log("Flight Deactivated! (F)");
                // fpController.SetFlightMode(false);
            }
        }
    }
}