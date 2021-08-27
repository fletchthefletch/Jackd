using System.Collections;
using UnityEngine;

public class FadeScene : MonoBehaviour
{
    private Animator anim;
    [SerializeField] 
    private GameObject obj;

    private void Start()
    {
        fadeInCurrentScene();
    }
    public void fadeOutCurrentScene()
    {
        StartCoroutine(fadeOut());
    }

    public void fadeInCurrentScene()
    {
        StartCoroutine(fadeIn());
    }
    IEnumerator fadeOut()
    {
        // Start game close animation
        // Fade scene
        anim = obj.GetComponent<Animator>();
        anim.SetBool("StartNextScene", true);

        yield return null;
    }
    IEnumerator fadeIn()
    {
        // Start game close animation
        // Fade scene
        anim = obj.GetComponent<Animator>();
        anim.SetBool("StartNextScene", false);

        yield return null;
    }
}
