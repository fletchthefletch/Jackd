using UnityEngine;

public class Objective
{
    private string label;
    private string tag;
    private int pointsForCompleting;
    public Objective(string label, int pointsForCompleting, string tag)
    {
        this.label = label;
        this.tag = tag;
        this.pointsForCompleting = pointsForCompleting;
    }
    public string getLabel()
    {
        return this.label;
    }
    public string getTag()
    {
        return this.tag;
    }
    public int getPointsForCompleting()
    {
        return this.pointsForCompleting;
    }
}
