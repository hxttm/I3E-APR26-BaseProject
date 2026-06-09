using UnityEngine;

public class CrystalCollectables : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectCrystal();

            Destroy(gameObject);
        }
    }
}