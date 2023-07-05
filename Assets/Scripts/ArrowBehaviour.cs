using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyBehaviour>().DecreaseHealthOfEnemy(WeaponBehaviour.instance.damage);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Edge"))
        {
            Destroy(this.gameObject);
        }
    }
}