using UnityEngine;
using UnityEngine.UI;

public class UISkillManager : MonoBehaviour
{
    [Header("--- Player Reference ---")]
    [SerializeField] private PlayerStatus playerStatus;

    [Header("--- Slot UI References ---")]
    [Tooltip("Masukkan Button Slot 1 sampai 4 berurutan")]
    [SerializeField] private Button[] slotButtons = new Button[4];
    [SerializeField] private Image[] slotImages = new Image[4];

    [Header("--- Sprite Resources ---")]
    [Tooltip("Gambar default saat slot kosong (belum dipasang skill)")]
    [SerializeField] private Sprite emptySlotSprite;

    [Tooltip("Gambar masing-masing skill")]
    [SerializeField] private Sprite ironWillSprite;
    [SerializeField] private Sprite dancingWavesSprite;
    [SerializeField] private Sprite ironBodySprite;
    [SerializeField] private Sprite ironEyeSprite;

    private Color lockedColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    private Color unlockedColor = Color.white;


    private int[] slotUnlockLevels = new int[] { 1, 5, 10, 15 };

    void Start()
    {
        if (playerStatus == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerStatus = playerObj.GetComponent<PlayerStatus>();
        }
    }

    void Update()
    {
        if (playerStatus == null) return;
        RefreshSlotUI();
    }


    private void RefreshSlotUI()
    {
        int pLevel = playerStatus.currentLevel;

        for (int i = 0; i < 4; i++)
        {
            bool isUnlocked = pLevel >= slotUnlockLevels[i];


            slotButtons[i].interactable = isUnlocked;

            if (!isUnlocked)
            {

                slotImages[i].color = lockedColor;
                slotImages[i].sprite = emptySlotSprite;
            }
            else
            {

                slotImages[i].color = unlockedColor;

                PlayerStatus.SkillType currentSkill = playerStatus.equippedSkills[i];
                if (currentSkill == PlayerStatus.SkillType.None)
                {
                    slotImages[i].sprite = emptySlotSprite; 
                }
                else
                {
                    slotImages[i].sprite = GetSpriteForSkill(currentSkill);
                }
            }
        }
    }

    private Sprite GetSpriteForSkill(PlayerStatus.SkillType skill)
    {
        switch (skill)
        {
            case PlayerStatus.SkillType.IronWill: return ironWillSprite;
            case PlayerStatus.SkillType.DancingWaves: return dancingWavesSprite;
            case PlayerStatus.SkillType.IronBody: return ironBodySprite;
            case PlayerStatus.SkillType.IronEye: return ironEyeSprite;
            default: return emptySlotSprite;
        }
    }


    public void EquipSkill(int skillTypeIndex)
    {
        PlayerStatus.SkillType skillToEquip = (PlayerStatus.SkillType)skillTypeIndex;


        if (playerStatus.HasSkill(skillToEquip))
        {
            Debug.LogWarning("Lu udah pasang skill ini cok! Gak bisa dipasang dobel!");
            return;
        }


        for (int i = 0; i < 4; i++)
        {
            bool isUnlocked = playerStatus.currentLevel >= slotUnlockLevels[i];

            if (isUnlocked && playerStatus.equippedSkills[i] == PlayerStatus.SkillType.None)
            {

                playerStatus.equippedSkills[i] = skillToEquip;
                Debug.Log($"Skill {skillToEquip} berhasil dipasang ke Slot {i + 1}");
                NotificationLogManager.Instance.AddLog($"Skill {skillToEquip} Activated!", Color.green);
                return;
            }
        }

        Debug.LogWarning("Semua slot lu udah penuh atau belum kebuka cok!");
    }


    public void UnequipSlot(int slotIndex)
    {

        if (playerStatus.currentLevel >= slotUnlockLevels[slotIndex])
        {
            if (playerStatus.equippedSkills[slotIndex] != PlayerStatus.SkillType.None)
            {
                playerStatus.equippedSkills[slotIndex] = PlayerStatus.SkillType.None;
                Debug.Log($"Slot {slotIndex + 1} berhasil dicopot!");
            }
        }
    }


    public void ClickAddIronWill() { EquipSkill((int)PlayerStatus.SkillType.IronWill); }
    public void ClickAddDancingWaves() { EquipSkill((int)PlayerStatus.SkillType.DancingWaves); }
    public void ClickAddIronBody() { EquipSkill((int)PlayerStatus.SkillType.IronBody); }
    public void ClickAddIronEye() { EquipSkill((int)PlayerStatus.SkillType.IronEye); }

    public void ClickSlot1() { UnequipSlot(0); }
    public void ClickSlot2() { UnequipSlot(1); }
    public void ClickSlot3() { UnequipSlot(2); }
    public void ClickSlot4() { UnequipSlot(3); }
}