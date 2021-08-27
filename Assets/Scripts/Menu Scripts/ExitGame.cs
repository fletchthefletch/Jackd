using System.Collections;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public int timeToExit;

    public void exitGame() 
    {
        StartCoroutine(closeGame()); 

    }
  
        IEnumerator closeGame()
    {
        yield return new WaitForSeconds(timeToExit);
        Debug.Log("Exiting game...");
        Application.Quit();
        yield return null;
    }

}
