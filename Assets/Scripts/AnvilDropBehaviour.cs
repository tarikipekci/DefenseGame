using UnityEngine;

public class AnvilDropBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (LevelManager.instance.GetAnvilDropped() == false)
            {
                LevelManager.instance.SetAnvilDropped(true);
                LevelManager.instance.StartCoroutine(LevelManager.instance.DropAnvil());
                Destroy(gameObject);
            }
        }
    }
}