using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float interactionDistance = 3f; 
    public LayerMask interactableLayer;    

    [Header("UI Reference")]
    public GameObject popupPanel;
    public TMP_Text popupText;

    private Camera playerCamera;
    public static Interactable CurrentTarget { get; private set; }

    void Start()
    {
        playerCamera = Camera.main; 
    }

    void Update()
    {
        RaycastForInteractables();
    }

    void RaycastForInteractables()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                CurrentTarget = interactable;
                
                if (popupPanel != null && popupText != null)
                {
                    popupPanel.SetActive(true);
                    popupText.text = CurrentTarget.GetLookAtPrompt();
                }
                return; 
            }
        }

        ClearCurrentTarget();
    }

    void ClearCurrentTarget()
    {
        CurrentTarget = null;
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }
}