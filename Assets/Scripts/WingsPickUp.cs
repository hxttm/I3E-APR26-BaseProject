using UnityEngine;
using StarterAssets; // Tells Unity to look inside the Starter Assets folder!

public class WingsPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object touching the wings is the PlayerCapsule
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule")
        {
            // Now Unity can find the FirstPersonController script perfectly!
            FirstPersonController fpController = other.GetComponent<FirstPersonController>();
            
            if (fpController != null)
            {
                Debug.Log("Wings Equipped! You can now use your flight mechanics!");
                
                // Destroy the floating physical wings object so it goes into your inventory
                Destroy(gameObject);
            }
        }
    }
}