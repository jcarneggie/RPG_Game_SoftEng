using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [System.Serializable]
    public enum EquipmentTier { None, Rare, Epic, Mythic }

    [System.Serializable]
    public enum SkillType { None, IronWill, DancingWaves, IronBody, IronEye }

    [Header("--- Leveling & Points System ---")]
    public int currentLevel = 1;
    public int currentXp = 0;
    public int maxXp = 10;
    public int availableStatPoints = 0;

    [Header("--- Currency System (Sistem Baru) ---")]
    public int currentCoin = 0;
    public int currentDiamond = 0;

    // ========================================================
    // SAKTI: SISTEM KONTAMINASI & DEBUFF
    // ========================================================
    [Header("--- Contamination System ---")]
    [Range(0f, 100f)] public float currentContamination = 0f; // Nilai 0% sampai 100%

    [Header("--- Allocated Points Tracker (Display Kanan) ---")]
    public int allocatedHpPoints = 0;
    public int allocatedAttackPoints = 0;
    public int allocatedDefensePoints = 0;
    public int allocatedAccuracyPoints = 0;
    public int allocatedHpRecoveryPoints = 0;
    public int allocatedEvasionPoints = 0;
    public int allocatedCritDamagePoints = 0;
    public int allocatedCritRatePoints = 0;

    [Header("--- Player Default Stats ---")]
    [SerializeField] private float baseHp = 100f;
    [SerializeField] private float baseAttack = 100f;
    [SerializeField] private float baseDefense = 100f;
    [SerializeField] private float baseHpRecovery = 10f;
    [SerializeField] private float baseEvasion = 100f;
    [SerializeField] private float baseCritDamage = 0f;
    [SerializeField] private float baseCritRate = 0f;
    [SerializeField] private float baseAccuracy = 100f;
    [SerializeField] private float baseAttackSpeed = 1f;

    [Header("--- Owned Equipments (Hasil Gacha) ---")]
    public bool ownsRareWeapon = false;
    public bool ownsEpicWeapon = false;
    public bool ownsMythicWeapon = false;

    public bool ownsRareAccessory = false;
    public bool ownsEpicAccessory = false;
    public bool ownsMythicAccessory = false;

    [Header("--- Equipment Slots (Current) ---")]
    public EquipmentTier activeWeaponTier = EquipmentTier.None;
    public EquipmentTier activeAccessoryTier = EquipmentTier.None;

    [Header("--- Skill Slots ---")]
    public SkillType[] equippedSkills = new SkillType[4];

    [Header("--- UI References ---")]
    [Tooltip("Tarik objek Slider HP Player ke sini")]
    [SerializeField] private UIProgressBar playerHpBar;

    public float currentHp { get; private set; }

    // SAKTI: Timer internal untuk menghitung jeda 1 detik HP Recovery
    private float hpRecoveryTimer = 0f;

    void Awake()
    {
        ResetHpToMax();
    }

    // ========================================================
    // SAKTI: UPDATE UNTUK MENJALANKAN RECOVERY SETIAP DETIK
    // ========================================================
    void Update()
    {
        // HP Recovery hanya berjalan jika player masih hidup dan darahnya belum penuh
        if (currentHp > 0 && currentHp < FinalMaxHp)
        {
            hpRecoveryTimer += Time.deltaTime;

            // Begitu waktu mencapai atau melewati 1 detik
            if (hpRecoveryTimer >= 1f)
            {
                // Tambahkan HP sesuai status final HP Recovery lu
                currentHp = Mathf.Min(FinalMaxHp, currentHp + FinalHpRecovery);

                // Update visual ke bar UI darah
                if (playerHpBar != null) playerHpBar.UpdateValue(currentHp, FinalMaxHp);

                // Reset timer kembali ke nol untuk hitungan detik berikutnya
                hpRecoveryTimer = 0f;
            }
        }
    }

    public void GainXp(int amount)
    {
        currentXp += amount;
        while (currentXp >= maxXp)
        {
            currentXp -= maxXp;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        availableStatPoints += 3;
        maxXp = maxXp + maxXp;
        Debug.Log($"LEVEL UP! Sekarang Level {currentLevel}. Poin Tersedia: {availableStatPoints}");
    }

    public void AddCoin(int amount)
    {
        currentCoin += amount;
    }

    public void AddDiamond(int amount)
    {
        currentDiamond += amount;
    }

    public bool SpendDiamond(int amount)
    {
        if (currentDiamond >= amount)
        {
            currentDiamond -= amount;
            return true;
        }
        return false;
    }

    public void ApplyMimicWeaponCurse()
    {
        baseAttack = Mathf.Max(1f, baseAttack - 1f);
    }

    public void ApplyMimicAccessoryCurse()
    {
        baseHp = Mathf.Max(1f, baseHp - 1f);
        ResetHpToMax();
    }

    // ========================================================
    // SAKTI: LOGIKA HITUNG DEBUFF KONTAMINASI & CLEANING
    // ========================================================
    public float GetContaminationDebuff()
    {
        if (currentContamination >= 90f) return 0.3f; // 90-100% -> 30% debuff
        if (currentContamination >= 80f) return 0.2f; // 80-89% -> 20% debuff
        if (currentContamination >= 70f) return 0.1f; // 70-79% -> 10% debuff
        return 0f; // Dibawah 70% aman
    }

    public bool CleanContamination()
    {
        if (currentContamination >= 50f && currentCoin >= 1000)
        {
            currentCoin -= 1000; // SAKTI: Gua benerin bug lu di sini, masa bayar 1000 tapi ngurangnya cuma 10 koin wkwk
            currentContamination = 0f; // Reset ke 0%
            Debug.Log("[CLEANING] Sukses! Kontaminasi bersih 0%. Koin -1000.");
            return true;
        }
        Debug.LogWarning("[CLEANING] Gagal! Koin lu kurang atau belum 50% cok!");
        return false;
    }

    #region STAT POINT ALLOCATION
    public void AllocatePointToHp() { if (availableStatPoints <= 0) return; allocatedHpPoints++; baseHp += 100f; availableStatPoints--; ResetHpToMax(); }
    public void AllocatePointToAttack() { if (availableStatPoints <= 0) return; allocatedAttackPoints++; baseAttack += 100f; availableStatPoints--; }
    public void AllocatePointToDefense() { if (availableStatPoints <= 0) return; allocatedDefensePoints++; baseDefense += 100f; availableStatPoints--; }
    public void AllocatePointToAccuracy() { if (availableStatPoints <= 0) return; allocatedAccuracyPoints++; baseAccuracy += 10f; availableStatPoints--; }
    public void AllocatePointToHpRecovery() { if (availableStatPoints <= 0) return; allocatedHpRecoveryPoints++; baseHpRecovery += 10f; availableStatPoints--; }
    public void AllocatePointToEvasion() { if (availableStatPoints <= 0) return; allocatedEvasionPoints++; baseEvasion += 100f; availableStatPoints--; }
    public void AllocatePointToCritDamage() { if (availableStatPoints <= 0) return; allocatedCritDamagePoints++; baseCritDamage += 5f; availableStatPoints--; }
    public void AllocatePointToCritRate() { if (availableStatPoints <= 0) return; if (baseCritRate >= 100f) return; allocatedCritRatePoints++; baseCritRate += 0.5f; if (baseCritRate > 100f) baseCritRate = 100f; availableStatPoints--; }
    #endregion

    public bool HasSkill(SkillType skillToCheck)
    {
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            if (equippedSkills[i] == skillToCheck) return true;
        }
        return false;
    }

    #region GETTERS FOR FINAL STATS (SISTEM DEBUFF EQUIPMENT SAKTI)
    public float FinalAttack
    {
        get
        {
            float multiplier = 1f;
            if (activeWeaponTier == EquipmentTier.Rare) multiplier = 2f;
            else if (activeWeaponTier == EquipmentTier.Epic) multiplier = 3f;
            else if (activeWeaponTier == EquipmentTier.Mythic) multiplier = 4f;

            // Memotong kekuatan multiplier equipment berdasarkan debuff kontaminasi
            float debuff = GetContaminationDebuff();
            float effectiveMultiplier = 1f + ((multiplier - 1f) * (1f - debuff));

            float stat = baseAttack * effectiveMultiplier;
            if (HasSkill(SkillType.IronWill)) stat *= 1.1f;
            return stat;
        }
    }

    public float FinalMaxHp
    {
        get
        {
            float multiplier = 1f;
            if (activeAccessoryTier == EquipmentTier.Rare) multiplier = 2f;
            else if (activeAccessoryTier == EquipmentTier.Epic) multiplier = 3f;
            else if (activeAccessoryTier == EquipmentTier.Mythic) multiplier = 4f;

            float debuff = GetContaminationDebuff();
            float effectiveMultiplier = 1f + ((multiplier - 1f) * (1f - debuff));

            return baseHp * effectiveMultiplier;
        }
    }

    public float FinalDefense
    {
        get
        {
            float multiplier = 1f;
            if (activeAccessoryTier == EquipmentTier.Rare) multiplier = 2f;
            else if (activeAccessoryTier == EquipmentTier.Epic) multiplier = 3f;
            else if (activeAccessoryTier == EquipmentTier.Mythic) multiplier = 4f;

            float debuff = GetContaminationDebuff();
            float effectiveMultiplier = 1f + ((multiplier - 1f) * (1f - debuff));

            float stat = baseDefense * effectiveMultiplier;
            if (HasSkill(SkillType.IronBody)) stat *= 1.1f;
            return stat;
        }
    }

    public float FinalHpRecovery
    {
        get
        {
            float multiplier = 1f;
            if (activeAccessoryTier == EquipmentTier.Rare) multiplier = 2f;
            else if (activeAccessoryTier == EquipmentTier.Epic) multiplier = 3f;
            else if (activeAccessoryTier == EquipmentTier.Mythic) multiplier = 4f;

            float debuff = GetContaminationDebuff();
            float effectiveMultiplier = 1f + ((multiplier - 1f) * (1f - debuff));

            return baseHpRecovery * effectiveMultiplier;
        }
    }

    public float FinalAttackSpeed
    {
        get
        {
            float multiplier = 1f;
            if (activeWeaponTier == EquipmentTier.Rare) multiplier = 2f;
            else if (activeWeaponTier == EquipmentTier.Epic) multiplier = 2f;
            else if (activeWeaponTier == EquipmentTier.Mythic) multiplier = 4f;

            float debuff = GetContaminationDebuff();
            float effectiveMultiplier = 1f + ((multiplier - 1f) * (1f - debuff));

            return baseAttackSpeed * effectiveMultiplier;
        }
    }

    public float FinalEvasion { get { float stat = baseEvasion; if (HasSkill(SkillType.DancingWaves)) stat *= 1.1f; return stat; } }
    public float FinalAccuracy { get { float stat = baseAccuracy; if (HasSkill(SkillType.IronEye)) stat *= 1.1f; return stat; } }
    public float FinalCritDamage => baseCritDamage;
    public float FinalCritRate => baseCritRate;
    #endregion

    public void ResetHpToMax()
    {
        currentHp = FinalMaxHp;
        if (playerHpBar != null) playerHpBar.UpdateValue(currentHp, FinalMaxHp);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHp -= damageAmount;
        if (playerHpBar != null) playerHpBar.UpdateValue(currentHp, FinalMaxHp);

        if (currentHp <= 0)
        {
            currentHp = 0;
            ResetHpToMax(); // Darah di-reset buat persiapan scene selanjutnya

            // SAKTI: Cek lu lagi ada di scene mana!
            if (GameplayManager.Instance != null)
            {
                // Kalau ada GameplayManager biasa, berarti lu di Floor Normal
                GameplayManager.Instance.OnPlayerDeath();
            }
            else if (BossGameplayManager.Instance != null)
            {
                // Kalau ada BossGameplayManager, berarti lu di Stage Boss
                BossGameplayManager.Instance.OnPlayerDeath();
            }
        }
    }

    // ========================================================
    // SAKTI: FUNGSI BANTUAN UNTUK SAVE MANAGER BACA & TULIS DATA
    // ========================================================
    public float GetBaseStat(string statType)
    {
        switch (statType)
        {
            case "hp": return baseHp;
            case "attack": return baseAttack;
            case "defense": return baseDefense;
            case "accuracy": return baseAccuracy;
            case "hpRecovery": return baseHpRecovery;
            case "evasion": return baseEvasion;
            case "critDamage": return baseCritDamage;
            case "critRate": return baseCritRate;
            default: return 0f;
        }
    }

    public void SetBaseStats(float hp, float atk, float def, float acc, float hpRec, float eva, float critD, float critR)
    {
        baseHp = hp;
        baseAttack = atk;
        baseDefense = def;
        baseAccuracy = acc;
        baseHpRecovery = hpRec;
        baseEvasion = eva;
        baseCritDamage = critD;
        baseCritRate = critR;
    }

    // ========================================================
    // SAKTI: FUNGSI BANTUAN BUAT NGE-LOAD SISA HP TERAKHIR
    // ========================================================
    public void LoadCurrentHp(float savedHp)
    {
        currentHp = savedHp;
        // Pastikan darah ga nembus batas atas atau bawah pas diload
        if (currentHp > FinalMaxHp) currentHp = FinalMaxHp;
        if (currentHp <= 0) currentHp = FinalMaxHp;

        if (playerHpBar != null) playerHpBar.UpdateValue(currentHp, FinalMaxHp);
    }
}