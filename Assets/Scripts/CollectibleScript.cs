using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    public int collectibleScore = 0; // Store the score value of this collectible, editable from the Unity Inspector. (this allows different collectibles to be worth different amounts of points)

    AudioSource collectibleAudio;

    void Start()
    {
        collectibleAudio = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject so we can play a sound when collected
    }

}
