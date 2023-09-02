using UnityEngine;

public class StatBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _animatorV, _animatorD, _animatorA, _animatorM;
    private static readonly int Pressed = Animator.StringToHash("pressed");
    [SerializeField] private GameObject panel, continueButton;
    private bool upgraded;

    public void IncreaseVitality()
    {
        if (upgraded == false)
        {
            PlayerBehaviour.instance.maxHealth += 1;
            PlayerBehaviour.instance.healthBar.maxValue = PlayerBehaviour.instance.maxHealth;
            PlayerBehaviour.instance.currentHealth = PlayerBehaviour.instance.maxHealth;
            _animatorV.SetTrigger(Pressed);
            LevelManager.instance.vitalityLevel++;
            upgraded = true;
            UpdateSkillPoints();
        }
    }

    public void IncreaseMovementSpeed()
    {
        if (upgraded == false)
        {
            PlayerBehaviour.instance.moveSpeed += 1;
            _animatorM.SetTrigger(Pressed);
            LevelManager.instance.movementSpeedLevel++;
            upgraded = true;
            UpdateSkillPoints();
        }
    }

    public void IncreaseDamage()
    {
        if (upgraded == false)
        {
            WeaponBehaviour.instance.damage += 0.25f;
            _animatorD.SetTrigger(Pressed);
            LevelManager.instance.damageLevel++;
            upgraded = true;
            UpdateSkillPoints();
        }
    }

    public void IncreaseAttackSpeed()
    {
        if (upgraded == false)
        {
            WeaponBehaviour.instance.attackSpeed += 0.01f;
            _animatorA.SetTrigger(Pressed);
            LevelManager.instance.attackSpeedLevel++;
            upgraded = true;
            UpdateSkillPoints();
        }
    }

    public void CloseUpgradePanel()
    {
        upgraded = false;
        panel.SetActive(false);
        continueButton.SetActive(false);
        Time.timeScale = 1f;
    }

    private void UpdateSkillPoints()
    {
        LevelManager.instance.SetUpgradePanel();
        if (upgraded)
        {
            continueButton.SetActive(true);
        }
    }
}