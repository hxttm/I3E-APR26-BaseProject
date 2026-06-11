using TMPro;
using UnityEngine;
using UnityEngine.UI; // Required for using UI Images

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public TMP_Text crystalText;
    public Image wingsInventoryIcon; // Drag your UI Wing Image here!

    [Header("Player Stats")]
    public int crystalCount = 0;
    public bool hasWingsInInventory = false;

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

    public void RemoveCrystals(int amount)
    {
        crystalCount -= amount;
        if (crystalCount < 0) crystalCount = 0;
        UpdateUI();
    }

    // Call this function when the player successfully picks up the wings
    public void EquipWingsToInventory()
    {
        hasWingsInInventory = true;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (crystalText != null)
            crystalText.text = "Crystal Count: " + crystalCount + "/10";

        // If we have the wings, show the icon. Otherwise, keep it hidden/faded!
        if (wingsInventoryIcon != null)
        {
            wingsInventoryIcon.gameObject.SetActive(hasWingsInInventory);
        }
    }
}