using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class UINameInput : MonoBehaviour
{
    [Header("--- UI References ---")]
    [Tooltip("Tarik objek InputField (TMP) tempat player ngetik nama ke sini")]
    [SerializeField] private TMP_InputField nameInputField;

    [Header("--- Scene Settings ---")]
    [Tooltip("Ketik nama scene floor 1 lu yang SAMA PERSIS huruf besar kecilnya!")]
    [SerializeField] private string nextSceneName = "floor1normal";
    public void SaveNameAndStartGame()
    {
        if (nameInputField == null)
        {
            Debug.LogError("[NAME INPUT] Objek InputField belum dicolok di Inspector, cok!");
            return;
        }

        string inputName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(inputName))
        {
            inputName = "Player";
            Debug.Log("[NAME INPUT] Kolom kosong, otomatis diset menjadi: Player");
        }

        PlayerPrefs.SetString("PlayerName", inputName);
        PlayerPrefs.Save();


        SceneManager.LoadScene(nextSceneName);
    }
}