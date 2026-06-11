using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerSaveData
{
    public int level, xp, maxXp, statPoints;
    public int coin, diamond;
    public float contamination;
    public float currentHealth;
    public string currentFloorScene;

    public int allocHp, allocAtk, allocDef, allocAcc, allocHpRec, allocEva, allocCritDmg, allocCritRate;

    public float baseHp, baseAtk, baseDef, baseAcc, baseHpRec, baseEva, baseCritDmg, baseCritRate;

    public bool owRW, owEW, owMW, owRA, owEA, owMA;

    public PlayerStatus.EquipmentTier actWep, actAcc;
    public PlayerStatus.SkillType[] eqSkills;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Header("--- Reference ---")]
    [Tooltip("Sekarang bakal OTOMATIS terisi sendiri, lu gak usah repot colok-colok lagi cok!")]
    [SerializeField] private PlayerStatus playerStatus;

    private string saveFilePath;

    private void Awake()
    {
  
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }

        saveFilePath = Application.persistentDataPath + "/rpg_savefile.json";


        if (playerStatus == null) playerStatus = FindObjectOfType<PlayerStatus>();

        LoadGame();
    }

    public void SaveGame()
    {

        if (playerStatus == null) playerStatus = FindObjectOfType<PlayerStatus>();
        if (playerStatus == null)
        {
            Debug.LogError("[SAVE SYSTEM] Gagal Save! PlayerStatus tidak ditemukan di scene ini!");
            return;
        }

        PlayerSaveData data = new PlayerSaveData();

        data.level = playerStatus.currentLevel;
        data.xp = playerStatus.currentXp;
        data.maxXp = playerStatus.maxXp;
        data.statPoints = playerStatus.availableStatPoints;

        data.coin = playerStatus.currentCoin;
        data.diamond = playerStatus.currentDiamond;
        data.contamination = playerStatus.currentContamination;

        data.currentHealth = playerStatus.currentHp;

        data.currentFloorScene = SceneManager.GetActiveScene().name;

        data.allocHp = playerStatus.allocatedHpPoints;
        data.allocAtk = playerStatus.allocatedAttackPoints;
        data.allocDef = playerStatus.allocatedDefensePoints;
        data.allocAcc = playerStatus.allocatedAccuracyPoints;
        data.allocHpRec = playerStatus.allocatedHpRecoveryPoints;
        data.allocEva = playerStatus.allocatedEvasionPoints;
        data.allocCritDmg = playerStatus.allocatedCritDamagePoints;
        data.allocCritRate = playerStatus.allocatedCritRatePoints;

        data.baseHp = playerStatus.GetBaseStat("hp");
        data.baseAtk = playerStatus.GetBaseStat("attack");
        data.baseDef = playerStatus.GetBaseStat("defense");
        data.baseAcc = playerStatus.GetBaseStat("accuracy");
        data.baseHpRec = playerStatus.GetBaseStat("hpRecovery");
        data.baseEva = playerStatus.GetBaseStat("evasion");
        data.baseCritDmg = playerStatus.GetBaseStat("critDamage");
        data.baseCritRate = playerStatus.GetBaseStat("critRate");

        data.owRW = playerStatus.ownsRareWeapon;
        data.owEW = playerStatus.ownsEpicWeapon;
        data.owMW = playerStatus.ownsMythicWeapon;
        data.owRA = playerStatus.ownsRareAccessory;
        data.owEA = playerStatus.ownsEpicAccessory;
        data.owMA = playerStatus.ownsMythicAccessory;

        data.actWep = playerStatus.activeWeaponTier;
        data.actAcc = playerStatus.activeAccessoryTier;
        data.eqSkills = playerStatus.equippedSkills;

        string jsonText = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, jsonText);

        Debug.Log("[SAVE SYSTEM] 100% Progress berhasil dikunci ke Device!");
    }

    public void LoadGame()
    {
        if (playerStatus == null) return;

        if (File.Exists(saveFilePath))
        {
            string jsonText = File.ReadAllText(saveFilePath);
            PlayerSaveData loadedData = JsonUtility.FromJson<PlayerSaveData>(jsonText);

            playerStatus.currentLevel = loadedData.level;
            playerStatus.currentXp = loadedData.xp;
            playerStatus.maxXp = loadedData.maxXp;
            playerStatus.availableStatPoints = loadedData.statPoints;

            playerStatus.currentCoin = loadedData.coin;
            playerStatus.currentDiamond = loadedData.diamond;
            playerStatus.currentContamination = loadedData.contamination;

            playerStatus.allocatedHpPoints = loadedData.allocHp;
            playerStatus.allocatedAttackPoints = loadedData.allocAtk;
            playerStatus.allocatedDefensePoints = loadedData.allocDef;
            playerStatus.allocatedAccuracyPoints = loadedData.allocAcc;
            playerStatus.allocatedHpRecoveryPoints = loadedData.allocHpRec;
            playerStatus.allocatedEvasionPoints = loadedData.allocEva;
            playerStatus.allocatedCritDamagePoints = loadedData.allocCritDmg;
            playerStatus.allocatedCritRatePoints = loadedData.allocCritRate;

            playerStatus.SetBaseStats(loadedData.baseHp, loadedData.baseAtk, loadedData.baseDef, loadedData.baseAcc, loadedData.baseHpRec, loadedData.baseEva, loadedData.baseCritDmg, loadedData.baseCritRate);

            playerStatus.ownsRareWeapon = loadedData.owRW;
            playerStatus.ownsEpicWeapon = loadedData.owEW;
            playerStatus.ownsMythicWeapon = loadedData.owMW;
            playerStatus.ownsRareAccessory = loadedData.owRA;
            playerStatus.ownsEpicAccessory = loadedData.owEA;
            playerStatus.ownsMythicAccessory = loadedData.owMA;

            playerStatus.activeWeaponTier = loadedData.actWep;
            playerStatus.activeAccessoryTier = loadedData.actAcc;
            playerStatus.equippedSkills = loadedData.eqSkills;

            playerStatus.LoadCurrentHp(loadedData.currentHealth);

            Debug.Log($"[SAVE SYSTEM] Data sukses ditarik! Player aman di scene: {loadedData.currentFloorScene}");
        }
        else
        {
            Debug.Log("[SAVE SYSTEM] Data kosong. Memulai game baru murni.");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) SaveGame();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        playerStatus = FindObjectOfType<PlayerStatus>();
        if (playerStatus != null)
        {
            LoadGame();
        }
    }
}