using UnityEngine;

public class XpBehaviour : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxDistanceFromPlayer;

    private void Start()
    {
        player = PlayerBehaviour.instance.transform;
    }

    private void Update()
    {
        if (Vector2.Distance(player.position, transform.position) < maxDistanceFromPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerBehaviour.instance.xpAmount += 2 / (PlayerBehaviour.instance.levelBackup + 1);
            Destroy(this.gameObject);
        }
    }
}