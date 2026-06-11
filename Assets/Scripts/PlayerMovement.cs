using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float gravity = -19.62f; 
    public float jumpHeight = 2f;    

    [Header("Flying Ability")]
    public bool canFly = false;      // Does the player own the wings yet?
    public bool isFlying = false;    // Is the player currently mid-flight?
    public float flySpeed = 8f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Handle Basic Movement (WASD / Arrow Keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * walkSpeed * Time.deltaTime);

        // 2. Check for the "F" key to toggle Flight Mode (Only works if wings are unlocked!)
        if (canFly && Input.GetKeyDown(KeyCode.F))
        {
            isFlying = !isFlying; // Toggles flight state on and off
            
            if (isFlying)
            {
                // Give a small initial upward boost when taking off
                velocity.y = flySpeed * 0.5f; 
            }
        }

        // 3. Physics & Gravity Logic based on current state
        if (isFlying)
        {
            // FLYING STATE: Move up/down using Spacebar and Left Shift, or hover cleanly
            if (Input.GetKey(KeyCode.Space))
            {
                velocity.y = flySpeed; // Fly upwards
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                velocity.y = -flySpeed; // Fly downwards
            }
            else
            {
                velocity.y = 0f; // Hover perfectly in mid-air when no keys are pressed
            }
        }
        else
        {
            // WALKING/GROUNDED STATE: Regular gravity rules apply
            if (controller.isGrounded)
            {
                if (velocity.y < 0)
                {
                    velocity.y = -2f; // Snap to the floor safely
                }

                // Normal Jump using Spacebar
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
            }
            else
            {
                // Fall naturally under gravity if we aren't in flight mode
                velocity.y += gravity * Time.deltaTime;
            }
        }

        // Apply vertical movement calculations
        controller.Move(velocity * Time.deltaTime);
    }

    // Called automatically by RaycastWingPickup when you press Q on the dropped wings!
    public void UnlockFlying()
    {
        canFly = true;
        Debug.Log("Wings Equipped! Press F to soar into flight, and F again to drop back down!");
    }
}