using UnityEngine;
using TMPro;
using System.Collections;

public class Chest : MonoBehaviour
{
    [Header("State")]
    private bool playerNearby;
    private bool opened;
    private bool isOpening;

    [Header("Chest Parts")]
    public Transform chestLid;

    [Header("Spawn")]
    public Transform spawnPoint;
    public GameObject gemPrefab;

    [Header("UI")]
    public GameObject popupPanel;
    public TMP_Text popupText;

    [Header("Cost")]
    public int cost = 3;

    [Header("Lid Animation")]
    public float openAngle = -90f;
    public float openSpeed = 5f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        if (chestLid != null)
        {
            closedRotation = chestLid.localRotation;
            openRotation = Quaternion.Euler(openAngle, 0f, 0f);
        }
    }

    void Update()
    {
        // 1. Handle Key Input (Only if player is nearby and chest isn't already opened/opening)
        if (playerNearby && !opened && !isOpening)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                TryOpenChest();
            }
        }

        // 2. Handle Lid Animation (Completely separated so it runs smoothly)
        if (isOpening && chestLid != null)
        {
            chestLid.localRotation = Quaternion.Slerp(
                chestLid.localRotation,
                openRotation,
                Time.deltaTime * openSpeed
            );

            if (Quaternion.Angle(chestLid.localRotation, openRotation) < 1f)
            {
                chestLid.localRotation = openRotation;
                isOpening = false; // Animation finished!
            }
        }
    }

    void TryOpenChest()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance missing from the scene!");
            return;
        }

        // Check if player has enough crystals
        if (GameManager.Instance.crystalCount < cost)
        {
            if (popupText != null) 
            {
                popupText.text = "Not enough gems!";
                StopAllCoroutines(); 
                StartCoroutine(ResetTextAfterDelay());
            }
            return;
        }

        // Deduct gems and trigger opening state
        GameManager.Instance.RemoveCrystals(cost);
        OpenChest();
    }

    IEnumerator ResetTextAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        if (playerNearby && !opened && popupText != null)
        {
            popupText.text = $"Press O to open (-{cost} gems)";
        }
    }

    void OpenChest()
    {
        opened = true;
        isOpening = true;

        if (popupPanel != null)
            popupPanel.SetActive(false);

        StartCoroutine(SpawnGemsCoroutine());
    }

    IEnumerator SpawnGemsCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-0.3f, 0.3f),
                0.2f,
                Random.Range(-0.3f, 0.3f)
            );

            GameObject gem = Instantiate(gemPrefab, spawnPoint.position + offset, Quaternion.identity);

            Rigidbody rb = gem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(new Vector3(Random.Range(-1f, 1f), 4f, Random.Range(-1f, 1f)), ForceMode.Impulse);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddCrystal();
            }

            yield return new WaitForSeconds(0.15f); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (opened) return;

        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            if (popupPanel != null)
            {
                popupPanel.SetActive(true);
                popupText.text = $"Press O to open (-{cost} gems)";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            StopAllCoroutines(); 

            if (popupPanel != null)
                popupPanel.SetActive(false);
        }
    }
}