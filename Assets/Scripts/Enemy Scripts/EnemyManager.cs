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
    private int TimeBetweenWaves;
    [SerializeField]
    private float numberOfWaves;
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


    public GameObject Cow;

    // Enemies
    private List<Enemy> enemies = new List<Enemy>();

    private void Start()
    {
        playlist = FindObjectOfType<PlayListCycler>();
        game = FindObjectOfType<MainGame>();
    }
    public void startFirstWave()
    {
        wavesHaventStarted = false;
        waveWindow.SetActive(true);
        StartCoroutine(showFirstWavePrompt());
        
    }

    // Deletecode
    private bool toggle = false;

    void Update()
    {
        // Deletecode
        if (Input.GetKey(KeyCode.Q))
        {
            if (!toggle)
            {
                Debug.Log("make cow");
                Vector3 posit = new Vector3(-279.992f, 0f, 33f);
                Instantiate(Cow, posit, Quaternion.Euler(0, 0, 0));
                //enemies.Add(new Enemy(0));
                toggle = true;
            }
        }

        if (wavesHaventStarted)
        {
            return; // Do nothing   
        }
        if (currentWave < numberOfWaves)
        {
            updateWaveTime();

        }
        else if (!allWavesFinished) {
            // Check if all the enemies in the final wave are dead

            // This gets called once after the final wave has been generated
            nextWaveIn = 0.0f;
            allWavesFinished = true;
            game.gameObjectives.startNextObjective();
        }
    }

    // Deletecode
    public void destroyEnemy(int id)
    {
        /*
        Enemy enemy = enemies.Find(e => e.id == id);
        if (enemy == null)
        {
            Debug.Log("Could not destroy enemy object");
            return;
        }
        else
        {
            // Destroy enemy object
        }
        */
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

    public void startWave(int waveToGenerate)
    {
        switch (waveToGenerate)
        {
            case 0:
                // Create 1 cow
                Debug.Log("First Wave Coming...");
                break;
            case 1:
                // Create 3 cows
                Debug.Log("Second Wave Coming...");
                break;
            case 2:
                // Create 5 cows
                Debug.Log("Third Wave Coming...");
                break;
            case 3:
                // Create 1 bull
                Debug.Log("Fourth Wave Coming...");
                break;
            case 4:
                // Create 1 bull
                Debug.Log("Fifth Wave Coming...");
                break;
        }
    }
}
