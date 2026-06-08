using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BossGameplayManager : MonoBehaviour
{
    public static BossGameplayManager Instance;

    [Header("--- References ---")]
    [Tooltip("Tarik objek Player lu ke sini")]
    [SerializeField] private Transform playerTransform;
    [Tooltip("Tarik objek Player lu yang punya komponen Animator ke sini")]
    [SerializeField] private Animator playerAnimator;

    [Header("--- Backgrounds to Stop ---")]
    [Tooltip("Tarik SEMUA objek background/floor yang bergerak ke sini")]
    [SerializeField] private BackgroundParallax[] movingBackgrounds;

    [Header("--- Settings ---")]
    public float MapSpeed = 3.5f;
    public bool IsMapMoving { get; private set; } = true;

    [Tooltip("Jarak minimal antara Player dan Boss untuk memulai tarung")]
    [SerializeField] private float attackDistanceThreshold = 1.2f;

    [Header("--- Boss Target Slot ---")]
    [Tooltip("Tarik objek Boss lu yang punya script BossMonster ke sini")]
    [SerializeField] private BossMonster targetBoss;

    [Header("--- UI Panels ---")]
    [Tooltip("Tarik Panel kemenangan 'Boss Defeated' ke sini")]
    [SerializeField] private GameObject bossDefeatedPanel;
    [Tooltip("Tarik Panel layar gelap/merah lu ke sini")]
    [SerializeField] private GameObject deathPanel;

    [Header("--- Auto Scene Transitions ---")]
    [Tooltip("Nama scene kalau MATI (mundur)")]
    [SerializeField] private string previousFloorScene = "floor1normal";
    [Tooltip("Nama scene kalau MENANG (maju)")]
    [SerializeField] private string nextFloorScene = "floor2normal";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        SetMapMovement(true);
        if (bossDefeatedPanel != null) bossDefeatedPanel.SetActive(false);
        if (deathPanel != null) deathPanel.SetActive(false);
    }

    void Update()
    {
        if (targetBoss != null && targetBoss.gameObject.activeSelf && IsMapMoving)
        {
            float realDistance = Vector3.Distance(playerTransform.position, targetBoss.transform.position);

            if (realDistance <= attackDistanceThreshold)
            {
                TriggerBattleState();
            }
        }
    }

    private void TriggerBattleState()
    {
        SetMapMovement(false);
        if (playerAnimator != null) playerAnimator.SetBool("isAttacking", true);

        if (targetBoss != null)
        {
            targetBoss.StartFighting();
            PlayerStatus playerStatus = playerTransform.GetComponent<PlayerStatus>();

            if (BossBattleSystem.Instance != null && playerStatus != null)
            {
                BossBattleSystem.Instance.StartBattle(playerStatus, targetBoss);
            }
        }
    }

    public void OnBossDefeated()
    {
        StartCoroutine(VictorySequenceRoutine());
    }

    private IEnumerator VictorySequenceRoutine()
    {
        if (BossBattleSystem.Instance != null) BossBattleSystem.Instance.EndBattle();

        SetMapMovement(false);
        if (playerAnimator != null) playerAnimator.SetBool("isAttacking", false);

        if (bossDefeatedPanel != null) bossDefeatedPanel.SetActive(true);
        Debug.Log("[BOSS STAGE] Kemenangan mutlak! Tahan 3 detik nunggu loot kesedot...");

        // SAKTI: Auto save sebelum pindah biar duit gak hilang
        if (SaveManager.Instance != null) SaveManager.Instance.SaveGame();

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(nextFloorScene);
    }

    public void OnPlayerDeath()
    {
        StartCoroutine(DeathSequenceRoutine());
    }

    private IEnumerator DeathSequenceRoutine()
    {
        if (BossBattleSystem.Instance != null) BossBattleSystem.Instance.EndBattle();

        SetMapMovement(false);
        if (playerAnimator != null) playerAnimator.SetBool("isAttacking", false);

        // SAKTI: KUNCI DATA TERKINI BIAR TIDAK HILANG / REVERT SAAT PINDAH SCENE!
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame();
            Debug.Log("[BOSS DEATH] Data koin & stat diamankan sebelum ditendang balik.");
        }

        if (deathPanel != null) deathPanel.SetActive(true);
        Debug.Log("[BOSS STAGE] Player mampus dihajar bos! Tahan 2 detik...");

        yield return new WaitForSeconds(2f);
        if (deathPanel != null) deathPanel.SetActive(false);

        SceneManager.LoadScene(previousFloorScene);
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