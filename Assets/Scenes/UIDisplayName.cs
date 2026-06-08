using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))] // Otomatis mastiin kalau objek ini punya komponen teks TMP
public class UIDisplayName : MonoBehaviour
{
    void Start()
    {
        TextMeshProUGUI nameText = GetComponent<TextMeshProUGUI>();

        if (nameText != null)
        {
            // Ambil nama dari memori device. Kalau kuncinya gak ketemu, kasih default "Player"
            string savedName = PlayerPrefs.GetString("PlayerName", "Player");

            // Timpa teks di UI dengan nama yang disimpan
            nameText.text = savedName;
        }
    }
}