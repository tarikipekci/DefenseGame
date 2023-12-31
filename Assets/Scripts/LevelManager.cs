using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Serializable]
internal struct Waves
{
    [SerializeField] public int waveCounter;

    [SerializeField] public int countOfGroupElements;

    [SerializeField] public float groupSpawnDuration, resetDuration;

    [SerializeField] public int amountOfGreenSlime,
        amountOfFireSlime,
        amountOfBrute,
        amountOfDarkTablet,
        amountOfImp,
        amountOfBug;


    public int CountEnemies()
    {
        return amountOfGreenSlime + amountOfFireSlime + amountOfBrute + amountOfDarkTablet + amountOfImp + amountOfBug;
    }
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject brute, fireSlime, darkTablet, imp, bug;
    [SerializeField] private Text timerText;
    [SerializeField] public GameObject deathScreen;
    [SerializeField] private float cooldownToSpawn, cooldownToSpawnReset;
    public static LevelManager instance;
    [SerializeField] private GameObject spawner;
    private int wave, currentEnemyCount;
    [SerializeField] private Waves[] waves;
    public List<EnemyBehaviour> enemies;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] public Text vitality, damage, attackSpeed, movementSpeed, currentPoint;
    [HideInInspector] public float vitalityLevel = 1f, damageLevel = 1f, attackSpeedLevel = 1f, movementSpeedLevel = 1f;

    [SerializeField] private TextMeshProUGUI waveCounterText;

    [SerializeField] public float vitalityIncreaseAmount,
        damageIncreaseAmount,
        attackSpeedIncreaseAmount,
        movementSpeedIncreaseAmount;

    [SerializeField] private Text enemyCounterText;
    [SerializeField] private GameObject anvilShadow, anvil, anvilTrace;
    [SerializeField] private float anvilDropSpeed, anvilShadowAnimationDuration = 1f;
    private bool anvilDropped;
    private int groupCounter;
    private bool groupTime;
    private Vector2 dropLocation;
    private GameObject newAnvil, newShadow;

    private void Awake()
    {
        instance = this;
        cooldownToSpawnReset = cooldownToSpawn;
        currentEnemyCount = waves[wave].CountEnemies();
        enemyCounterText.text = currentEnemyCount.ToString();
        waveCounterText.text = "Wave " + waves[wave].waveCounter;
    }

    public IEnumerator DropAnvil()
    {
        var dropLocation = new Vector2(Random.Range(-51, 51), Random.Range(28, -28));
        if (Mathf.Abs(Vector2.Distance(PlayerBehaviour.instance.transform.position, dropLocation)) < 10f)
        {
            var newAnvilShadow = Instantiate(anvilShadow, dropLocation, Quaternion.identity);
            yield return new WaitForSeconds(anvilShadowAnimationDuration);
            var newAnvil = Instantiate(anvil, new Vector2(dropLocation.x, 45), Quaternion.identity);
            SpawnAnvil(dropLocation, newAnvil, newAnvilShadow);
        }
        else
        {
            StartCoroutine(DropAnvil());
        }
    }

    private void SpawnAnvil(Vector2 dropLocation, GameObject newAnvil, GameObject newShadow)
    {
        this.newAnvil = newAnvil;
        this.dropLocation = dropLocation;
        this.newShadow = newShadow;
    }

    private void MoveTowards(Vector2 dropLocation, GameObject newAnvil, Object shadow)
    {
        if (!newAnvil.transform.position.Equals(dropLocation))
        {
            newAnvil.transform.position =
                Vector2.MoveTowards(newAnvil.transform.position, dropLocation, anvilDropSpeed * Time.deltaTime);
        }

        if (newAnvil.transform.position.Equals(dropLocation))
        {
            anvilDropped = false;
            Destroy(shadow);
            var anvilTraceCopy = Instantiate(anvilTrace, new Vector3(dropLocation.x, dropLocation.y - 1f),
                Quaternion.identity);
            Destroy(newAnvil, 0.5f);
            Destroy(anvilTraceCopy, 7f);
        }
    }

    public bool GetAnvilDropped()
    {
        return anvilDropped;
    }

    public void SetAnvilDropped(bool dropped)
    {
        anvilDropped = dropped;
    }

    private void Update()
    {
        DisplayTimeCounter();
        CanSpawn();
        if (anvilDropped)
        {
            MoveTowards(dropLocation, newAnvil, newShadow);
        }
    }

    private IEnumerator RandomSpawn(int waveCounter)
    {
        if (groupTime)
        {
            groupCounter++;
            if (groupCounter == waves[waveCounter].countOfGroupElements)
            {
                groupTime = false;
                cooldownToSpawn = cooldownToSpawnReset;
                groupCounter = 0;
                waves[waveCounter].groupSpawnDuration = waves[waveCounter].resetDuration;
            }
        }

        var randomLocation = new Vector3(Random.Range(-50, 50), Random.Range(27, -27), 0f);

        if (waves[waveCounter].CountEnemies() > 0f)
        {
            var newSpawner = Instantiate(spawner, randomLocation, Quaternion.identity);
            Destroy(newSpawner, 1f);

            yield return new WaitForSeconds(1f);
        }

        if (waves[waveCounter].amountOfGreenSlime > 0)
        {
            //var newGreenSlime = Instantiate(greenSlime, randomLocation, Quaternion.identity);
            var newGreenSlime = ObjectPooling.instance.GetPooledGreenSlimes();
            if (newGreenSlime != null)
            {
                newGreenSlime.transform.position = randomLocation;
                newGreenSlime.SetActive(true);
            }

            waves[waveCounter].amountOfGreenSlime--;
            enemies.Add(newGreenSlime.GetComponent<EnemyBehaviour>());
        }

        if (waves[waveCounter].amountOfFireSlime > 0)
        {
            var newFireSlime = Instantiate(fireSlime, randomLocation, Quaternion.identity);
            waves[waveCounter].amountOfFireSlime--;
            enemies.Add(newFireSlime.GetComponent<EnemyBehaviour>());
        }

        if (waves[waveCounter].amountOfBrute > 0)
        {
            var newBrute = Instantiate(brute, randomLocation, Quaternion.identity);
            waves[waveCounter].amountOfBrute--;
            enemies.Add(newBrute.GetComponent<EnemyBehaviour>());
        }

        if (waves[waveCounter].amountOfDarkTablet > 0)
        {
            var newDarkTablet = Instantiate(darkTablet, randomLocation, Quaternion.identity);
            waves[waveCounter].amountOfDarkTablet--;
            enemies.Add(newDarkTablet.GetComponent<EnemyBehaviour>());
        }

        if (waves[waveCounter].amountOfImp > 0)
        {
            var newImp = Instantiate(imp, randomLocation, Quaternion.identity);
            waves[waveCounter].amountOfImp--;
            enemies.Add(newImp.GetComponent<EnemyBehaviour>());
        }

        if (waves[waveCounter].amountOfBug > 0)
        {
            var newBug = Instantiate(bug, randomLocation, Quaternion.identity);
            waves[waveCounter].amountOfBug--;
            enemies.Add(newBug.GetComponent<EnemyBehaviour>());
        }

        SetUpgradePanel();
        if (enemies.Count == 0)
        {
            wave++;
            currentEnemyCount = waves[wave].CountEnemies();
            waveCounterText.text = "Wave " + waves[wave].waveCounter;
        }

        enemyCounterText.text = currentEnemyCount.ToString();
    }

    public void ReturnHowManyEnemiesLeft()
    {
        --currentEnemyCount;
        enemyCounterText.text = currentEnemyCount.ToString();
    }

    public void SetUpgradePanel()
    {
        if (enemies.Count == 0)
        {
            upgradePanel.SetActive(true);
            UIBehaviour.instance.GetPauseButton().SetActive(false);
            vitality.text = "Current Vitality: " + vitalityLevel + "\nNext Level: " +
                            (vitalityLevel + 1);
            damage.text = "Current Damage: " + damageLevel + "\nNext Level: " +
                          (damageLevel + 1);
            attackSpeed.text = "Current Attack Speed: " + attackSpeedLevel + "\nNext Level: " +
                               (attackSpeedLevel + 1);
            movementSpeed.text = "Current Speed: " + movementSpeedLevel + "\nNext Level: " +
                                 (movementSpeedLevel + 1);
            Time.timeScale = 0f;
        }
    }

    private void CanSpawn()
    {
        if (waves[wave].groupSpawnDuration > 0 && groupTime == false)
        {
            waves[wave].groupSpawnDuration -= Time.deltaTime;
        }
        else
        {
            cooldownToSpawn = 0f;
            groupTime = true;
        }

        if (cooldownToSpawn > 0)
        {
            cooldownToSpawn -= Time.deltaTime;
        }
        else
        {
            StartCoroutine(RandomSpawn(wave));
            cooldownToSpawn = cooldownToSpawnReset;
        }
    }

    private void DisplayTimeCounter()
    {
        float minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
        float seconds = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void SetBaseStats(float vitality, float damage, float attackSpeed, float moveSpeed)
    {
        vitalityLevel = vitality;
        damageLevel = damage;
        attackSpeedLevel = attackSpeed;
        movementSpeedLevel = moveSpeed;
    }

    public void UpdateStats()
    {
        PlayerBehaviour.instance.maxHealth = (int)(vitalityLevel * vitalityIncreaseAmount);
        PlayerBehaviour.instance.currentHealth = PlayerBehaviour.instance.maxHealth;
        PlayerBehaviour.instance.healthBar.maxValue = PlayerBehaviour.instance.maxHealth;
        WeaponBehaviour.instance.damage = damageLevel * damageIncreaseAmount;
        WeaponBehaviour.instance.attackSpeed = attackSpeedLevel * attackSpeedIncreaseAmount;
        PlayerBehaviour.instance.moveSpeed = movementSpeedLevel * movementSpeedIncreaseAmount;
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Level");
        Time.timeScale = 1f;
        UIBehaviour.instance.GetPauseButton().SetActive(true);
    }
}