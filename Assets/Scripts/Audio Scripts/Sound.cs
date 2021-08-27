using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public string soundClass;

    public AudioClip clip;

    [Range(0f, 1.0f)]
    public float volume;
    [Range(.1f, 20f)]
    public float pitch;

    [HideInInspector]
    public AudioSource mySource;

}
