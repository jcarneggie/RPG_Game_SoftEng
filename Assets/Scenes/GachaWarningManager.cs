using UnityEngine;

public class GachaWarningManager : MonoBehaviour
{
    [Header("--- UI Panels ---")]
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private GameObject gachaMainPanel;

    private const string GACHA_WARNED_KEY = "HasBeenWarnedGacha";

    void Start()
    {
        if (warningPanel != null) warningPanel.SetActive(false);
    }


    public void OnGachaButtonClick()
    {

        if (PlayerPrefs.GetInt(GACHA_WARNED_KEY, 0) == 0)
        {

            if (warningPanel != null) warningPanel.SetActive(true);
        }
        else
        {

            BukaMenuGachaAsli();
        }
    }


    public void ConfirmWarning()
    {

        PlayerPrefs.SetInt(GACHA_WARNED_KEY, 1);
        PlayerPrefs.Save();

        if (warningPanel != null) warningPanel.SetActive(false);
        BukaMenuGachaAsli();
    }

    private void BukaMenuGachaAsli()
    {
        if (gachaMainPanel != null) gachaMainPanel.SetActive(true);
        Debug.Log("[GACHA] Menu Gacha Terbuka Aman Cok!");
    }
}