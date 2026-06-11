using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIContaminationManager : MonoBehaviour
{
    [Header("--- Player Reference ---")]
    [SerializeField] private PlayerStatus playerStatus;

    [Header("--- Contamination UI ---")]
    [Tooltip("Tarik TextMeshPro untuk angka persentase kontaminasi")]
    [SerializeField] private TextMeshProUGUI contaminationText;

    [Tooltip("Tarik Tombol Clean ke sini")]
    [SerializeField] private Button cleanButton;

    void Start()
    {
        if (playerStatus == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerStatus = playerObj.GetComponent<PlayerStatus>();
        }


        if (cleanButton != null)
        {
            cleanButton.onClick.AddListener(OnClickClean);
        }
    }

    void Update()
    {
        if (playerStatus == null) return;


        if (contaminationText != null)
        {
            contaminationText.text = "Contaminated:" + playerStatus.currentContamination.ToString("F1") + "%";
        }

 
        if (cleanButton != null)
        {
            cleanButton.gameObject.SetActive(playerStatus.currentContamination >= 50f);
        }
    }

    private void OnClickClean()
    {
        if (playerStatus != null)
        {
            playerStatus.CleanContamination();
        }
    }
}