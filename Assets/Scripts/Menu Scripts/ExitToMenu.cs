using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMenu : MonoBehaviour
{
    [SerializeField]
    private float timeUntilExit = 3f;
    [SerializeField]
    private FadeScene fader;
    [SerializeField]
    private FadeAudio faderAud;

    public void exitToMenu()
    {
        StartCoroutine(exitBackToMenu());
    }
    private IEnumerator exitBackToMenu()
    {
        faderAud.fadeOutAudio();
        fader.fadeOutCurrentScene();
        LevelLoader.setSceneAfterLoading("MainMenuScene");

        // Delay
        yield return new WaitForSecondsRealtime(timeUntilExit);
        
        // Load loading screen 
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);

        Debug.Log("Returning to menu...");
    }
}
