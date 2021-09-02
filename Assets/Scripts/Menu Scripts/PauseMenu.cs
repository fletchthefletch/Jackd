using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip buttonHover;
    [SerializeField]
    private Slider backgroundMusicSlider;
    [SerializeField]
    private Slider FXSlider;
    [SerializeField]
    private Text scoreText;
    private MainGame game;
    [SerializeField]
    public Text objectiveText;
    public PlayListCycler playlist;
    private bool menuIsOpen = false;
    private EnemyManager eManager;

    void Start()
    {
        playlist = FindObjectOfType<PlayListCycler>();
        game = FindObjectOfType<MainGame>();
        eManager = FindObjectOfType<EnemyManager>();
    }

    public bool isOpen() {
        return menuIsOpen;
    }

    public void closePauseMenu()
    {
        // Make menu inactive
        Cursor.visible = false;
        menu.SetActive(false);
        menuIsOpen = false;
        playlist.playInteractionSound("closingMenuSound", true);
        playlist.fadeInSoundsExcept("closingMenuSound", 1f);

        // Unfreeze enemies and waves
        eManager.setGamePaused(false);
    }

    public void openPauseMenu()
    {
        // Make menu active
        if (playlist == null)
        {
            playlist = FindObjectOfType<PlayListCycler>();
        }
        if (eManager == null)
        {
            eManager = FindObjectOfType<EnemyManager>();
        }
        if (game == null)
        {
            game = FindObjectOfType<MainGame>();
        }

        // Freeze enemies and waves
        eManager.setGamePaused(true);

        playlist.playInteractionSound("openingMenuSound", true);
        playlist.fadeOutSoundsExcept("openingMenuSound", 1f);

        // Get current stats / values
        backgroundMusicSlider.value = playlist.getCurrentBackgroundMusicVolume();
        FXSlider.value = playlist.getCurrentFXVolume();

        // Update player values
        objectiveText.text = game.gameObjectives.getCurrentObjective();
        scoreText.text = game.player.getPlayerScore().ToString(); 

        menu.SetActive(true);
        menuIsOpen = true;
        Cursor.visible = true;
    }

    public void playButtonHover() 
    {
        audioSource.PlayOneShot(buttonHover);   
    }
}
