using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("--- Monster Stats ---")]
    public string monsterName = "Delta Virus";
    public float maxHp = 50f;
    public float currentHp;
    public float attack = 50f;
    public float defense = 10f;
    public float accuracy = 100f;
    public float evasion = 10f;
    public float attackSpeed = 1f;

    [Header("--- Rewards Drop Prefabs ---")]
    public GameObject coinPrefab;
    public GameObject xpPrefab;

    [Header("--- UI References (Sistem Baru) ---")]
    [Tooltip("Tarik objek Slider HP Monster ke sini")]
    [SerializeField] private UIProgressBar monsterHpBar;

    [Header("--- Hover Animation ---")]
    [SerializeField] private float floatSpeed = 3f;
    [SerializeField] private float floatHeight = 0.2f;

    private Vector3 startPos;
    private Vector3 initialHierarchyPos;
    private bool isMyTurnToFight = false;

    void Start()
    {
        currentHp = maxHp;
        startPos = transform.position;
        initialHierarchyPos = transform.position; 

        
        if (monsterHpBar != null) monsterHpBar.UpdateValue(currentHp, maxHp);
    }

    void Update()
    {
        if (GameplayManager.Instance.IsMapMoving)
        {
            transform.Translate(Vector3.left * GameplayManager.Instance.MapSpeed * Time.deltaTime, Space.World);
            startPos.x -= GameplayManager.Instance.MapSpeed * Time.deltaTime;
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

        
        if (monsterHpBar != null) monsterHpBar.UpdateValue(currentHp, maxHp);

        if (currentHp <= 0) Die();
    }

    private void Die()
    {
        if (coinPrefab != null) Instantiate(coinPrefab, transform.position + Vector3.up, Quaternion.identity);
        if (xpPrefab != null) Instantiate(xpPrefab, transform.position + Vector3.down, Quaternion.identity);
        if (NotificationLogManager.Instance != null)
        {
            
            NotificationLogManager.Instance.AddLog($"Killed {monsterName}", Color.white);
            NotificationLogManager.Instance.AddLog("Virus Contamination Added + 0.1%", Color.white);
        }
        GameplayManager.Instance.OnMonsterDefeated();

        
        gameObject.SetActive(false);
    }

    
    public void ResetMonster()
    {
        transform.position = initialHierarchyPos; 
        startPos = initialHierarchyPos;           
        currentHp = maxHp;                        
        isMyTurnToFight = false;

        if (monsterHpBar != null) monsterHpBar.UpdateValue(currentHp, maxHp); 
        gameObject.SetActive(true);               
    }
}