using System.Collections;
using UnityEngine;

public class FadeScene : MonoBehaviour
{
    private Animator anim;
    [SerializeField] 
    private GameObject obj;
    [SerializeField]
    private float fadeInDuration = 3f;


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
        // Fade scene
        anim = obj.GetComponent<Animator>();
        anim.SetBool("StartNextScene", true);
        yield return null;
    }
    IEnumerator fadeIn()
    {
        // Fade scene
        anim = obj.GetComponent<Animator>();
        anim.SetBool("StartNextScene", false);

        yield return new WaitForSecondsRealtime(fadeInDuration);
    }
}
