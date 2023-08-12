using UnityEngine;

public class AnvilDropBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LevelManager.instance.StartCoroutine(LevelManager.instance.DropAnvil());
            Destroy(gameObject);
        }
    }
}