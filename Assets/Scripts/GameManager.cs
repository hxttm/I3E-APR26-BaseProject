using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int crystalCount = 0;
    public int requiredCrystals = 10;

    public TMP_Text crystalText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void CollectCrystal()
    {
        crystalCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        crystalText.text = "Crystals: " + crystalCount + "/" + requiredCrystals;
    }
}