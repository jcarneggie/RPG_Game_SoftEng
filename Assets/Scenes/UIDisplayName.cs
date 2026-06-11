using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))] 
public class UIDisplayName : MonoBehaviour
{
    void Start()
    {
        TextMeshProUGUI nameText = GetComponent<TextMeshProUGUI>();

        if (nameText != null)
        {

            string savedName = PlayerPrefs.GetString("PlayerName", "Player");


            nameText.text = savedName;
        }
    }
}