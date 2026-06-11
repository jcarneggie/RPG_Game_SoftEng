using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [Header("Pengaturan Gerakan")]
    [Tooltip("Seberapa cepat pergerakan background")]
    public float speed = 1f;

    [Tooltip("Jarak maksimal pergerakan ke kiri dan kanan dari posisi awal")]
    public float moveRange = 2f;

    // Untuk menyimpan posisi awal background agar tidak kebablasan
    private Vector3 startPosition;

    void Start()
    {
        // Catat posisi awal pas game pertama kali dinyalakan
        startPosition = transform.position;
    }

    void Update()
    {

        float movement = Mathf.Sin(Time.time * speed) * moveRange;


        transform.position = new Vector3(startPosition.x + movement, startPosition.y, startPosition.z);
    }
}