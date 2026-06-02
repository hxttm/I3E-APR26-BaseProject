using System; // Import standard .NET system types (not strictly needed here but common in C# files)
using UnityEngine; // Import Unity-specific classes like MonoBehaviour, GameObject, Collider, and print

public class DoorScript : MonoBehaviour
{
    Animator myAnimator;

    bool isOpen = false;

    void Start()
    {
        myAnimator = GetComponentInParent<Animator>();
    }

    public void Interact()
    {
        if(isOpen)
        {
            myAnimator.SetTrigger("CloseDoor");
        }
        else
        {
            myAnimator.SetTrigger("OpenDoor");
        }
        isOpen = !isOpen;
    }
}