using UnityEngine;
using UnityEngine.UI;

public class GameSettingsManager : MonoBehaviour
{
    [Header("--- UI References ---")]
    [Tooltip("Tarik objek Slider volume lu ke sini")]
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {

        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
            volumeSlider.value = savedVolume;
            SetVolume(savedVolume);


            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float volumeValue)
    {

        AudioListener.volume = volumeValue;

        PlayerPrefs.SetFloat("GameVolume", volumeValue);
        PlayerPrefs.Save();
    }

    public void ExitGame()
    {

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame();
            Debug.Log("[EXIT] Data diamankan sebelum keluar!");
        }

        Debug.Log("[EXIT] Menutup game...");


        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}