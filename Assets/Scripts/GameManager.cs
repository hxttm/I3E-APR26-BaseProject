using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text crystalText;
    public int crystalCount = 0;

    void Awake()
    {
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

    void UpdateUI()
    {
        crystalText.text = "Crystal Count: " + crystalCount + "/10";
    }
}



public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text crystalText;

    public int crystalCount = 0;
    public int gemCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCrystal(int amount)
    {
        crystalCount += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        crystalText.text = "Crystal Count: " + crystalCount + "/10";
    }
}