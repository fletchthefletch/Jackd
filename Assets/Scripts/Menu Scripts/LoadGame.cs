using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
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
