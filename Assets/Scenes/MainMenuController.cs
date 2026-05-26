using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ada untuk mengontrol perpindahan scene

public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// Fungsi khusus untuk langsung memindahkan pemain ke scene Floor1 Normal.
    /// Pasang fungsi ini di On Click() tombol Start lu.
    /// </summary>
    public void StartFloor1Normal()
    {
        Debug.Log("Memuat Scene: Floor1 Normal...");

        // Membuka scene permainan pertama secara instan
        SceneManager.LoadScene("Floor1 Normal");
    }
}