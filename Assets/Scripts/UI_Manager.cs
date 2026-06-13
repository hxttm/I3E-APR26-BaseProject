using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI healthText; 
    public TextMeshProUGUI warningText; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (warningText != null)
        {
            warningText.text = "";
        }
    }

    public void UpdateHealthUI(int currentHealth)
    {
        // Handled inside PlayerHealth script
    }

    public void ShowFallWarning(string message)
    {
        if (warningText != null)
        {
            StopAllCoroutines(); 
            StartCoroutine(DisplayWarningDisplay(message));
        }
    }

    private IEnumerator DisplayWarningDisplay(string message)
    {
        // Forces the game view UI text element to display the message instantly
        warningText.text = message;
        
        yield return new WaitForSeconds(2f); // Stays on screen for 2 seconds
        
        warningText.text = ""; // Clears off screen
    }
}