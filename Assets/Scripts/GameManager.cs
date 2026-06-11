using UnityEngine;
using TMPro;
using UnityEngine.UI; // REQUIRED to control UI Image components!

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public TMP_Text crystalText;
    public Image wingsInventoryIcon; // Drag your 'WingsIcon' object here!

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

    // This is called automatically by RaycastWingPickup when you press Q!
    public void EquipWingsToInventory()
    {
        hasWingsInInventory = true;
        UpdateUI();
    }

    public void UpdateUI()
    {
        // 1. Update the Gem counter text out of 10
        if (crystalText != null)
        {
            crystalText.text = "Crystal Count: " + crystalCount + "/10";
        }

        // 2. Turn on the inventory icon if the player has the wings!
        if (wingsInventoryIcon != null)
        {
            wingsInventoryIcon.gameObject.SetActive(hasWingsInInventory);
        }
    }
}