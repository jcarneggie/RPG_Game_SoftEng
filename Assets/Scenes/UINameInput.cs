using UnityEngine;
using TMPro; // Wajib karena kita pakai TextMeshPro Input Field
using UnityEngine.SceneManagement; // Wajib untuk fungsi perpindahan scene

public class UINameInput : MonoBehaviour
{
    [Header("--- UI References ---")]
    [Tooltip("Tarik objek InputField (TMP) tempat player ngetik nama ke sini")]
    [SerializeField] private TMP_InputField nameInputField;

    [Header("--- Scene Settings ---")]
    [Tooltip("Ketik nama scene floor 1 lu yang SAMA PERSIS huruf besar kecilnya!")]
    [SerializeField] private string nextSceneName = "floor1normal";

    // Fungsi ini bakal dipanggil pas tombol PLAY/CONFIRM diklik
    public void SaveNameAndStartGame()
    {
        if (nameInputField == null)
        {
            Debug.LogError("[NAME INPUT] Objek InputField belum dicolok di Inspector, cok!");
            return;
        }

        // Ambil teks inputan player dan bersihkan spasi kosong di awal/akhir
        string inputName = nameInputField.text.Trim();

        // Antisipasi kalau player dongo langsung klik play tanpa ngetik nama
        if (string.IsNullOrEmpty(inputName))
        {
            inputName = "Player"; // Kasih nama default
            Debug.Log("[NAME INPUT] Kolom kosong, otomatis diset menjadi: Player");
        }

        // Kunci nama ke dalam brankas PlayerPrefs lokal device
        PlayerPrefs.SetString("PlayerName", inputName);
        PlayerPrefs.Save();

        // Pindah ke scene game utama
        SceneManager.LoadScene(nextSceneName);
    }
}