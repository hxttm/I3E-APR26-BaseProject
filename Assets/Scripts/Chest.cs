using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool playerNearby = false;
    private bool opened = false;

    public Renderer chestRenderer;

    [Header("Gem Settings")]
    public GameObject gemPrefab;
    public Transform spawnPoint;

    void Update()
    {
        if (playerNearby && !opened && Input.GetKeyDown(KeyCode.O))
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        opened = true;

        // 💰 give +5 crystals
        GameManager.Instance.AddCrystals(5);

        // 💎 spawn 3 gems
        SpawnGems();

        // 🎨 visual change
        if (chestRenderer != null)
        {
            chestRenderer.material.color = Color.gray;
        }
    }

    void SpawnGems()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-0.4f, 0.4f),
                0.5f,
                Random.Range(-0.4f, 0.4f)
            );

            GameObject gem = Instantiate(
                gemPrefab,
                spawnPoint.position + offset,
                Quaternion.identity
            );

            // optional: pop-up force
            Rigidbody rb = gem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}