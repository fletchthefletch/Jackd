using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip buttonHover;
    public AudioClip buttonClick;

    public void playButtonClick()
    {
        source.PlayOneShot(buttonClick);
    }
    public void playButtonHover()
    {
        source.PlayOneShot(buttonHover);
    }
}
