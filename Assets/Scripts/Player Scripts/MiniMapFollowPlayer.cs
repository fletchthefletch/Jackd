using UnityEngine;

public class MiniMapFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject mainCharacter;

    void LateUpdate()
    {
        float x = mainCharacter.transform.position.x;
        float z = mainCharacter.transform.position.z;
        transform.position = new Vector3(x, 130, z);
    }
}
