using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// This class manages the generation of enemy waves, enemy interactions, and wave timekeeping

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private int currentWave;
    [SerializeField]
    private int TimeBetweenWaves = 60;
    [SerializeField]
    private float numberOfWaves = 5f;
    [SerializeField]
    private float waveBannerDisplayTime;
    [SerializeField]
    private Text waveCounterUI;
    [SerializeField]
    private GameObject waveWindow;
    private float nextWaveIn;
    private float currentTime;
    private bool allWavesFinished = false;
    private bool wavesHaventStarted = true;
    private float firstwavePromptDuration = 5f;

    [SerializeField]
    private GameObject waveBannerUI;
    [SerializeField]
    private Text guidePrompt;
    private PlayListCycler playlist;
    private MainGame game;

    // Spawn objects
    public GameObject Cow;
    public GameObject DarkVoidSmall;
    private float voidGenerationDelay = 3f;

    // Enemies
    private List<GameObject> enemies;
    private List<Enemy> enemyScripts;

    [SerializeField]
    private List<GameObject> spawnPointObjects;
    private List<Vector3> spawnPoints;
    private int currentSpawnPointIndex;
    private bool gameIsPaused = false;

    private void Start()
    {
        playlist = FindObjectOfType<PlayListCycler>();
        game = FindObjectOfType<MainGame>();
        enemies = new List<GameObject>();
        enemyScripts = new List<Enemy>();
        spawnPoints = new List<Vector3>();
        currentSpawnPointIndex = 0;

        // Get spawnpoint positions
        foreach (GameObject g in spawnPointObjects)
        {
            spawnPoints.Add(g.transform.position);
        }
    }
    public void setGamePaused(bool isPaused)
    {
        gameIsPaused = isPaused;
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().setGamePaused(isPaused);
        }
        
    }
    public void startFirstWave()
    {
        wavesHaventStarted = false;
        waveWindow.SetActive(true);
        StartCoroutine(showFirstWavePrompt());
    }

    private void checkClosestEnemy()
    {
        Enemy closestEnemy = null;
        float smallestDist = -1f;

        foreach (GameObject g in enemies)
        { 
            Enemy e = g.GetComponent<Enemy>();
            float dist = e.getDistanceToPlayer();
            if (closestEnemy == null)
            {
                // First enemy
                smallestDist = dist;
                closestEnemy = e;
                continue;
            }
            if (dist <= smallestDist)
            {
                // Disable enemy
                closestEnemy.enabled = false;
                closestEnemy = e;
            }
            else
            {
                // Disable enemy
                e.enabled = false;
            }
        }
        if (closestEnemy == null)
        {
            return;
        }

        // Enable closest enemy
        closestEnemy.enabled = true;
    }

    void Update()
    {
        if (wavesHaventStarted)
        {
            return; // Do nothing   
        }

        if (gameIsPaused)
        {
            return;
        }

        // Delete dead enemies
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.Remove(enemies[i]);
            }
        }

        //if (enemies.Count >= 2)
        //{
            checkClosestEnemy();
        //}


        if (currentWave < numberOfWaves)
        {
            updateWaveTime();
        }
        else if (!allWavesFinished) {
            // Check if all the enemies in the final wave are dead
            if (enemies.Count > 0)
            {
                // At least one enemy is still alive
                return;
            }
            
            // This gets called once after the final wave has been generated
            nextWaveIn = 0.0f;
            allWavesFinished = true;
            game.gameObjectives.startNextObjective();
        }
    }

    public bool wavesAreFinished()
    {
        return allWavesFinished;
    }
    private void updateWaveTime()
    {
        currentTime += Time.deltaTime;
        nextWaveIn = TimeBetweenWaves - currentTime;
        waveCounterUI.text = Mathf.Round(nextWaveIn).ToString();
        if (nextWaveIn <= 0.0f)
        {
            // Start wave, and reset wave timer
            currentTime = 0.0f;
            startWave(currentWave);
            StartCoroutine(showWaveBanner(currentWave));

            playlist.playInteractionSound("waveStarting", true);
            currentWave++;
        }
    }
    private IEnumerator showWaveBanner(int waveNum)
    {
        // Show wave banner
        waveBannerUI.SetActive(true);
        // Wait N seconds
        yield return new WaitForSeconds(waveBannerDisplayTime);
        // Hide wave banner
        waveBannerUI.SetActive(false);
    }
    private IEnumerator showFirstWavePrompt()
    {
        // Show wave banner
        guidePrompt.text = "Enemy Waves Preparing...";
        // Wait N seconds
        yield return new WaitForSeconds(firstwavePromptDuration);
        // Hide wave banner
        guidePrompt.text = "";
    }
    private IEnumerator spawnEnemiesForWave(int numOfCows, int numOfBulls)
    {
        // Instantiate cows
        for (int i = 0; i < numOfCows; i++)
        {
            // Dark void
            GameObject voidOb = Instantiate(DarkVoidSmall, spawnPoints[currentSpawnPointIndex], Quaternion.Euler(0, 0, 0));

            // Delay
            yield return new WaitForSecondsRealtime(voidGenerationDelay);

            // Cow
            GameObject cow = Instantiate(Cow, spawnPoints[currentSpawnPointIndex], Quaternion.Euler(0, 0, 0));
            enemies.Add(cow);
            enemyScripts.Add(cow.GetComponent<Enemy>());

            // Destroy void
            yield return new WaitForSecondsRealtime(voidGenerationDelay);

            Destroy(voidOb);

            // Set enemy variables here
            cow.GetComponent<Enemy>().setChaseDepth(0);
            currentSpawnPointIndex++;
        }

        // Instantiate bulls
        for (int i = 0; i < numOfBulls; i++)
        {
            // Dark void
            GameObject voidOb = Instantiate(DarkVoidSmall, spawnPoints[currentSpawnPointIndex], Quaternion.Euler(0, 0, 0));

            // Delay
            yield return new WaitForSecondsRealtime(voidGenerationDelay);

            // Bull
            GameObject bull = null;// Instantiate(Bull, spawnPoints[currentSpawnPointIndex], Quaternion.Euler(0, 0, 0));
            enemies.Add(bull);
            enemyScripts.Add(bull.GetComponent<Enemy>());

            // Destroy void
            yield return new WaitForSecondsRealtime(voidGenerationDelay);

            Destroy(voidOb);
            // Set variables here
            bull.GetComponent<Enemy>().setChaseDepth(1);
            currentSpawnPointIndex++;
        }
  
        yield return null;
    }

    public void startWave(int waveToGenerate)
    {
        switch (waveToGenerate)
        {
            case 0:
                Debug.Log("First Wave Coming...");
                StartCoroutine(spawnEnemiesForWave(1, 0));
                break;
            case 1:
                Debug.Log("Second Wave Coming...");
                StartCoroutine(spawnEnemiesForWave(3, 0));
                break;
            case 2:
                Debug.Log("Third Wave Coming...");
                //StartCoroutine(spawnEnemiesForWave(0, 1));
                break;
            case 3:
                Debug.Log("Fourth Wave Coming...");
                //StartCoroutine(spawnEnemiesForWave(2, 1));
                break;
            case 4:
                Debug.Log("Fifth Wave Coming...");
                //StartCoroutine(spawnEnemiesForWave(1, 2));
                break;
        }
    }
}
