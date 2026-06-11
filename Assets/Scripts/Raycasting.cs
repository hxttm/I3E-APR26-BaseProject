using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Every interactable object will have its own version of these functions
    public abstract string GetLookAtPrompt(); // What text pops up?
    public abstract void OnInteract();       // What happens when you press 'E' or 'O'?
}