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

    void Start()
    {
        playlist = FindObjectOfType<PlayListCycler>();
        game = FindObjectOfType<MainGame>();
    }

    public bool isOpen() {
        return menuIsOpen;
    }

    public void closePauseMenu()
    {
        // Make menu inactive
        menu.SetActive(false);
        menuIsOpen = false;
        playlist.playInteractionSound("closingMenuSound", true);
        playlist.fadeInSoundsExcept("closingMenuSound", 1f);
    }

    public void openPauseMenu()
    {
        // Make menu active
        if (playlist == null)
        {
            playlist = FindObjectOfType<PlayListCycler>();
        }
   
        playlist.playInteractionSound("openingMenuSound", true);
        playlist.fadeOutSoundsExcept("openingMenuSound", 1f);

        // Get current stats / values
        backgroundMusicSlider.value = playlist.getCurrentBackgroundMusicVolume();
        FXSlider.value = playlist.getCurrentFXVolume();

        if (game == null)
        {
            game = FindObjectOfType<MainGame>();
            if (game == null)
            {
                Debug.Log("Could not locate game");
            }
        }

        // Update player values
        objectiveText.text = game.gameObjectives.getCurrentObjective();
        scoreText.text = game.player.getPlayerScore().ToString(); 

        menu.SetActive(true);
        menuIsOpen = true;            
    }

    public void playButtonHover() 
    {
        audioSource.PlayOneShot(buttonHover);   
    }
}
