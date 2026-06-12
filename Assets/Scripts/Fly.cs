using UnityEngine;
using StarterAssets; 

public class Fly : MonoBehaviour
{
    [Header("Flight Settings")]
    public float flySpeed = 5f;
    private bool isFlying = false;

    [Header("References")]
    private PlayerHealth playerHealth;
    private CharacterController characterController;
    private FirstPersonController fpController;

    void Start()
    {
        // Gather all components on the player object
        playerHealth = GetComponent<PlayerHealth>();
        characterController = GetComponent<CharacterController>();
        fpController = GetComponent<FirstPersonController>();

        if (playerHealth == null) Debug.LogError("Fly: Missing PlayerHealth component!");
        if (characterController == null) Debug.LogError("Fly: Missing CharacterController component!");
        if (fpController == null) Debug.LogError("Fly: Missing FirstPersonController component!");
    }

    void Update()
    {
        if (playerHealth == null || fpController == null || characterController == null) return;

        // Verify the player has the wings unlocked in their PlayerHealth inventory data
        if (playerHealth.hasWingsInInventory && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlight();
        }

        if (isFlying)
        {
            HandleFlightMovement();
        }
    }

    private void ToggleFlight()
    {
        isFlying = !isFlying;

        if (isFlying)
        {
            // Turn off the asset's normal movement script so it stops calculating gravity
            fpController.enabled = false; 
            Debug.Log("Take-off! (F) Normal movement paused. Use WASD to glide, Space to rise, Shift to sink.");
        }
        else
        {
            // Turn the script back on to let the asset resume normal walking and gravity
            fpController.enabled = true; 
            Debug.Log("Landed! (F) Normal movement and gravity restored.");
        }
    }

    private void HandleFlightMovement()
    {
        // 1. Handle Horizontal Movement (WASD) manually while flying
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down arrows

        // Calculate directions relative to where the player's body is facing
        Vector3 moveDirection = (transform.forward * vertical) + (transform.right * horizontal);

        // 2. Handle Vertical Movement (Space / Left Shift)
        if (Input.GetKey(KeyCode.Space))
        {
            moveDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveDirection += Vector3.down;
        }

        // 3. Apply the combined flight movement to the character controller
        characterController.Move(moveDirection.normalized * flySpeed * Time.deltaTime);
    }
}