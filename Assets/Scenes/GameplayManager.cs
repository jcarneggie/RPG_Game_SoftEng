using UnityEngine;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine.SceneManagement; 

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    [Header("--- References ---")]
    [Tooltip("Tarik objek Player lu ke sini")]
    [SerializeField] private Transform playerTransform;
    [Tooltip("Tarik objek Player lu yang punya komponen Animator ke sini")]
    [SerializeField] private Animator playerAnimator;

    [Header("--- Audio Setup ---")]
    [Tooltip("Tarik GameplayManager lu sendiri ke sini buat audionya")]
    [SerializeField] private AudioSource attackAudioSource;

    [Header("--- Backgrounds to Stop (Bisa Banyak Objek) ---")]
    [Tooltip("Tarik SEMUA objek background/floor yang bergerak ke sini (Size: 2)")]
    [SerializeField] private BackgroundParallax[] movingBackgrounds;

    [Header("--- Settings ---")]
    public float MapSpeed = 3.5f;
    public bool IsMapMoving { get; private set; } = true;

    [Tooltip("Jarak minimal antara Player dan Monster untuk memulai tarung (Cocok di angka 1 sampai 1.5)")]
    [SerializeField] private float attackDistanceThreshold = 1.2f;

    [Header("--- Queue List (Isi 5 Monster Berurutan) ---")]
    [SerializeField] private List<Monster> monsterQueue = new List<Monster>();

    private List<Monster> masterMonsterList = new List<Monster>();

    [Header("--- Boss System ---")]
    public int currentLoopCount = 0;
    public int loopsRequiredForBoss = 5; 

    [Tooltip("Tarik objek Tombol Boss lu ke sini")]
    [SerializeField] private GameObject buttonBoss;

    [Tooltip("Ketik nama scene bos lu. Huruf besar/kecil WAJIB SAMA!")]
    [SerializeField] private string bossSceneName = "scene_boss";

    [Header("--- Death Screen ---")]
    [Tooltip("Tarik Panel layar gelap/merah lu ke sini")]
    [SerializeField] private GameObject deathPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        masterMonsterList = new List<Monster>(monsterQueue);
        SetMapMovement(true);

        if (buttonBoss != null) buttonBoss.SetActive(false);
        if (deathPanel != null) deathPanel.SetActive(false);
    }

    void Update()
    {
        if (monsterQueue.Count > 0 && IsMapMoving)
        {
            if (monsterQueue[0] != null && playerTransform != null && monsterQueue[0].gameObject.activeSelf)
            {
                float realDistance = Vector3.Distance(playerTransform.position, monsterQueue[0].transform.position);

                if (realDistance <= attackDistanceThreshold)
                {
                    TriggerBattleState();
                }
            }
        }
    }

    private void TriggerBattleState()
    {
        SetMapMovement(false); 
        if (playerAnimator != null) playerAnimator.SetBool("isAttacking", true);
        if (attackAudioSource != null && !attackAudioSource.isPlaying)
        {
            attackAudioSource.Play();
        }
        if (monsterQueue.Count > 0 && monsterQueue[0] != null)
        {
            monsterQueue[0].StartFighting();

            PlayerStatus playerStatus = playerTransform.GetComponent<PlayerStatus>();

            if (BattleSystem.Instance != null && playerStatus != null)
            {
                BattleSystem.Instance.StartBattle(playerStatus, monsterQueue[0]);
            }
        }

    }

    public void OnMonsterDefeated()
    {
        if (BattleSystem.Instance != null) BattleSystem.Instance.EndBattle();
        if (attackAudioSource != null) attackAudioSource.Stop();
        PlayerStatus playerStatus = playerTransform.GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.currentContamination = Mathf.Min(100f, playerStatus.currentContamination + 0.1f);
        }

        if (monsterQueue.Count > 0) monsterQueue.RemoveAt(0);

        if (monsterQueue.Count > 0)
        {
            SetMapMovement(true); 
            if (playerAnimator != null) playerAnimator.SetBool("isAttacking", false);
        }
        else
        {
            Debug.Log("Semua kloter monster mampus! Jalan terus nunggu map looping.");
            SetMapMovement(true);
            if (playerAnimator != null) playerAnimator.SetBool("isAttacking", false);
        }
    }

    public void OnMapLoopTriggered()
    {
        currentLoopCount++;
        Debug.Log($"[LOOPING] Map nge-reset! Ini loop ke-{currentLoopCount}");

        if (currentLoopCount >= loopsRequiredForBoss)
        {
            if (buttonBoss != null) buttonBoss.SetActive(true);
            Debug.Log("[BOSS] Syarat terpenuhi! Tombol Boss Terbuka!");
        }

        Debug.Log("[LOOPING] Map nge-reset! Bangkitkan kembali 5 monster di posisi semula!");

        monsterQueue.Clear();

        foreach (Monster m in masterMonsterList)
        {
            if (m != null)
            {
                m.ResetMonster(); 
                monsterQueue.Add(m); 
            }
        }

        SetMapMovement(true);
    }

    public void OnPlayerDeath()
    {
        StartCoroutine(DeathSequenceRoutine());
    }

    private IEnumerator DeathSequenceRoutine()
    {
        if (BattleSystem.Instance != null) BattleSystem.Instance.EndBattle();
        SetMapMovement(false);
        if (playerAnimator != null) playerAnimator.SetBool("isAttacking", false);


        if (SaveManager.Instance != null) SaveManager.Instance.SaveGame();

        if (deathPanel != null) deathPanel.SetActive(true);

        currentLoopCount = 0;
        if (buttonBoss != null) buttonBoss.SetActive(false);
        Debug.Log("[DEATH] Player Mati! Layar gelap, progres loop hangus cok!");

        yield return new WaitForSeconds(2f);

        if (deathPanel != null) deathPanel.SetActive(false);

        foreach (BackgroundParallax bp in movingBackgrounds)
        {
            if (bp != null) bp.ResetPosition();
        }

        OnMapLoopTriggered();
    }

    public void LoadBossScene()
    {
        if (SaveManager.Instance != null) SaveManager.Instance.SaveGame();
        SceneManager.LoadScene(bossSceneName);
    }

    private void SetMapMovement(bool status)
    {
        IsMapMoving = status;
        foreach (BackgroundParallax bp in movingBackgrounds)
        {
            if (bp != null) bp.SetMovement(status);
        }
    }
}