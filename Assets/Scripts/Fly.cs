using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float gravity = -9.81f;
    
    [Header("Flying Ability")]
    public bool canFly = false; 
    public float flySpeed = 8f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        // Get the Character Controller component attached to this player
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Handle Basic Ground Walking (WASD or Arrow Keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate direction relative to where the player is facing
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * walkSpeed * Time.deltaTime);

        // 2. Handle Flying Logic vs Regular Gravity
        if (canFly && Input.GetKey(KeyCode.Space))
        {
            // If wings are unlocked and spacebar is held down -> FLY UP!
            velocity.y = flySpeed;
        }
        else
        {
            // Regular gravity drops you back down if you aren't flying or on the ground
            if (controller.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // Snaps player safely to the floor
            }
            else
            {
                velocity.y += gravity * Time.deltaTime; // Fall down naturally
            }
        }

        // Apply vertical movement (flying or falling)
        controller.Move(velocity * Time.deltaTime);
    }

    // This is the function called by the WingChest script!
    public void UnlockFlying()
    {
        canFly = true;
        Debug.Log("Wings Activated! Hold SPACEBAR to soar!");
    }
}