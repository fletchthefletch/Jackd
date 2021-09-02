using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
#pragma warning disable CS0108 
    public AudioClip audio;
    public AudioSource source;

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(playSound(sceneIndex));
    }

    IEnumerator playSound(int sceneIndex) 
    {
        // Play 'next scene' sound
        source.PlayOneShot(audio);

        // Wait for end of clip
        yield return new WaitForSeconds(audio.length);

        // Load next scene
        LevelLoader.setSceneAfterLoading("MainDesertScene");
        SceneManager.LoadScene(1, LoadSceneMode.Single); // Load loading scene, and remove this scene
    }
}
