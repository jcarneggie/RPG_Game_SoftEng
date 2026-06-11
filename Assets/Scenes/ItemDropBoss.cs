using UnityEngine;

public class ItemDropBoss : MonoBehaviour
{
    public enum ItemType { Coin, Xp, Diamond } 

    [Header("--- Item Settings ---")]
    [SerializeField] private ItemType itemType;
    [SerializeField] private int valueAmount = 5; 

    [Header("--- Magnet Mechanics ---")]
    [SerializeField] private float magnetRadius = 5f; 
    [SerializeField] private float moveSpeed = 10f;    

    private Transform playerTransform;
    private PlayerStatus playerStatus;
    private bool isBeingAttracted = false;

    void Start()
    {
       
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerStatus = playerObj.GetComponent<PlayerStatus>();
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

       
        if (distanceToPlayer <= magnetRadius || isBeingAttracted)
        {
            isBeingAttracted = true;

            
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            
            if (distanceToPlayer <= 0.2f)
            {
                CollectItem();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        if (playerStatus != null)
        {
            if (itemType == ItemType.Coin)
            {
                playerStatus.AddCoin(valueAmount);
            }
            else if (itemType == ItemType.Xp)
            {
                playerStatus.GainXp(valueAmount);
            }
            else if (itemType == ItemType.Diamond)
            {
                playerStatus.AddDiamond(valueAmount);
            }
        }

        Destroy(gameObject);
    }
}