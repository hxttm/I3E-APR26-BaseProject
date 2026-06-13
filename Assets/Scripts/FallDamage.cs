using UnityEngine;

public class FallDamage : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private CharacterController controller;
    
    private float highestYPoint;
    private bool wasInAir;

    [Header("Flight Settings")]
    public bool isFlying = false;

    [Header("Fall Settings")]
    public float damageHeightThreshold = 4f; 
    public float groundCheckDistance = 1.1f; 
    public LayerMask groundLayer; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerHealth = GetComponent<PlayerHealth>();
        
        if (groundLayer == 0)
        {
            groundLayer = ~0; 
        }
    }

    void Update()
    {
        // Checked automatically by your Fly script now!
        if (isFlying)
        {
            wasInAir = false;
            return; 
        }

        bool isTouchingGround = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (!isTouchingGround)
        {
            if (!wasInAir)
            {
                highestYPoint = transform.position.y;
                wasInAir = true;
            }
            else if (transform.position.y > highestYPoint)
            {
                highestYPoint = transform.position.y;
            }
        }
        else
        {
            if (wasInAir)
            {
                float fallDistance = highestYPoint - transform.position.y;

                if (fallDistance > damageHeightThreshold)
                {
                    if (playerHealth != null)
                    {
                        playerHealth.DamageFromFall(); 
                    }

                    if (UIManager.Instance != null)
                    {
                        string roundedDistance = fallDistance.ToString("G3");
                        UIManager.Instance.ShowFallWarning("u fell " + roundedDistance + "m! drop was too high");
                    }
                }
                wasInAir = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}