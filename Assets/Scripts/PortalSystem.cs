using UnityEngine;

public class PortalSystem : MonoBehaviour
{
    [Header("Portal Settings")]
    public Renderer portalPlaneRenderer; 
    public Material greenPortalMaterial; 

    private bool isPlayerNearby = false;
    private bool isPortalUnlocked = false;

    void Update()
    {
        // Removed the automated EndGame() check from here so it doesn't trigger too early!
    }

    // This function is called by the flying orb projectile when it hits the portal
    public void ActivatePortalWithOrb()
    {
        if (!isPortalUnlocked)
        {
            UnlockPortal();
        }
    }

    private void UnlockPortal()
    {
        isPortalUnlocked = true;

        if (portalPlaneRenderer != null && greenPortalMaterial != null)
        {
            portalPlaneRenderer.material = greenPortalMaterial;
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowFallWarning("Portal activated! Step through to exit.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            PlayerHealth player = other.GetComponent<PlayerHealth>();

            // --- IF THE PORTAL IS ALREADY GREEN, END THE GAME IMMEDIATELY ---
            if (isPortalUnlocked)
            {
                EndGame();
                return; // Stop running the rest of this function
            }

            if (!isPortalUnlocked)
            {
                if (player != null && !player.hasGreenOrbInInventory)
                {
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.ShowFallWarning("magical orb needed to unlock this portal");
                    }
                }
                else
                {
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.ShowFallWarning("Press T to throw the Magical Orb!");
                    }
                }
            }
        }
    }

    // Safety Check: If the player is already standing inside the portal when the orb hits it
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isPortalUnlocked)
        {
            EndGame();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            if (UIManager.Instance != null && !isPortalUnlocked)
            {
                UIManager.Instance.ShowFallWarning("");
            }
        }
    }

    private void EndGame()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowFallWarning("Game Completed!");
        }

        Debug.Log("Game Finished! Exiting application...");
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
}