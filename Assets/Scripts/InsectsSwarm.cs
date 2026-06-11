using UnityEngine;

public class InsectSwarm : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damagePerTick = 1;         
    public float damageInterval = 1.5f;   

    [Header("Movement (Patrol)")]
    public float patrolRadius = 5f;       // How far they wander AND the max distance they will chase you from home
    public float patrolSpeed = 1.5f;      
    public float spotReachedThreshold = 0.5f;

    [Header("Movement (Attack)")]
    public float attackSpeed = 4.5f;      
    public float attackRadius = 6f;       // How close you need to get to make them mad
    public float attackJitterSpeed = 15f; 
    public float attackJitterDistance = 1.2f; 

    [Header("Visuals Reference")]
    [SerializeField] private Transform visualGroup;

    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private float nextDamageTime = 0f;
    private PlayerHealth playerHealthScript = null;
    private Transform playerTransform = null;

    void Start()
    {
        startingPosition = transform.position;
        PickNewRandomTarget();
        FindPlayerReference();

        if (visualGroup == null && transform.childCount > 0)
        {
            visualGroup = transform.GetChild(0);
        }
    }

    private void FindPlayerReference()
    {
        GameObject playerObj = GameObject.Find("PlayerCapsule");
        if (playerObj == null) playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    void Update()
    {
        bool shouldAttack = false;

        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            
            // CRUCIAL CHECK: Are the insects getting too far away from their original spawn point?
            float distanceFromHome = Vector3.Distance(startingPosition, transform.position);

            // They only attack if you are close AND they haven't flown past their maximum patrol boundary
            if (distanceToPlayer <= attackRadius && distanceFromHome < patrolRadius)
            {
                shouldAttack = true;
            }
        }

        // State Machine
        if (shouldAttack)
        {
            AttackTargetPlayer();
        }
        else
        {
            PatrolAround();
        }

        // Deal continuous damage ONLY if the player is physically touching the trigger zone
        if (playerHealthScript != null && Time.time >= nextDamageTime)
        {
            playerHealthScript.TakeDamage(damagePerTick);
            nextDamageTime = Time.time + damageInterval;
        }
    }

    private void PatrolAround()
    {
        if (visualGroup != null)
        {
            visualGroup.localPosition = Vector3.MoveTowards(visualGroup.localPosition, Vector3.zero, patrolSpeed * Time.deltaTime);
        }

        // Move back toward the zone
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < spotReachedThreshold)
        {
            PickNewRandomTarget();
        }
    }

    private void AttackTargetPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, attackSpeed * Time.deltaTime);

        if (visualGroup != null)
        {
            Vector3 jitterOffset = new Vector3(
                Mathf.Sin(Time.time * attackJitterSpeed) * attackJitterDistance,
                Mathf.Cos(Time.time * (attackJitterSpeed * 0.8f)) * 0.5f,
                Mathf.Cos(Time.time * attackJitterSpeed) * attackJitterDistance
            );
            visualGroup.localPosition = Vector3.MoveTowards(visualGroup.localPosition, jitterOffset, attackSpeed * Time.deltaTime);
        }
    }

    private void PickNewRandomTarget()
    {
        Vector3 randomInsideSphere = Random.insideUnitSphere * patrolRadius;
        randomInsideSphere.y = Random.Range(-0.3f, 0.3f); 
        targetPosition = startingPosition + randomInsideSphere;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule" || other.gameObject.name == "Capsule")
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health == null) health = other.GetComponentInParent<PlayerHealth>();

            if (health != null)
            {
                playerHealthScript = health;
                nextDamageTime = Time.time; 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule" || other.gameObject.name == "Capsule")
        {
            playerHealthScript = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // The Yellow sphere in your scene view represents their strict home territory!
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Application.isPlaying ? startingPosition : transform.position, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}