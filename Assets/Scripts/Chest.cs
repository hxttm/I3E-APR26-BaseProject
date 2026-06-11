using UnityEngine;
using System.Collections;

public class RaycastChest : Interactable
{
    [Header("Chest Parts")]
    public Transform chestLid;
    public Transform spawnPoint;

    [Header("Spawn Settings")]
    public bool isWingChest = false; 
    public GameObject gemPrefab;
    public GameObject wingsPrefab; 

    [Header("Cost Settings")]
    public int cost = 3; 
    
    private bool opened = false;
    private bool isOpening = false;

    [Header("Lid Animation")]
    public float openAngle = -90f;
    public float openSpeed = 5f;
    private Quaternion openRotation;

    void Start()
    {
        if (chestLid != null)
            openRotation = Quaternion.Euler(openAngle, 0f, 0f);
    }

    void Update()
    {
        if (isOpening && chestLid != null)
        {
            chestLid.localRotation = Quaternion.Slerp(chestLid.localRotation, openRotation, Time.deltaTime * openSpeed);
            if (Quaternion.Angle(chestLid.localRotation, openRotation) < 1f)
            {
                chestLid.localRotation = openRotation;
                isOpening = false;
            }
        }

        // Handle the actual button presses based on what chest this is
        if (!opened && !isOpening)
        {
            // If the player is looking at this chest (handled by PlayerInteraction)
            if (PlayerInteraction.CurrentTarget == this) 
            {
                if (isWingChest && Input.GetKeyDown(KeyCode.O))
                {
                    OnInteract();
                }
                else if (!isWingChest && Input.GetKeyDown(KeyCode.E))
                {
                    OnInteract();
                }
            }
        }
    }

    public override string GetLookAtPrompt()
    {
        if (opened) return "";
        
        // Displays your custom key prompts!
        if (isWingChest)
        {
            return $"Press O to unlock WINGS (-{cost} gems)";
        }
        else
        {
            return $"Press E to open (-{cost} gems)";
        }
    }

    public override void OnInteract()
    {
        if (opened || isOpening || GameManager.Instance == null) return;

        if (GameManager.Instance.crystalCount < cost)
        {
            Debug.Log("Not enough gems!");
            return;
        }

        GameManager.Instance.RemoveCrystals(cost);
        opened = true;
        isOpening = true;

        if (isWingChest)
        {
            SpawnPhysicalWings();
        }
        else
        {
            StartCoroutine(SpawnGemsCoroutine());
        }
    }

    void SpawnPhysicalWings()
    {
        if (wingsPrefab != null && spawnPoint != null)
        {
            GameObject spawnedWings = Instantiate(wingsPrefab, spawnPoint.position + Vector3.up * 0.5f, Quaternion.identity);
            Rigidbody rb = spawnedWings.GetComponent<Rigidbody>();
            if (rb != null) rb.AddForce(Vector3.up * 4f, ForceMode.Impulse);
        }
    }

    IEnumerator SpawnGemsCoroutine()
    {
        for (int i = 0; i < 8; i++) 
        {
            Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), 0.2f, Random.Range(-0.3f, 0.3f));
            GameObject gem = Instantiate(gemPrefab, spawnPoint.position + offset, Quaternion.identity);
            Rigidbody rb = gem.GetComponent<Rigidbody>();
            if (rb != null) rb.AddForce(new Vector3(Random.Range(-1f, 1f), 4f, Random.Range(-1f, 1f)), ForceMode.Impulse);

            GameManager.Instance.AddCrystal();
            yield return new WaitForSeconds(0.15f); 
        }
    }
}