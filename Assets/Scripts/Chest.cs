using UnityEngine;

public class Chest : MonoBehaviour
{
    public int gemCost = 3;
    public int crystalReward = 5;

    private bool playerNearby = false;
    private bool opened = false;

    private Renderer chestRenderer;

    void Start()
    {
        chestRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (playerNearby && !opened && Input.GetKeyDown(KeyCode.O))
        {
            if (GameManager.Instance.gemCount >= gemCost)
            {
                GameManager.Instance.gemCount -= gemCost;
                GameManager.Instance.AddCrystal(crystalReward);

                opened = true;

                chestRenderer.material.color = Color.gray;
            }
            else
            {
                Debug.Log("Not enough gems!");
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