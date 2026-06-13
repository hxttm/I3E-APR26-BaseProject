using UnityEngine;

public class OrbProjectile : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 15f;
    private Vector3 throwDirection;
    private bool isThrown = false;

    // This is called automatically by your PlayerHealth script when you press 'T'
    public void Launch(Vector3 direction)
    {
        throwDirection = direction.normalized;
        isThrown = true;
        
        // Destroys the orb after 5 seconds if you miss, so it doesn't float forever
        Destroy(gameObject, 5f); 
    }

    void Update()
    {
        if (isThrown)
        {
            // Moves the orb forward through the air over time
            transform.position += throwDirection * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Try to find the PortalSystem script directly on the object hit
        PortalSystem portal = other.GetComponent<PortalSystem>();
        
        // 2. If it wasn't on that specific part, check its parent object
        if (portal == null)
        {
            portal = other.GetComponentInParent<PortalSystem>();
        }

        // 3. If we successfully found the portal, turn it green!
        if (portal != null)
        {
            portal.ActivatePortalWithOrb();
            
            // Destroy the flying projectile copy so it disappears inside the portal
            Destroy(gameObject); 
        }
    }
}