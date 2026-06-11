using UnityEngine;
using System.Collections;

public class JemChest : MonoBehaviour
{
    [Header("Chest Parts")]
    public Transform chestLid;
    public Transform spawnPoint;

    [Header("Spawn Settings")]
    public GameObject gemPrefab;

    [Header("Canvas UI Reference")]
    public GameObject canvasJemsObject; // Drag your 'CanvasJems' or 'OpenJems' text object here!

    private bool playerIsClose = false;
    private bool opened = false;
    private bool isOpening = false;

    private float openAngle = -90f;
    private float openSpeed = 5f;
    private Quaternion openRotation;

    void Start()
    {
        if (chestLid != null)
            openRotation = Quaternion.Euler(openAngle, 0f, 0f);

        // Hide the UI Canvas automatically when the game starts
        if (canvasJemsObject != null)
            canvasJemsObject.SetActive(false);
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

        if (playerIsClose && !opened && !isOpening)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                TryOpenChest();
            }
        }
    }

    private void TryOpenChest()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.crystalCount < 3)
        {
            Debug.Log("Not enough jems!");
            return;
        }

        GameManager.Instance.RemoveCrystals(3);
        
        opened = true;
        isOpening = true;

        // Hide the canvas UI permanently once the chest opens
        if (canvasJemsObject != null)
            canvasJemsObject.SetActive(false);

        StartCoroutine(SpawnAndRewardGems());
    }

    IEnumerator SpawnAndRewardGems()
    {
        for (int i = 0; i < 5; i++)
        {
            if (gemPrefab != null && spawnPoint != null)
            {
                Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), 0.1f, Random.Range(-0.2f, 0.2f));
                GameObject spawnedGem = Instantiate(gemPrefab, spawnPoint.position + offset, Quaternion.identity);
                
                Rigidbody rb = spawnedGem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(new Vector3(Random.Range(-1f, 1f), 4f, Random.Range(-1f, 1f)), ForceMode.Impulse);
                }
            }

            GameManager.Instance.AddCrystal();
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule")
        {
            playerIsClose = true;
            if (!opened && canvasJemsObject != null)
            {
                canvasJemsObject.SetActive(true); // Pop up the canvas UI!
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule")
        {
            playerIsClose = false;
            if (canvasJemsObject != null)
            {
                canvasJemsObject.SetActive(false); // Hide the canvas UI!
            }
        }
    }
}