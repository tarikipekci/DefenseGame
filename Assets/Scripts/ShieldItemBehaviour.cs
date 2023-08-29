using UnityEngine;

public class ShieldItemBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerBehaviour.instance.SetShieldEnabled(true);
            PlayerBehaviour.instance.GetShieldObject().SetActive(true);
            Destroy(gameObject);
        }
    }
}
