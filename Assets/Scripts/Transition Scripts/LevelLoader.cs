using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private FadeScene fadeScene;
    [SerializeField] private int preLoadDelay;
    [SerializeField] private int postLoadDelay;

    // Short term method for data persistence
    private static string sceneAfterLoading = "MainDesertScene";
    private static int highScore = 0;

    public static void setSceneAfterLoading(string next)
    {
        sceneAfterLoading = next;
    }
    public static int getHighScore()
    {
        return highScore;
    }
    public static void setHighScore(int score)
    {
        highScore = score;
    }

    public void Start()
    {
        if (preLoadDelay < 1)
        {
            preLoadDelay = 1;
        }
        if (postLoadDelay < 1)
        {
            postLoadDelay = 1;
        }
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        yield return new WaitForSeconds(preLoadDelay);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneAfterLoading, LoadSceneMode.Additive);

        Scene scene = SceneManager.GetSceneByName(sceneAfterLoading);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;

            yield return null;

        }
        if (operation.isDone)
        {
            Debug.Log("Loaded game");

            // Fade current scene
            fadeScene.fadeOutCurrentScene();

            // Unload loading screen
            SceneManager.UnloadSceneAsync("LoadingScene");
        }
        else
        {
            Debug.Log("Failed to load scene");
        }
    }
}
