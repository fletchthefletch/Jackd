using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField]
    private Slider healthbar;
    [SerializeField]
    private Image healthBarColourRect;
    [SerializeField]
    private Color goodHealthColor;
    [SerializeField]
    private Color okayHealthColor;
    [SerializeField]
    private Color badHealthColor;
    [SerializeField]
    private Text moneyText;

    // Start is called before the first frame update
    void Start()
    {
        float tempHealth = 1f;
        // Get player health first
        sethealthbar(tempHealth);
    }
    public void sethealthbar(float value)
    {
        // Set value
        healthbar.value = value;

        // Update colour
        if (value >= 0.75f) {
            healthBarColourRect.color = goodHealthColor;
        }
        else if (value < 0.75f && value >= 0.4f)
        {
            healthBarColourRect.color = okayHealthColor;
        }
        else
        {
            healthBarColourRect.color = badHealthColor;
        }
    }

    public void setMoney(int money)
    {
        moneyText.text = "$" + money.ToString() + ".00";
    }
}
