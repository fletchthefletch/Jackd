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
    private float timeUntilExitAfterVictory;
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

    void Start()
    {
        playlist = FindObjectOfType<PlayListCycler>();
        playlist.playNextSongInPlaylist();
        fadeScene.fadeInCurrentScene();
        player = FindObjectOfType<Player>();
        gameObjectives = new Objectives();
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (player.takeDamage(0.15f))
            {
                Debug.Log("Player is still alive");
            }
            else
            {
                Debug.Log("Player is dead!");
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            gameObjectives.startNextObjective();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.heal(0.05f);
            Debug.Log("Healing player...");
        }
    }

    public IEnumerator playerHasWon()
    {
        waveBannerUI.SetActive(true);
        // Show victory banner
        Color temp =  waveBannerImage.color;
        waveBannerImage.color = victoryColor;
        waveBannerText.text = "Victory!";
        playlist.playInteractionSound("victory", false);

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
