using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMenu : MonoBehaviour
{
    [SerializeField] 
    private int timeUntilExitingStarts;
    [SerializeField]
    private FadeScene fader;
    [SerializeField]
    private FadeAudio faderAud;

    public void exitToMenu()
    {
        faderAud.fadeOutAudio();
        fader.fadeOutCurrentScene();
        LevelLoader.setSceneAfterLoading("MainMenuScene");

        // Load loading screen 
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);

        Debug.Log("Returning to menu...");
    }
}
