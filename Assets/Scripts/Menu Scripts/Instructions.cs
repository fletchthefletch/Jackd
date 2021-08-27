using UnityEngine;

public class Instructions : MonoBehaviour
{
    public GameObject targetObject;

    public void openInstructions()
    {
        GameObject instructionObject = targetObject;
        instructionObject.SetActive(true);
    }
    public void closeInstructions()
    {
        GameObject instructionObject = targetObject;
        instructionObject.SetActive(false);
    }
}
