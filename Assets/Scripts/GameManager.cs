using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int crystalCount = 0;

    public TMP_Text crystalText;

    private void Awake()
    {
        Instance = this;
    }

    public void CollectCrystal()
    {
        crystalCount++;

        Debug.Log("Crystal Count = " + crystalCount);

        UpdateUI();
    }

void UpdateUI()
{
    if (crystalText == null)
    {
        Debug.LogError("UI TEXT NOT ASSIGNED!");
        return;
    }

    crystalText.text = "Crystals: " + crystalCount + "/10";
}