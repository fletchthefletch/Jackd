using UnityEngine;
using UnityEngine.UI;

public class DetectPlayerBeanstalk : MonoBehaviour
{
    [SerializeField]
    private Text guidePrompt;
    private MainGame game;
    [SerializeField]
    private Collider mainStalkColliderInteractive;
    [SerializeField]
    private Collider mainStalkCollider;
    [SerializeField]
    private Collider stalkBaseCollider;
    [SerializeField]
    private GameObject dirtPlatform;

    private string objectiveTag = "beanstalk";

    private void Start()
    {
        game = FindObjectOfType<MainGame>();
        
        // Local collision ignorance
        Physics.IgnoreCollision(mainStalkColliderInteractive, stalkBaseCollider, true);
        Physics.IgnoreCollision(mainStalkColliderInteractive, mainStalkCollider, true);
        Physics.IgnoreCollision(stalkBaseCollider, mainStalkCollider, true);

        Collider[] dirtColliders = dirtPlatform.GetComponents<Collider>();

        // Dirt platform collision ignorance
        foreach (Collider c in dirtColliders)
        {
            Physics.IgnoreCollision(c, mainStalkCollider, true);
            Physics.IgnoreCollision(c, mainStalkColliderInteractive, true);
            Physics.IgnoreCollision(c, stalkBaseCollider, true);
        }
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
            guidePrompt.text = "Hold 'F' to Climb Beanstalk";
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
            // Perform climb here
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Detect the players height here
                guidePrompt.text = "";
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
    /*
     * Do the below from within the objectives class
     *
    // Check if beanstalk has finished growing
    if (!handler.isBeanStalkFullyGrown())
    {
        return;
    }
    // Check if waves have finished
    if (!enemyManager.wavesAreFinished())
    {
        return;
    }
    // TODO: Check if player has killed all the enemies

        */

}
