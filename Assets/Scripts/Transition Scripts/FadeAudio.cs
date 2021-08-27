using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAudio : MonoBehaviour
{
    public float duration;
    public GameObject musicObject;

    // This class fades all music belonging to a particular gameobject
    public void fadeInAudio()
    {
        // Fade music
        if (musicObject == null)
        {
            musicObject = GameObject.Find("Scene Music");
        }
        AudioSource[] audioSources = musicObject.GetComponents<AudioSource>();
        foreach (AudioSource a in audioSources) 
        {
            StartCoroutine(fade(0f, a, duration, 1));
        }

    }

    public void fadeOutAudio()
    {
        // Fade music
        if (musicObject == null)
        {
            musicObject = GameObject.Find("Scene Music");
        }
        AudioSource[] audioSources = musicObject.GetComponents<AudioSource>();
        foreach (AudioSource a in audioSources)
        {
            StartCoroutine(fade(a.volume, a, duration, 0));
        }

    }

    IEnumerator fade(float start, AudioSource audioSource, float duration, float targetVolume)
    {

        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

}
