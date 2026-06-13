using UnityEngine;

public class OrbFly : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 25f; 
    private Vector3 throwDirection;
    private bool isThrown = false;

    // Called automatically by PlayerHealth when you press T
    public void Launch(Vector3 direction)
    {
        throwDirection = direction.normalized;
        isThrown = true;
        
        // Destroys the visual orb copy after 4 seconds so it doesn't clutter your hierarchy
        Destroy(gameObject, 4f); 
    }

    void Update()
    {
        if (isThrown)
        {
            // Fly straight forward through 3D space
            transform.position += throwDirection * speed * Time.deltaTime;
        }
    }

    // Clean impact: If it passes through anything, it simply cleans itself up
    private void OnTriggerEnter(Collider other)
    {
        // If it touches the portal structure, destroy the visual orb clone instantly on impact
        if (other.name.Contains("Plane") || other.name.Contains("Portal"))
        {
            Destroy(gameObject);
        }
    }
}