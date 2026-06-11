using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentManager : MonoBehaviour
{
    [Header("--- Player Reference ---")]
    [SerializeField] private PlayerStatus playerStatus;

    [Header("--- Weapon UI Images ---")]
    [SerializeField] private Image rareWeaponImage;
    [SerializeField] private Image epicWeaponImage;
    [SerializeField] private Image mythicWeaponImage;

    [Header("--- Accessory UI Images ---")]
    [SerializeField] private Image rareAccessoryImage;
    [SerializeField] private Image epicAccessoryImage;
    [SerializeField] private Image mythicAccessoryImage;

    private Color lockedColor = new Color(0.2f, 0.2f, 0.2f, 1f);     
    private Color unlockedColor = Color.white;                        
    private Color equippedColor = new Color(0.5f, 0.5f, 0.5f, 1f);    

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

       
        UpdateWeaponVisuals();
        UpdateAccessoryVisuals();
    }

    private void UpdateWeaponVisuals()
    {
        // Rare Weapon
        SetItemColor(rareWeaponImage, playerStatus.ownsRareWeapon, playerStatus.activeWeaponTier == PlayerStatus.EquipmentTier.Rare);

        // Epic Weapon
        SetItemColor(epicWeaponImage, playerStatus.ownsEpicWeapon, playerStatus.activeWeaponTier == PlayerStatus.EquipmentTier.Epic);

        // Mythic Weapon
        SetItemColor(mythicWeaponImage, playerStatus.ownsMythicWeapon, playerStatus.activeWeaponTier == PlayerStatus.EquipmentTier.Mythic);
    }

    private void UpdateAccessoryVisuals()
    {
        // Rare Accessory
        SetItemColor(rareAccessoryImage, playerStatus.ownsRareAccessory, playerStatus.activeAccessoryTier == PlayerStatus.EquipmentTier.Rare);

        // Epic Accessory
        SetItemColor(epicAccessoryImage, playerStatus.ownsEpicAccessory, playerStatus.activeAccessoryTier == PlayerStatus.EquipmentTier.Epic);

        // Mythic Accessory
        SetItemColor(mythicAccessoryImage, playerStatus.ownsMythicAccessory, playerStatus.activeAccessoryTier == PlayerStatus.EquipmentTier.Mythic);
    }


    private void SetItemColor(Image itemImage, bool isOwned, bool isEquipped)
    {
        if (itemImage == null) return;

        if (!isOwned)
        {
            itemImage.color = lockedColor; 
        }
        else if (isEquipped)
        {
            itemImage.color = equippedColor; 
        }
        else
        {
            itemImage.color = unlockedColor; 
        }
    }


    public void EquipRareWeapon()
    {
        if (playerStatus != null && playerStatus.ownsRareWeapon)
        {
            playerStatus.activeWeaponTier = PlayerStatus.EquipmentTier.Rare;
            Debug.Log($"[EQUIP] Pakai RARE Weapon. ATK Real-time: {playerStatus.FinalAttack}");
        }
    }

    public void EquipEpicWeapon()
    {
        if (playerStatus != null && playerStatus.ownsEpicWeapon)
        {
            playerStatus.activeWeaponTier = PlayerStatus.EquipmentTier.Epic;
            Debug.Log($"[EQUIP] Pakai EPIC Weapon. ATK Real-time: {playerStatus.FinalAttack}");
        }
    }

    public void EquipMythicWeapon()
    {
        if (playerStatus != null && playerStatus.ownsMythicWeapon)
        {
            playerStatus.activeWeaponTier = PlayerStatus.EquipmentTier.Mythic;
            Debug.Log($"[EQUIP] Pakai MYTHIC Weapon. ATK Real-time: {playerStatus.FinalAttack}");
        }
    }


    public void EquipRareAccessory()
    {
        if (playerStatus != null && playerStatus.ownsRareAccessory)
        {
            playerStatus.activeAccessoryTier = PlayerStatus.EquipmentTier.Rare;
            playerStatus.ResetHpToMax();
            Debug.Log($"[EQUIP] Pakai RARE Accessory. HP Real-time: {playerStatus.FinalMaxHp}");
            NotificationLogManager.Instance.AddLog("Rare Weapon Equipped!", Color.magenta);
        }
    }

    public void EquipEpicAccessory()
    {
        if (playerStatus != null && playerStatus.ownsEpicAccessory)
        {
            playerStatus.activeAccessoryTier = PlayerStatus.EquipmentTier.Epic;
            playerStatus.ResetHpToMax();
            Debug.Log($"[EQUIP] Pakai EPIC Accessory. HP Real-time: {playerStatus.FinalMaxHp}");
            NotificationLogManager.Instance.AddLog("Epic Weapon Equipped!", Color.magenta);
        }
    }

    public void EquipMythicAccessory()
    {
        if (playerStatus != null && playerStatus.ownsMythicAccessory)
        {
            playerStatus.activeAccessoryTier = PlayerStatus.EquipmentTier.Mythic;
            playerStatus.ResetHpToMax();
            Debug.Log($"[EQUIP] Pakai MYTHIC Accessory. HP Real-time: {playerStatus.FinalMaxHp}");
            NotificationLogManager.Instance.AddLog("Mythic Weapon Equipped!", Color.magenta);
        }
    }
}