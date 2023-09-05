using UnityEngine;

public class FireballBehaviour : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerBehaviour.instance.DecreaseHealthOfPlayer(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Edge"))
        {
            Destroy(gameObject);
        }
    }
}