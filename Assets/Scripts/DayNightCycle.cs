using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time; // 0.5 = midday, 0 = 12:00am, 1 = 11:59pm

    public float fullDayLength; 
    public float startTime = 0.4f; // When the scene will begin at
    public float timeRate; // How much to increment time each frame
    public Vector3 noon; // Rotation of the sun at noon

    [Header("Sun / Moon")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve lightIntensityMultiplier;

    void Start()
    {
        // Day management
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    void Update()
    {
        // Increment time
        time += timeRate * Time.deltaTime;
        if (time >= 1f)
        {
            // A new day begins
            time = 0.0f;
        }

        // Change colour
        sun.color = sunColor.Evaluate(time);
        // Change sun intensity
        //RenderSettings.ambientIntensity = lightIntensityMultiplier.Evaluate(time); // Perhaps a unity bug? Enabling this reduces enemy speed
    }
}
