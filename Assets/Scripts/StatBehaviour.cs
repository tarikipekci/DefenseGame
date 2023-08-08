using UnityEngine;

public class StatBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _animatorV, _animatorD, _animatorA, _animatorM;
    private static readonly int Pressed = Animator.StringToHash("pressed");
    [SerializeField] private GameObject panel, continueButton;

    public void IncreaseVitality()
    {
        if (PlayerBehaviour.instance.level >= 1)
        {
            PlayerBehaviour.instance.maxHealth += 1;
            PlayerBehaviour.instance.healthBar.maxValue = PlayerBehaviour.instance.maxHealth;
            PlayerBehaviour.instance.currentHealth = PlayerBehaviour.instance.maxHealth;
            _animatorV.SetTrigger(Pressed);
            PlayerBehaviour.instance.level--;
            LevelManager.instance.vitalityLevel++;
            UpdateSkillPoints();
        }
    }

    public void IncreaseMovementSpeed()
    {
        if (PlayerBehaviour.instance.level >= 1)
        {
            PlayerBehaviour.instance.moveSpeed += 1;
            _animatorM.SetTrigger(Pressed);
            PlayerBehaviour.instance.level--;
            LevelManager.instance.movementSpeedLevel++;
            UpdateSkillPoints();
        }
    }

    public void IncreaseDamage()
    {
        if (PlayerBehaviour.instance.level >= 1)
        {
            WeaponBehaviour.instance.damage += 0.25f;
            _animatorD.SetTrigger(Pressed);
            PlayerBehaviour.instance.level--;
            LevelManager.instance.damageLevel++;
            UpdateSkillPoints();
        }
    }

    public void IncreaseAttackSpeed()
    {
        if (PlayerBehaviour.instance.level >= 1)
        {
            WeaponBehaviour.instance.attackSpeed += 0.01f;
            _animatorA.SetTrigger(Pressed);
            PlayerBehaviour.instance.level--;
            LevelManager.instance.attackSpeedLevel++;
            UpdateSkillPoints();
        }
    }

    public void CloseUpgradePanel()
    {
        panel.SetActive(false);
        continueButton.SetActive(false);
        Time.timeScale = 1f;
    }

    private void UpdateSkillPoints()
    {
        LevelManager.instance.SetUpgradePanel();
        LevelManager.instance.currentPoint.text = PlayerBehaviour.instance.level + " points left!";
        if (PlayerBehaviour.instance.level <= 0)
        {
            continueButton.SetActive(true);
        }
    }
}