using System; // Import standard .NET system types (not strictly needed here but common in C# files)
using UnityEngine; // Import Unity-specific classes like MonoBehaviour, GameObject, Collider, and print
using TMPro; // Import TextMeshPro namespace for advanced text handling (not used in this script but often included in Unity projects for UI text)

public class PlayerScript : MonoBehaviour
{

    GameObject currentCollectible; // Store the collectible object the player is currently able to interact with

    int playerScore = 0; // Keep track of how many points the player has collected so far

    [SerializeField]
    int targetScore = 0; // The goal score required to complete a task, editable from the Unity Inspector

    [SerializeField]
    TextMeshProUGUI scoreText; // Reference to the UI text element that displays the player's score

    void Start()
    {
        scoreText.text = "Score: " + playerScore; // Initialize the score display to show the starting score of 0 when the game begins
    }

    void OnInteract() // Custom interaction method called when the player performs an interact action
    {
        if(currentCollectible != null) // Only collect something if the player is currently near a collectible
        {
            CollectibleScript collScript = currentCollectible.GetComponentInParent<CollectibleScript>(); // Find the collectible script on the collectible object or its parents
            if(collScript == null) // Check if the collectible script was found successfully
            {
                print("Error: No CollectibleScript found on " + currentCollectible.name); // Log an error in the Unity Console if the collectible is missing its data component
                return; // Exit the method early because we cannot safely collect the item without the script
            }
            else
            {
                playerScore += collScript.collectibleScore; // Add the collectible's score value to the player's total score
                print("Player has collected " + playerScore + " points"); // Print the updated score to the console for debugging or feedback
                scoreText.text = "Score: " + playerScore; // Update the on-screen score display to reflect the new score after collecting an item
                Destroy(currentCollectible); // Remove the collectible object from the scene after it has been collected
                currentCollectible = null; // Clear the reference so the player no longer has an active collectible selected
            }
        }
    }

    void OnTriggerEnter(Collider other) // Unity event called when another collider enters this GameObject's trigger collider
    {
        if(other.gameObject.tag == "Collectible") // Check if the object entering the trigger is tagged as a collectible
        {
            currentCollectible = other.GetComponentInParent<CollectibleScript>().gameObject; // Store the collectible GameObject so the player can interact with it later
        }

        if(other.gameObject.tag == "GoalArea" && playerScore >= targetScore) // Check if the player entered the goal area and has enough points
        {
            print("Player entered trigger zone with " + playerScore + " points"); // Print a success message when the player reaches the goal with enough score
        }
    }

    void OnTriggerExit(Collider other) // Unity event called when another collider leaves this GameObject's trigger collider
    {
        if(other.gameObject == currentCollectible) // If the collectible leaving the trigger is the one we were tracking
        {
            currentCollectible = null; // Clear the current collectible because it is no longer in range
        }
    }

}
