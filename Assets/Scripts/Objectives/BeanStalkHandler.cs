using UnityEngine;

public class BeanStalkHandler : MonoBehaviour
{
    // Beanstalk management
    [SerializeField]
    private GameObject beanstalk;
    public AnimationCurve scaleValue;
    public AnimationCurve posValue;
    [SerializeField]
    private float beanstalkMaxSize;
    [SerializeField]
    private float beanstalkStartingSize;
    [SerializeField]
    private float timeUntilBeanStalkIsFullyGrown = 300;
    [SerializeField]
    private float beanTime;
    private float beanTimeRate; // How much to increment time each frame
    private float beanstalkStartingPositionY = -0.94f;
    private float beanstalkEndPositionY = -30.0f;
    private bool beanIsGrowing = false;
    private bool fullyGrown = false;

    void updateBean()
    {
        float scaleAmount = scaleValue.Evaluate(beanTime);
        float posAmount = posValue.Evaluate(beanTime);

        beanstalk.transform.localScale =
            new Vector3(scaleAmount, scaleAmount, scaleAmount);
        beanstalk.transform.position =
            new Vector3(beanstalk.transform.position.x, posAmount, beanstalk.transform.position.z);
    }
    public void startGrowingBeans()
    {
        beanIsGrowing = true;
    }
    public bool isBeanStalkFullyGrown()
    {
        return fullyGrown;
    }

    void Start()
    {
        // Beanstalk management
        beanTime = 0f;
        beanTimeRate = 1.0f / timeUntilBeanStalkIsFullyGrown;

        // Size keys
        scaleValue.MoveKey(0, new Keyframe(0f, beanstalkStartingSize));
        scaleValue.MoveKey(1, new Keyframe(1f, beanstalkMaxSize));

        // Position keys
        posValue.MoveKey(0, new Keyframe(0f, beanstalkStartingPositionY));
        posValue.MoveKey(1, new Keyframe(1f, beanstalkEndPositionY));
    }

    void Update()
    {
        if (beanIsGrowing && !fullyGrown)
        {
            beanTime += beanTimeRate * Time.deltaTime;
            updateBean();
            if (beanTime >= 1f)
            {
                // Bean is fully grown
                fullyGrown = true;
            }
        }
    }

}
