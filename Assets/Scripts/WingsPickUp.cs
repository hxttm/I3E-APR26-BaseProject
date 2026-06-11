using UnityEngine;

public class RaycastWingPickup : Interactable
{
    void Update()
    {
        // Listen for the Q key if the player is actively looking at the dropped wings
        if (PlayerInteraction.CurrentTarget == this && Input.GetKeyDown(KeyCode.Q))
        {
            OnInteract();
        }
    }

    public override string GetLookAtPrompt()
    {
        return "Press Q to wear Wings";
    }

    public override void OnInteract()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.EquipWingsToInventory();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null) 
                movement.UnlockFlying(); 
        }

        Destroy(gameObject);
    }
}