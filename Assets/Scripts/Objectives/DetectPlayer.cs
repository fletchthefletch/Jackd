using UnityEngine;
using UnityEngine.UI;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField]
    private Text guidePrompt;
    private MainGame game;
    private string objectiveTag = "bean";

    private void Start()
    {
        game = FindObjectOfType<MainGame>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!checkObjective(objectiveTag))
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            // Player is walking on dirt platform
            guidePrompt.text = "Press 'F' to Plant Magic Beans";
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!checkObjective(objectiveTag))
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            // Player is leaving on dirt platform
            guidePrompt.text = "";
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!checkObjective(objectiveTag))
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.F))
            {
                // Plant beans
                game.playerCompletedObjective();
            }
        }
    }
    private bool checkObjective(string obj)
    {
        if (game.gameObjectives.getCurrentObjectiveTag().Equals(obj))
        {
            return true;
        }
        return false;
    }
}
