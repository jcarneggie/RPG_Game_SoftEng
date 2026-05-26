using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    [Header("References")]
    [SerializeField] private BackgroundParallax frontBackground;
    // Nanti tinggal tarik animator atau script Player/Monster lu ke sini

    private void Awake()
    {
        // Singleton pattern agar script ini mudah dipanggil dari script Monster atau Player nanti
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Panggil fungsi ini saat monster muncul di depan karakter.
    /// </summary>
    public void OnEncounterMonster()
    {
        Debug.Log("Monster menghadang! Sumbu dunia berhenti bergerak.");
        frontBackground.SetMovement(false);

        // TODO: Ubah animasi karakter lu dari WALK ke IDLE / ATTACK di sini
        // playerAnimator.SetTrigger("Attack");
    }

    /// <summary>
    /// Panggil fungsi ini dari script Monster ketika HP Monster mencapai 0.
    /// </summary>
    public void OnMonsterDefeated()
    {
        Debug.Log("Monster mati! Melanjutkan perjalanan...");
        frontBackground.SetMovement(true);

        // TODO: Kembalikan animasi karakter lu dari IDLE ke WALK di sini
        // playerAnimator.SetTrigger("Walk");
    }

    /// <summary>
    /// Panggil fungsi ini dari script Player ketika HP Player mencapai 0.
    /// </summary>
    public void OnPlayerDeath()
    {
        Debug.Log("Karakter mati! Mengulang petualangan dari awal...");

        // 1. Hentikan gerak background sementara
        frontBackground.SetMovement(false);

        // 2. Reset posisi background ke titik koordinat awal semula
        frontBackground.ResetPosition();

        // 3. Hidupkan pergerakan background kembali untuk petualangan baru
        frontBackground.SetMovement(true);

        // TODO: Reset HP Player ke penuh, hancurkan sisa monster di layar, dll.
    }
}