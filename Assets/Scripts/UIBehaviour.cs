using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private Text healthBar;
    [SerializeField] private PlayerBehaviour player;
    private void Update()
    {
        healthBar.text = player.currentHealth + "/" + player.maxHealth;
    }
}
