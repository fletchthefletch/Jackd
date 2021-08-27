using System.Collections;
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

    private void Start()
    {
        playlist = FindObjectOfType<PlayListCycler>();
    }
    public void startFirstWave()
    {
        wavesHaventStarted = false;
        waveWindow.SetActive(true);
        StartCoroutine(showFirstWavePrompt());
        
    }
    void Update()
    {
        if (wavesHaventStarted)
        {
            return; // Do nothing   
        }
        if (currentWave < numberOfWaves)
        {
            updateWaveTime();

        }
        else if (!allWavesFinished) {
            // This gets called once after the final wave has been generated
            nextWaveIn = 0.0f;
            allWavesFinished = true;
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
