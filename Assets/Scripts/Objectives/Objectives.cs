using System.Collections.Generic;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    private List<Objective> objectives = new List<Objective>();

    private int currentObjective;
    [SerializeField]
    private GameObject beans;
    private MainGame game;

    private void Start()
    {
        game = FindObjectOfType<MainGame>();
    }

    public Objectives()
    {
        // Add game objectives
        objectives.Add(new Objective("Plant magic beans", 100, "bean"));
        objectives.Add(new Objective("Fight off waves", 300, "waves"));
        objectives.Add(new Objective("Climb beanstalk!", 100, "beanstalk"));
        currentObjective = 0;
    }

    public string getCurrentObjective()
    {
        if (currentObjective >= objectives.Count)
        {
            return "All objectives completed";
        }
        else
        {
            return objectives[currentObjective].getLabel();
        }
    }
    public int getCurrentObjectivePointsForCompleting()
    {
        return objectives[currentObjective].getPointsForCompleting();
    }
    public string getCurrentObjectiveTag()
    {
        if (currentObjective >= objectives.Count)
        {
            return "";
        }
        else
        {
            return objectives[currentObjective].getTag();
        }
    }
    public void plantBeans()
    {
        // Start waves
        FindObjectOfType<EnemyManager>().startFirstWave();

        // Destroy beans
        GameObject bean1 = GameObject.Find("bean1");
        GameObject bean2 = GameObject.Find("bean2");
        GameObject bean3 = GameObject.Find("bean3");

        Destroy(bean1);
        Destroy(bean2);
        Destroy(bean3);

        // Update current objective
        startNextObjective();

        // Start growing beanstalk
        FindObjectOfType<BeanStalkHandler>().startGrowingBeans();
    }

    public bool startNextObjective() // Returns true if all objectives are complete
    {
        if (currentObjective >= objectives.Count)
        {
            // Player has won

            return true;
        }
        else 
        {
            currentObjective = currentObjective + 1;
            return false;
        }
    }
}
