using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainCanvas : MonoBehaviour
{
    [SerializeField]
    private Text highScoreText;

    [SerializeField]
    private List<Image> leftIcons = new List<Image>();
    [SerializeField]
    private List<Image> rightIcons = new List<Image>();

    [SerializeField]
    private Color defaultIconcolor;
    [SerializeField]
    private Color hoverIconcolor;

    void Start()
    {
        highScoreText.text = LevelLoader.getHighScore().ToString();
    }
    public void decorateButton1(int id)
    {
        for (int i = 0; i < leftIcons.Count; i++) // This is only acceptable beacuse we know the arrays are corresponding
        {
            if (i == id)
            {
                leftIcons[i].color = hoverIconcolor;
                rightIcons[i].color = hoverIconcolor;
            }
            else
            {
                leftIcons[i].color = defaultIconcolor;
                rightIcons[i].color = defaultIconcolor;
            }
        }
    }
    public void unDecorateButton(int id)
    { 
        leftIcons[id].color = defaultIconcolor;
        rightIcons[id].color = defaultIconcolor;
    }
    public void decorateButton(int id)
    {
        leftIcons[id].color = hoverIconcolor;
        rightIcons[id].color = hoverIconcolor;
    }
}
