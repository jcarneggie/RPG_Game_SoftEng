using UnityEngine;

public class BossMonster : MonoBehaviour
{
    [Header("--- Monster Stats ---")]
    public string monsterName = "Virus King Boss";
    public float maxHp = 500f;
    public float currentHp;
    public float attack = 150f;
    public float defense = 50f;
    public float accuracy = 100f;
    public float evasion = 20f;
    public float attackSpeed = 1f;

    [Header("--- Rewards Drop Prefabs ---")]
    public GameObject coinPrefab;
    public GameObject xpPrefab;
    public GameObject diamondPrefab; // SAKTI: Prefab Diamond khusus dropan Bos

    [Header("--- UI References (Sistem Baru) ---")]
    [Tooltip("Tarik objek Slider HP Monster ke sini")]
    [SerializeField] private UIProgressBar monsterHpBar;

    [Header("--- Hover Animation ---")]
    [SerializeField] private float floatSpeed = 3f;
    [SerializeField] private float floatHeight = 0.2f;

    private Vector3 startPos;
    private Vector3 initialHierarchyPos; // SAKTI: Untuk mencatat koordinat mutlak pas ditaro di Hierarchy awal game
    private bool isMyTurnToFight = false;

    void Start()
    {
        currentHp = maxHp;
        startPos = transform.position;
        initialHierarchyPos = transform.position; // Kunci posisi murni saat fajar pertama game dimulai

        // Set nilai awal bar darah saat monster muncul di layar
        if (monsterHpBar != null) monsterHpBar.UpdateValue(currentHp, maxHp);
    }

    void Update()
    {
        // SAKTI: Mengacu ke BossGameplayManager biar Bos tetep jalan kiri bareng map nyamperin player
        if (BossGameplayManager.Instance != null && BossGameplayManager.Instance.IsMapMoving)
        {
            transform.Translate(Vector3.left * BossGameplayManager.Instance.MapSpeed * Time.deltaTime, Space.World);
            startPos.x -= BossGameplayManager.Instance.MapSpeed * Time.deltaTime;
        }

        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void StartFighting()
    {
        isMyTurnToFight = true;
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        // UPDATE VISUAL BAR DARAH MONSTER TIAP DIKETOK PLAYER
        if (monsterHpBar != null) monsterHpBar.UpdateValue(currentHp, maxHp);

        if (currentHp <= 0) Die();
    }

    private void Die()
    {
        // SAKTI: Drop Koin, XP, dan Diamond secara instan pas Bos mampus
        if (coinPrefab != null) Instantiate(coinPrefab, transform.position + Vector3.up, Quaternion.identity);
        if (xpPrefab != null) Instantiate(xpPrefab, transform.position + Vector3.down, Quaternion.identity);
        if (diamondPrefab != null) Instantiate(diamondPrefab, transform.position + Vector3.right, Quaternion.identity);

        // Lapor ke manager khusus stage bos bahwa bos telah tewas
        if (BossGameplayManager.Instance != null)
        {
            BossGameplayManager.Instance.OnBossDefeated();
        }

        // JANGAN DI-DESTROY, Cukup matikan biar data drop dan settingan lu selamat
        gameObject.SetActive(false);
    }

    public void ResetMonster()
    {
        transform.position = initialHierarchyPos; // Teleport balik ke koordinat tanah aslinya di Hierarchy
        startPos = initialHierarchyPos;           // Reset acuan kalkulasi translate jalan kirinya
        currentHp = maxHp;                        // Darah penuh lagi
        isMyTurnToFight = false;

        if (monsterHpBar != null) monsterHpBar.UpdateValue(currentHp, maxHp); // Bar darah penuh lagi
        gameObject.SetActive(true);               // Munculkan kembali fisiknya di layar
    }
}