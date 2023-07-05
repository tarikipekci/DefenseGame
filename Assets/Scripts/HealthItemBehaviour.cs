using UnityEngine;

public class HealthItemBehaviour : MonoBehaviour
{
    [SerializeField] private int healAmount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerBehaviour.instance.currentHealth < PlayerBehaviour.instance.maxHealth)
            {
                PlayerBehaviour.instance.IncreaseHealthOfPlayer(healAmount);
                Destroy(this.gameObject);
            }
        }
    }
}