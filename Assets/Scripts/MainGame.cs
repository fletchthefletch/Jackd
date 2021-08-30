using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    [SerializeField] private FadeAudio audioFader;
    [SerializeField] private FadeScene sceneFader;
    private static PlayListCycler playlist;
    [SerializeField] 
    private FadeScene fadeScene;
    [SerializeField]
    private PauseMenu pauseMenu;
    [SerializeField]
    public Player player;
    public Objectives gameObjectives;
    [SerializeField]
    private Text moneyText;
    [SerializeField]
    private float timeUntilExitAfterVictory = 7f;
    [SerializeField]
    private float timeUntilExitAfterDefeat = 4f;
    [SerializeField]
    private ExitToMenu exiter;
    [SerializeField]
    private GameObject waveBannerUI;
    [SerializeField]
    private Image waveBannerImage;
    [SerializeField]
    private Text waveBannerText;
    [SerializeField]
    private Color victoryColor;
    [SerializeField]
    private Color failureColor;
    private bool playerDefeated = false;



    void Start()
    {
        playlist = FindObjectOfType<PlayListCycler>();
        playlist.playNextSongInPlaylist();
        player = FindObjectOfType<Player>();
        gameObjectives = FindObjectOfType<Objectives>();
        exiter = FindObjectOfType<ExitToMenu>();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Show pause menu
            if (!pauseMenu.isActiveAndEnabled)
            {
                pauseMenu.openPauseMenu();
            }
            else
            {
                // Hide pause menu
                pauseMenu.closePauseMenu();
            }
        }

        // Check player health - if player health is <= 0, player has lost the game
        if (player.getPlayerHealth() <= 0f)
        {
            if (!playerDefeated)
            {
                StartCoroutine(playerHasLost());
                playerDefeated = true;
            }
        }
    }
    private IEnumerator playerHasLost()
    {
        waveBannerUI.SetActive(true);
        // Show victory banner
        Color temp = waveBannerImage.color;
        waveBannerImage.color = failureColor;
        waveBannerText.text = "- Defeat -";
        playlist.playInteractionSound("Defeat", true);
        playlist.fadeOutSoundsExcept("Defeat", 1f);

        // Wait N seconds
        yield return new WaitForSeconds(timeUntilExitAfterDefeat);
        exiter.exitToMenu();
        waveBannerImage.color = temp;
        waveBannerUI.SetActive(false);
    }

    private IEnumerator playerHasWon()
    {
        waveBannerUI.SetActive(true);
        // Show victory banner
        Color temp =  waveBannerImage.color;
        waveBannerImage.color = victoryColor;
        waveBannerText.text = "Victory!";
        playlist.playInteractionSound("victory", true);
        playlist.fadeOutSoundsExcept("victory", 2f);

        // Wait N seconds
        yield return new WaitForSeconds(timeUntilExitAfterVictory);
        exiter.exitToMenu();
        waveBannerImage.color = temp;
        waveBannerUI.SetActive(false);
    }
    public void playerCompletedObjective() 
    {
        int scoreIncrement = gameObjectives.getCurrentObjectivePointsForCompleting();
        switch (gameObjectives.getCurrentObjectiveTag())
        {
            case "bean":
                gameObjectives.plantBeans();
                break;
            case "waves":
                //gameObjectives.fightWaves();
                break;
            case "beanstalk":
                gameObjectives.startNextObjective();
                if (gameObjectives.startNextObjective())
                {
                    StartCoroutine(playerHasWon());
                }
                break;
        }

        // Increase player score 
        player.setPlayerScore(scoreIncrement);
    }
}
