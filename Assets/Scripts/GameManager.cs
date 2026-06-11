using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text crystalText;
    public int crystalCount = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCrystal()
    {
        crystalCount++;
        UpdateUI();
    }

    // ADD THIS: Helper method to safely deduct crystals
    public void RemoveCrystals(int amount)
    {
        crystalCount -= amount;
        if (crystalCount < 0) crystalCount = 0; // Prevent going into negative numbers
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (crystalText != null)
            crystalText.text = "Crystal Count: " + crystalCount + "/10";
    }
}