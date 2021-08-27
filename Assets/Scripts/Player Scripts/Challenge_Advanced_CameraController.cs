using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge_Advanced_CameraController : MonoBehaviour
{
   //Target to follow
    [SerializeField] private Transform m_Target;

     //Pivot Transform to Look up down
    [SerializeField] private Transform m_CameraPivot;

    //Offset for better control
    [SerializeField] private Vector3 m_offset; 

    //Camera distance from player
    [SerializeField] private float m_distFromTarget;

    //Left-Right, Up-Down Rotate sensitivity
    [SerializeField] private float m_SensitivityX, m_SensitivityY;

    //Clamp camera movement in Up-Down direction 
    [SerializeField] private float m_minY, m_maxY;

    //Up-Down, Left-Right angles
    private float m_angleY, m_angleX;

    //Camera rotation speed
    [SerializeField] private float m_RotationSpeed;

    //Speed at which the camera smoothly reaches its target position
    [SerializeField] private float m_cameraSmoothSpeed;

    //Only for reference parameter in the smoothing function
    private Vector3 m_cameraVelocity = Vector3.zero;

    //Whether the camera should always look at target
    [SerializeField] private bool lookAtFollow = false;

    //Smooth or Snap follow movement
    [SerializeField] private bool smoothFollow = false;

    //Smooth or Snap rotate movement
    [SerializeField] private bool smoothRotate = false;

    private Camera m_camera;    

    [SerializeField]
    private PauseMenu pauseMenu;


    void Awake()
    {
        m_camera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (pauseMenu.isOpen())
        {
            return; // Do not update player rotation and state
        }
        GetInputs();  
    }


    //We use LateUpdate because it is called after Update() where animations and computations logic resides
    //Since we want the camera to follow after all the computations have been carried out
   void LateUpdate()
   {
        if (lookAtFollow)
        {
            FollowCameraLookAtTarget();
        }
        else
        {
            FollowCameraFixedDistance();
        }

        if (smoothRotate)
        {
            RotateCameraSmooth();
        }
        else
        {
            RotateCameraSnap();
        }
   }
   
   
   private void FollowCameraFixedDistance()
   {    
       
        // Direction from camera --> target (so that the camera always stays focused on target even if we rotate)  
        // How far the camera stays from the player
        Vector3 targetPos = m_Target.position - transform.forward * m_distFromTarget;

        if (smoothFollow)
        {
            // Smooth from camera position --> target position using the follow speed
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_cameraVelocity, m_cameraSmoothSpeed);
        }
        else
        {
            transform.position = targetPos;
        }
   }

   private void FollowCameraLookAtTarget()
   {
        
        Vector3 targetPos = m_Target.position - (Quaternion.Euler(m_angleY, m_angleX, 0) * m_offset);

        if(smoothFollow)
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_cameraVelocity, m_cameraSmoothSpeed);
        else
            transform.position = targetPos;

        transform.LookAt(m_Target);
   }
   

    private void RotateCameraSmooth()
    {     
        Quaternion targetRotation = Quaternion.Euler(m_angleY, m_angleX, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);

    }
     
    private void RotateCameraSnap()
    {
     
        //Look left - right on parent transform
        Vector3 rotation = Vector3.zero;
        rotation.y = m_angleX;
        var targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        //Look up down on pivot transform
        rotation = Vector3.zero;
        rotation.x = m_angleY;
        targetRotation = Quaternion.Euler(rotation);
        m_CameraPivot.localRotation = targetRotation;

       

    }

   private void GetInputs()
   {
         //Up / Down rotation
        m_angleY -= Input.GetAxis("Mouse Y") * m_SensitivityY;

        //Clamp it so that we cannot rotate up or down beyond a certain limit such as below the ground
        m_angleY = Mathf.Clamp(m_angleY, m_minY, m_maxY);

        //Left / Right Rotation with 360 deg freedom
        m_angleX += Input.GetAxis("Mouse X") * m_SensitivityX;
   }
}
