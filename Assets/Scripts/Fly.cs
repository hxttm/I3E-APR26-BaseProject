using UnityEngine;
using StarterAssets; 

public class Fly : MonoBehaviour
{
    [Header("Flight Settings")]
    public float flySpeed = 5f;
    [Tooltip("How high (in meters) the player must be above the ground to activate flying.")]
    public float minHeightToFly = 1.2f; 
    
    public bool isFlying = false;

    [Header("References")]
    private PlayerHealth playerHealth;
    private CharacterController characterController;
    private FirstPersonController fpController;
    private FallDamage fallDamage; 

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        characterController = GetComponent<CharacterController>();
        fpController = GetComponent<FirstPersonController>();
        fallDamage = GetComponent<FallDamage>();

        if (playerHealth == null) Debug.LogError("Fly: Missing PlayerHealth component!");
        if (characterController == null) Debug.LogError("Fly: Missing CharacterController component!");
        if (fpController == null) Debug.LogError("Fly: Missing FirstPersonController component!");
    }

    void Update()
    {
        if (playerHealth == null || fpController == null || characterController == null) return;

        if (playerHealth.hasWingsInInventory && Input.GetKeyDown(KeyCode.F))
        {
            if (isFlying)
            {
                ToggleFlight();
            }
            else if (IsHighEnough())
            {
                ToggleFlight();
            }
            else
            {
                // --- ON-SCREEN WARNING ---
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowFallWarning("Too close to the ground! Jump first.");
                }
            }
        }

        if (isFlying)
        {
            HandleFlightMovement();
        }
    }

    private bool IsHighEnough()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.distance >= minHeightToFly)
            {
                return true;
            }
        }
        return false;
    }

    private void ToggleFlight()
    {
        isFlying = !isFlying;

        if (fallDamage != null)
        {
            fallDamage.isFlying = isFlying;
        }

        if (isFlying)
        {
            fpController.Gravity = 0f; 
            characterController.Move(Vector3.zero);

            // --- ON-SCREEN NOTIFICATION ---
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowFallWarning("Take-off successful!");
            }
        }
        else
        {
            fpController.Gravity = -15.0f; 

            // --- ON-SCREEN NOTIFICATION ---
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowFallWarning("Landed smoothly.");
            }
        }
    }

    private void HandleFlightMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");     

        Vector3 moveDirection = (transform.forward * vertical) + (transform.right * horizontal);

        if (Input.GetKey(KeyCode.Space))
        {
            moveDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveDirection += Vector3.down;
        }

        characterController.Move(moveDirection.normalized * flySpeed * Time.deltaTime);
    }
}