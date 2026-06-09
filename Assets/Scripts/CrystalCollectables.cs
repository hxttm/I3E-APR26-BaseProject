using UnityEngine;

public class CrystalCollectables : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Crystal Collected!");

            Destroy(gameObject);
        }
    }
}