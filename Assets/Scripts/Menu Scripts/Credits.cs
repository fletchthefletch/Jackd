using UnityEngine;

public class Credits : MonoBehaviour
{
    public GameObject targetObject;
    public void openCredits()
    {
        GameObject instructionObject = targetObject;
        instructionObject.SetActive(true);
    }
    public void closeCredits()
    {
        GameObject instructionObject = targetObject;
        instructionObject.SetActive(false);
    }
}
