using UnityEngine;

public class StatBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _animatorV, _animatorD, _animatorA, _animatorM;
    private static readonly int Pressed = Animator.StringToHash("pressed");
    [SerializeField] private GameObject panel;

    public void IncreaseVitality()
    {
        if (PlayerBehaviour.instance.level >= 1)
        {
            PlayerBehaviour.instance.maxHealth += 1;
            PlayerBehaviour.instance.healthBar.maxValue = PlayerBehaviour.instance.maxHealth;
            PlayerBehaviour.instance.currentHealth = PlayerBehaviour.instance.maxHealth;
            _animatorV.SetTrigger(Pressed);
            PlayerBehaviour.instance.level--;
            LevelManager.instance.currentPoint.text = PlayerBehaviour.instance.level.ToString();
        }
    }

    public void IncreaseMovementSpeed()
    {
        if (PlayerBehaviour.instance.level >= 1)
        {
            PlayerBehaviour.instance.moveSpeed += 1;
            _animatorM.SetTrigger(Pressed);
            PlayerBehaviour.instance.level--;
            LevelManager.instance.currentPoint.text = PlayerBehaviour.instance.level.ToString();
        }
    }

    public void IncreaseDamage()
    {
        if (PlayerBehaviour.instance.level >= 1)
        {
            WeaponBehaviour.instance.damage += 1;
            _animatorD.SetTrigger(Pressed);
            PlayerBehaviour.instance.level--;
            LevelManager.instance.currentPoint.text = PlayerBehaviour.instance.level.ToString();
        }
    }

    public void IncreaseAttackSpeed()
    {
        if (PlayerBehaviour.instance.level >= 1)
        {
            WeaponBehaviour.instance.attackSpeed += 0.001f;
            _animatorA.SetTrigger(Pressed);
            PlayerBehaviour.instance.level--;
            LevelManager.instance.currentPoint.text = PlayerBehaviour.instance.level.ToString();
        }
    }

    public void CloseUpgradePanel()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}