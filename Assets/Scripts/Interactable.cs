using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract string GetLookAtPrompt(); 
    public abstract void OnInteract();       
}