using UnityEngine;
using TMPro; 

public class UIStatManageDisplay : MonoBehaviour
{
    [Header("--- Player Reference ---")]
    [SerializeField] private PlayerStatus playerStatus;

    [Header("--- Leveling & Points Parents ---")]
    [SerializeField] private Transform levelParent;
    [SerializeField] private Transform xpParent;
    [SerializeField] private Transform availablePointsParent;

    [Header("--- Stat Tracker Parents (Mulai dari 0) ---")]
    [SerializeField] private Transform hpParent;
    [SerializeField] private Transform attackParent;
    [SerializeField] private Transform defenseParent;
    [SerializeField] private Transform accuracyParent;
    [SerializeField] private Transform hpRecoveryParent;
    [SerializeField] private Transform evasionParent;
    [SerializeField] private Transform critDamageParent;
    [SerializeField] private Transform critChanceParent;

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


        UpdateValueText(levelParent, "" + playerStatus.currentLevel);
        UpdateValueText(xpParent, playerStatus.currentXp + " / " + playerStatus.maxXp);
        UpdateValueText(availablePointsParent, playerStatus.availableStatPoints.ToString());


        UpdateValueText(hpParent, "" + playerStatus.allocatedHpPoints);
        UpdateValueText(attackParent, "" + playerStatus.allocatedAttackPoints);
        UpdateValueText(defenseParent, "" + playerStatus.allocatedDefensePoints);
        UpdateValueText(accuracyParent, "" + playerStatus.allocatedAccuracyPoints);
        UpdateValueText(hpRecoveryParent, "" + playerStatus.allocatedHpRecoveryPoints);
        UpdateValueText(evasionParent, "" + playerStatus.allocatedEvasionPoints);
        UpdateValueText(critDamageParent, "" + playerStatus.allocatedCritDamagePoints + "%");
        UpdateValueText(critChanceParent, "" + playerStatus.allocatedCritRatePoints + "%");
    }

    private void UpdateValueText(Transform parentTransform, string newValue)
    {
        if (parentTransform == null) return;


        Transform valueChild = parentTransform.Find("isi");
        if (valueChild != null)
        {

            TextMeshProUGUI tmpComponent = valueChild.GetComponent<TextMeshProUGUI>();
            if (tmpComponent != null)
            {
                tmpComponent.text = newValue;
            }
        }
    }


    public void ClickAddHp() { if (playerStatus != null) playerStatus.AllocatePointToHp(); }
    public void ClickAddAttack() { if (playerStatus != null) playerStatus.AllocatePointToAttack(); }
    public void ClickAddDefense() { if (playerStatus != null) playerStatus.AllocatePointToDefense(); }
    public void ClickAddAccuracy() { if (playerStatus != null) playerStatus.AllocatePointToAccuracy(); }
    public void ClickAddHpRecovery() { if (playerStatus != null) playerStatus.AllocatePointToHpRecovery(); }
    public void ClickAddEvasion() { if (playerStatus != null) playerStatus.AllocatePointToEvasion(); }
    public void ClickAddCritDamage() { if (playerStatus != null) playerStatus.AllocatePointToCritDamage(); }
    public void ClickAddCritChance() { if (playerStatus != null) playerStatus.AllocatePointToCritRate(); }
}