using UnityEngine;

public class PortalPlane : MonoBehaviour
{
    [Header("Portal Visuals")]
    public Material greenPortalMaterial; 

    private Renderer myRenderer;
    private bool isActivated = false;

    void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    public void TurnPlaneGreen()
    {
        isActivated = true;

        if (myRenderer != null && greenPortalMaterial != null)
        {
            myRenderer.material = greenPortalMaterial;
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowFallWarning("Portal activated! Step through to exit.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule") && isActivated)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowFallWarning("Game Completed!");
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
}