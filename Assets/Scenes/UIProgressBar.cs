using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    [Header("--- UI Elements ---")]
    [SerializeField] private Slider slider;

    /// <param name="currentValue">HP saat ini</param>
    /// <param name="maxValue">Max HP karakter/monster</param>
    public void UpdateValue(float currentValue, float maxValue)
    {
        if (slider == null) return;

        slider.maxValue = maxValue;

        slider.value = currentValue;
    }
}