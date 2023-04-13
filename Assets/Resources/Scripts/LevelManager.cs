using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public int nEnemies;
    public GameObject[] enemyType;
    public float spawnInterval;
    public bool isFinalWave;
    public bool isBigWave;
}
public class LevelManager : MonoBehaviour
{
    [Header("Sun")]
    public TMP_Text sunText;
    public int sun;
    public static int sunAmount;
    public GameObject sunPrefab;
    public float waitToSpawnSun = 5f;

    [Header("Wave System")]
    public Wave[] waves;
    public Transform[] spawnPoints;
    private Wave currentWave;
    private int currentWaveNumber;
    public bool canSpawn = true;
    private float nextSpawnTime;
    public Slider waveStatus;
    public bool canStart;

    [Header("Big Wave Warning")]
    public TMP_Text warning;
    public GameObject warningHUD;

    float currentVelocity = 0;
    public float waitToUpdate = 3f;

    public string bigWaveText;
    public string finalWaveText;

    void Start()
    {
        warningHUD.SetActive(false);

        canStart = true;
        sunAmount = sun;

    }

    // Update is called once per frame
    void Update()
    {
        var position = new Vector2(Random.Range(-4.46f, 5.14f), Random.Range(6f, 5.14f));

        waitToSpawnSun -= Time.deltaTime;

        sunText.SetText(sunAmount.ToString());

        currentWave = waves[(int)currentWaveNumber];

        SpawnWave();
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemie");

        if(totalEnemies.Length == 0 && !canSpawn && currentWaveNumber+1 != waves.Length)
        {
            currentWaveNumber++;
            canSpawn = true;
        }

        float currentWaveProgress = Mathf.SmoothDamp(waveStatus.value, currentWaveNumber + 1, ref currentVelocity, 100 * Time.deltaTime);

        if(waitToUpdate <= 0)
        {
            waveStatus.value = currentWaveProgress;
        }

        if(currentWave.isFinalWave == true)
        {
            warningHUD.SetActive(true);
            StartCoroutine(FinalWave());
        }

        if(currentWave.isBigWave == true)
        {
            warningHUD.SetActive(true);
            StartCoroutine(BigWave());
        }

    }

    public void SpawnWave()
    {
        if (canStart)
        {
            if(canSpawn && nextSpawnTime < Time.time)
            {
                GameObject randomEnemy = currentWave.enemyType[Random.Range(0, currentWave.enemyType.Length)];
                Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(randomEnemy, randomSpawn.position, Quaternion.identity);
                currentWave.nEnemies--;
                nextSpawnTime = Time.time + currentWave.spawnInterval;
                if(currentWave.nEnemies == 0)
                {
                    canSpawn = false;
                }
            }
        }
    }

    IEnumerator BigWave()
    {
        yield return new WaitForSeconds(0.5f);
        warning.SetText(bigWaveText);
        currentWave.isBigWave = false;

        yield return new WaitForSeconds(4.9f);
        warningHUD.SetActive(false);

    }

    IEnumerator FinalWave()
    {
        yield return new WaitForSeconds(0.5f);
        warning.SetText(finalWaveText);
        currentWave.isBigWave = false;

        yield return new WaitForSeconds(4.9f);
        warningHUD.SetActive(false);

    }
}
