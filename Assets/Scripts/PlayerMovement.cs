using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float gravity = -19.62f; 
    public float jumpHeight = 2f;    

    [Header("Flying Ability")]
    public bool canFly = false;      
    public bool isFlying = false;    
    public float flySpeed = 8f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * walkSpeed * Time.deltaTime);

        if (canFly && Input.GetKeyDown(KeyCode.F))
        {
            isFlying = !isFlying; 
            
            if (isFlying)
            {
                velocity.y = flySpeed * 0.5f; 
            }
        }

        if (isFlying)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                velocity.y = flySpeed; 
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                velocity.y = -flySpeed; 
            }
            else
            {
                velocity.y = 0f; 
            }
        }
        else
        {
            if (controller.isGrounded)
            {
                if (velocity.y < 0)
                {
                    velocity.y = -2f; 
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void UnlockFlying()
    {
        canFly = true;
        Debug.Log("Wings Equipped! Press F to soar into flight, and F again to drop back down!");
    }
}