using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance;

    [Header("--- Battle Targets ---")]
    [SerializeField] private PlayerStatus activePlayer;
    [SerializeField] private Monster activeMonster;

    private bool isBattleActive = false;
    private float playerAttackTimer = 0f;
    private float monsterAttackTimer = 0f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (!isBattleActive || activePlayer == null || activeMonster == null) return;

        // 1. Timer Serangan Otomatis Player
        playerAttackTimer += Time.deltaTime;
        if (playerAttackTimer >= (1f / activePlayer.FinalAttackSpeed))
        {
            ExecutePlayerAttack();
            playerAttackTimer = 0f;
        }

        // 2. Timer Serangan Otomatis Monster
        monsterAttackTimer += Time.deltaTime;
        if (monsterAttackTimer >= (1f / activeMonster.attackSpeed))
        {
            ExecuteMonsterAttack();
            monsterAttackTimer = 0f;
        }
    }

    public void StartBattle(PlayerStatus player, Monster monster)
    {
        activePlayer = player;
        activeMonster = monster;
        isBattleActive = true;
        playerAttackTimer = 0f;
        monsterAttackTimer = 0f;
        Debug.Log($"[BATTLE START] Melawan {monster.monsterName}!");
    }

    public void EndBattle()
    {
        isBattleActive = false;
        activeMonster = null;
        activePlayer = null;
    }

    private void ExecutePlayerAttack()
    {
        if (activeMonster == null) return;


        float hitChance = (activePlayer.FinalAccuracy / (activePlayer.FinalAccuracy + activeMonster.evasion)) * 100f;
        if (Random.Range(0f, 100f) > hitChance)
        {
            Debug.Log("[BATTLE] Serangan Player MISS!");
            return;
        }


        float baseDmg = Mathf.Max(1f, activePlayer.FinalAttack - activeMonster.defense);


        if (Random.Range(0f, 100f) <= activePlayer.FinalCritRate)
        {

            float critMultiplier = 2f + (activePlayer.FinalCritDamage / 100f);
            baseDmg *= critMultiplier;
            Debug.Log("[BATTLE] CRITICAL HIT!");
        }

        activeMonster.TakeDamage(baseDmg);
    }

    private void ExecuteMonsterAttack()
    {
        if (activeMonster == null || activePlayer == null) return;


        float hitChance = (activeMonster.accuracy / (activeMonster.accuracy + activePlayer.FinalEvasion)) * 100f;
        if (Random.Range(0f, 100f) > hitChance)
        {
            Debug.Log("[BATTLE] Serangan Monster MISS!");
            return;
        }


        float damageToPlayer = Mathf.Max(1f, activeMonster.attack - activePlayer.FinalDefense);
        activePlayer.TakeDamage(damageToPlayer);
    }
}