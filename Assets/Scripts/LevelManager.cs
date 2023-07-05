using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
internal struct Waves
{
    [SerializeField] public int waveCounter;
    [SerializeField] public float time;
    [SerializeField] public int amountOfGreenSlime, amountOfFireSlime, amountOfBrute;
    [SerializeField] public bool finished;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject greenSlime, brute, fireSlime;
    [SerializeField] private Text timerText;
    [SerializeField] public GameObject deathScreen;
    [SerializeField] private float cooldownToSpawn, cooldownToSpawnReset;
    public static LevelManager instance;
    [SerializeField] private GameObject spawner;
    private int wave;
    [SerializeField] private Waves[] waves;
    public List<EnemyBehaviour> enemies;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Text vitality, damage, waitToFire, movementSpeed;

    private void Awake()
    {
        instance = this;
        cooldownToSpawnReset = cooldownToSpawn;
    }

    private void Update()
    {
        DisplayTimeCounter();
        CanSpawn();
    }

    private IEnumerator RandomSpawn(int waveCounter)
    {
        var randomLocation = new Vector3(Random.Range(-17, 17), Random.Range(9, -9), 0f);
        var newSpawner = Instantiate(spawner, randomLocation, Quaternion.identity);
        Destroy(newSpawner, 1f);

        yield return new WaitForSeconds(1f);

        if (waves[waveCounter].amountOfGreenSlime > 0)
        {
            var newGreenSlime = Instantiate(greenSlime, randomLocation, Quaternion.identity);
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

        if (waves[waveCounter].amountOfGreenSlime == 0 && waves[waveCounter].amountOfFireSlime == 0 &&
            waves[waveCounter].amountOfBrute == 0)
        {
            waves[waveCounter].finished = true;
        }

        if (enemies.Count == 0)
        {
            waves[waveCounter].finished = true;
            upgradePanel.SetActive(true);
            vitality.text = "Current Vitality: " + PlayerBehaviour.instance.maxHealth;
            damage.text = "Current Damage: " + WeaponBehaviour.instance.damage;
            waitToFire.text = "Attack Speed: " + WeaponBehaviour.instance.cooldownToShoot;
            movementSpeed.text = "Current Speed: " + PlayerBehaviour.instance.moveSpeed;
            Time.timeScale = 0f;
        }
    }

    private void CanSpawn()
    {
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

    public void TryAgain()
    {
        SceneManager.LoadScene("Level");
        Time.timeScale = 1f;
    }
}