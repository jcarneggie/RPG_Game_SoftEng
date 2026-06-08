using UnityEngine;

public class BossBattleSystem : MonoBehaviour
{
    public static BossBattleSystem Instance;

    [Header("--- Battle Targets ---")]
    [SerializeField] private PlayerStatus activePlayer;
    [SerializeField] private BossMonster activeBoss; // SAKTI: Targetnya sekarang khusus BossMonster

    private bool isBattleActive = false;
    private float playerAttackTimer = 0f;
    private float bossAttackTimer = 0f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (!isBattleActive || activePlayer == null || activeBoss == null) return;

        // 1. Timer Serangan Otomatis Player
        playerAttackTimer += Time.deltaTime;
        if (playerAttackTimer >= (1f / activePlayer.FinalAttackSpeed))
        {
            ExecutePlayerAttack();
            playerAttackTimer = 0f;
        }

        // 2. Timer Serangan Otomatis Boss
        bossAttackTimer += Time.deltaTime;
        if (bossAttackTimer >= (1f / activeBoss.attackSpeed))
        {
            ExecuteBossAttack();
            bossAttackTimer = 0f;
        }
    }

    public void StartBattle(PlayerStatus player, BossMonster boss)
    {
        activePlayer = player;
        activeBoss = boss;
        isBattleActive = true;
        playerAttackTimer = 0f;
        bossAttackTimer = 0f;
        Debug.Log($"[BOSS BATTLE START] Melawan {boss.monsterName}!");
    }

    public void EndBattle()
    {
        isBattleActive = false;
        activeBoss = null;
        activePlayer = null;
    }

    private void ExecutePlayerAttack()
    {
        if (activeBoss == null) return;

        // Cek Hit / Miss berdasarkan rasio industri agar monster ber-evasion tinggi tidak kebal permanen
        float hitChance = (activePlayer.FinalAccuracy / (activePlayer.FinalAccuracy + activeBoss.evasion)) * 100f;
        if (Random.Range(0f, 100f) > hitChance)
        {
            Debug.Log("[BOSS BATTLE] Serangan Player MISS!");
            return;
        }

        // Hitung Damage Murni Player ke Boss (Min Damage = 1)
        float baseDmg = Mathf.Max(1f, activePlayer.FinalAttack - activeBoss.defense);

        // Hitung Critical Hit Sistem Umum
        if (Random.Range(0f, 100f) <= activePlayer.FinalCritRate)
        {
            // Jika Critical, damage ditambah bonus persen Crit Damage
            float critMultiplier = 2f + (activePlayer.FinalCritDamage / 100f);
            baseDmg *= critMultiplier;
            Debug.Log("[BOSS BATTLE] CRITICAL HIT!");
        }

        activeBoss.TakeDamage(baseDmg);
    }

    private void ExecuteBossAttack()
    {
        if (activeBoss == null || activePlayer == null) return;

        // Cek Hit / Miss Boss ke Player
        float hitChance = (activeBoss.accuracy / (activeBoss.accuracy + activePlayer.FinalEvasion)) * 100f;
        if (Random.Range(0f, 100f) > hitChance)
        {
            Debug.Log("[BOSS BATTLE] Serangan Boss MISS!");
            return;
        }

        // Hitung Damage Boss ke Player (Min Damage = 1)
        float damageToPlayer = Mathf.Max(1f, activeBoss.attack - activePlayer.FinalDefense);
        activePlayer.TakeDamage(damageToPlayer);
    }
}