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
    private PlayerController playerCon;
    private BeanStalkHandler beanStalkhandler;

    private string objectiveTag = "beanstalk";

    private bool isClimbing = false;
    private void Start()
    {
        game = FindObjectOfType<MainGame>();
        beanStalkhandler = FindObjectOfType<BeanStalkHandler>();

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

        // Get player
        playerCon = FindObjectOfType<PlayerController>();
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
            guidePrompt.text = "Hold 'F' to Climb Beanstalk";
            // Perform climb here
            if (Input.GetKey(KeyCode.F))
            {
                // Detect the players height here
                // Perform climb here
                guidePrompt.text = "Keep Going!";
                // Detect the players height here
                if (!isClimbing)
                {
                    isClimbing = true;
                    playerCon.startStopClimbing(isClimbing);
                }
            }
            else 
            {
                isClimbing = false;
                playerCon.startStopClimbing(isClimbing);
            }
        }
    }
    private bool checkObjective(string obj)
    {
        if (game.gameObjectives.getCurrentObjectiveTag().Equals(obj))
        {
            if (beanStalkhandler.isBeanStalkFullyGrown())
            {
                return true;
            }
        }
        return false;
    }
}
