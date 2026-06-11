using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("--- Daftarkan Semua Panel Menu Lu Di Sini ---")]
    [Tooltip("Masukkan Panel_Status, Panel_Equipment, Panel_Skill, dll")]
    [SerializeField] private GameObject[] allPanels;

    [Header("--- Universal Blocker ---")]
    [Tooltip("Tarik objek BG_Blocker raksasa lu ke sini")]
    [SerializeField] private GameObject bgBlocker;


    public void OpenExclusivePanel(GameObject panelToOpen)
    {
        bool anyPanelOpened = false;

        foreach (GameObject panel in allPanels)
        {
            if (panel != null) panel.SetActive(false);
        }

        if (panelToOpen != null)
        {
            panelToOpen.SetActive(true);
            anyPanelOpened = true; 
        }

        if (bgBlocker != null)
        {
            bgBlocker.SetActive(anyPanelOpened);
        }
    }

    public void CloseAllPanels()
    {
        foreach (GameObject panel in allPanels)
        {
            if (panel != null) panel.SetActive(false);
        }

        if (bgBlocker != null) bgBlocker.SetActive(false);
    }
}