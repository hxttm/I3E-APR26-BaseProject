using UnityEngine;

public class CrystalCollectables : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player hit crystal");

            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager is NULL");
            }
            else
            {
                GameManager.Instance.CollectCrystal();
            }

            Destroy(gameObject);
        }
    }
}