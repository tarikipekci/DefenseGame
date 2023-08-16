using System;
using UnityEngine;

public class AnvilBehaviour : MonoBehaviour
{
    [SerializeField] private float damage;

    private void Update()
    {
        if (LevelManager.instance.GetAnvilDropped() == false)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBehaviour>().DecreaseHealthOfEnemy(damage);
        }
    }
}