using UnityEngine;

public class BossBattleSystem : MonoBehaviour
{
    public static BossBattleSystem Instance;

    [Header("--- Battle Targets ---")]
    [SerializeField] private PlayerStatus activePlayer;
    [SerializeField] private BossMonster activeBoss;

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


        playerAttackTimer += Time.deltaTime;
        if (playerAttackTimer >= (1f / activePlayer.FinalAttackSpeed))
        {
            ExecutePlayerAttack();
            playerAttackTimer = 0f;
        }


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


        float hitChance = (activePlayer.FinalAccuracy / (activePlayer.FinalAccuracy + activeBoss.evasion)) * 100f;
        if (Random.Range(0f, 100f) > hitChance)
        {
            Debug.Log("[BOSS BATTLE] Serangan Player MISS!");
            return;
        }


        float baseDmg = Mathf.Max(1f, activePlayer.FinalAttack - activeBoss.defense);


        if (Random.Range(0f, 100f) <= activePlayer.FinalCritRate)
        {

            float critMultiplier = 2f + (activePlayer.FinalCritDamage / 100f);
            baseDmg *= critMultiplier;
            Debug.Log("[BOSS BATTLE] CRITICAL HIT!");
        }

        activeBoss.TakeDamage(baseDmg);
    }

    private void ExecuteBossAttack()
    {
        if (activeBoss == null || activePlayer == null) return;


        float hitChance = (activeBoss.accuracy / (activeBoss.accuracy + activePlayer.FinalEvasion)) * 100f;
        if (Random.Range(0f, 100f) > hitChance)
        {
            Debug.Log("[BOSS BATTLE] Serangan Boss MISS!");
            return;
        }


        float damageToPlayer = Mathf.Max(1f, activeBoss.attack - activePlayer.FinalDefense);
        activePlayer.TakeDamage(damageToPlayer);
    }
}