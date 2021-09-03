using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip buttonHover;
    public AudioClip buttonClick;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void playButtonClick()
    {
        source.PlayOneShot(buttonClick);
    }
    public void playButtonHover()
    {
        source.PlayOneShot(buttonHover);
    }
}
