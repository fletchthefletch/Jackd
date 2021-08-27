using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayListCycler : MonoBehaviour
{

    public Sound[] sounds;
    private List<float> soundRevertVolume = new List<float>();
    private List<Sound> mainPlayList = new List<Sound>();
    private List<Sound> interactionSounds = new List<Sound>();
    private List<Sound> playerSounds = new List<Sound>();

    private int currentSongIndex = 0;

    private float currentBackgroundMusicvolume = 1f;
    private float currentFXvolume = 1f;

    
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.mySource = gameObject.AddComponent<AudioSource>();
            s.mySource.clip = s.clip;
            s.mySource.pitch = s.pitch;
           

            if (s.soundClass == "MAIN")
            {
                // Sound is background music
                s.mySource.volume = s.volume * currentBackgroundMusicvolume;
                mainPlayList.Add(s);
            }
            else if (s.soundClass == "PLAYER")
            {
                // Sound is related to the player
                s.mySource.volume = s.volume * currentFXvolume;
                playerSounds.Add(s);
            }
            else 
            {
                // Sound is related to interaction
                s.mySource.volume = s.volume * currentFXvolume;
                interactionSounds.Add(s);
            }
            soundRevertVolume.Add(s.mySource.volume);

        }
    }
    public float getCurrentBackgroundMusicVolume()
    {
        return currentBackgroundMusicvolume;
    }
    public float getCurrentFXVolume()
    {
        return currentFXvolume;
    }

    public void setCurrentBackgroundMusicVolume(float value)
    {
        currentBackgroundMusicvolume = value;
    }
    public void setCurrentFXVolume(float value)
    {
        currentFXvolume = value;
    }


    public void playNextSongInPlaylist()
    {
        if (mainPlayList[currentSongIndex].mySource.isPlaying)
        {
            mainPlayList[currentSongIndex].mySource.Stop();
        }

        currentSongIndex = (currentSongIndex + 1) % mainPlayList.Count;
        
        mainPlayList[currentSongIndex].mySource.Play();
    }

    public void playPlayerSound(int index, bool oneShot)
    {
        if (index >= playerSounds.Count || index < 0)
        {
            Debug.Log("Audio clip index beyond range");
            return;
        }
        if (oneShot)
        {
            playerSounds[index].mySource.PlayOneShot(playerSounds[index].mySource.clip);
        }
        else
        {
            playerSounds[index].mySource.Play();
        }
    }
    public void playPlayerSound(string name, bool oneShot)
    {
        Sound s = playerSounds.Find(x => x.name == name);
        if (oneShot)
        {
            if (s != null)
            {
                s.mySource.PlayOneShot(s.clip);
            }
        }
        else
        {
            s.mySource.Play();
        }
    }

    public void playInteractionSound(string name, bool oneShot)
    {
        Sound s = interactionSounds.Find(x => x.name.Equals(name));
        if (s == null)
        {
            Debug.Log("Could not play interaction sound");
            return;
        }
        if (oneShot)
        {
            s.mySource.PlayOneShot(s.clip);
            return;
        }
        else
        {
            s.mySource.Play();
        }
    }


    public void fadeOutSoundsExcept(string soundName, float duration)
    {
        // Fade out music
        foreach (Sound s in sounds)
        {
            if (!s.name.Equals(name)) 
            {
                if (s.soundClass.Equals("MAIN"))
                {
                    // Adjust background music
                    StartCoroutine(fade(s.mySource.volume, s.mySource, duration, 0f));
                }
                else
                {
                    StartCoroutine(fade(s.mySource.volume, s.mySource, duration, 0f));
                }
            }
        }
    }
    public void fadeInSoundsExcept(string soundName, float duration)
    {
        // Fade in music
        int i = 0;
        foreach (Sound s in sounds)
        {
            if (!s.name.Equals(name))
            {
                if (s.soundClass.Equals("MAIN"))
                {
                    // Adjust background music
                    StartCoroutine(fade(s.mySource.volume, s.mySource, duration, soundRevertVolume[i] * currentBackgroundMusicvolume));
                }
                else 
                {
                    StartCoroutine(fade(s.mySource.volume, s.mySource, duration, soundRevertVolume[i] * currentFXvolume));
                }
            }
            i++;
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
