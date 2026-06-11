using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class NotificationLogManager : MonoBehaviour
{
    public static NotificationLogManager Instance;

    [Header("--- UI Setup ---")]
    [Tooltip("Tarik Prefab UI Teks (TextMeshPro) lu ke sini")]
    [SerializeField] private GameObject logTextPrefab;

    [Tooltip("Tarik GameObek objek yang punya komponen Vertical Layout Group ke sini")]
    [SerializeField] private Transform logContainer;

    [Header("--- Settings ---")]
    [SerializeField] private float logDuration = 1f; 
    [SerializeField] private int maxVisibleLogs = 5; 

    private List<GameObject> activeLogs = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddLog(string message, Color textColor)
    {
        if (logTextPrefab == null || logContainer == null) return;

       
        if (activeLogs.Count >= maxVisibleLogs)
        {
            GameObject oldestLog = activeLogs[0];
            activeLogs.RemoveAt(0);
            Destroy(oldestLog);
        }

        
        GameObject newLog = Instantiate(logTextPrefab, logContainer);
        activeLogs.Add(newLog);

       
        TextMeshProUGUI tmpText = newLog.GetComponent<TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.text = message;
            tmpText.color = textColor;
        }

       
        StartCoroutine(DestroyLogRoutine(newLog));
    }

    private IEnumerator DestroyLogRoutine(GameObject logObj)
    {
        yield return new WaitForSeconds(logDuration);

        if (logObj != null)
        {
            activeLogs.Remove(logObj);
            Destroy(logObj);
        }
    }
}