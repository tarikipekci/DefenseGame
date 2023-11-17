using UnityEngine;

public class PlayerFireballBehaviour : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyBehaviour>().DecreaseHealthOfEnemy(damage);
            gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Edge"))
        {
            Destroy(gameObject);
        }
    }
}