using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

public class FixedFollowView : AView
{


    public float roll;
    public float fov;
    
    public Vector3 target;

    [SerializeField] private GameObject centralPoint;
    [SerializeField] private float yawOffsetMax;
    [SerializeField] private float ptichOffsetMax;
    
    
    void Start()
    {
        Vector3 dir = target - transform.position;
        Vector3 centaleDir = centralPoint.transform.position - transform.position;
        
        camConfig.yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        camConfig.pitch = (Mathf.Asin(dir.y) * -1) * Mathf.Rad2Deg;
    }


    public override CameraConfiguration GetConfiguration()
    {
        Vector3 dir = target - transform.position;
        Vector3 centraleDir = centralPoint.transform.position - transform.position;
        float yawOffset = Vector3.SignedAngle(centraleDir, dir, Vector3.up);
        yawOffset = Mathf.Clamp(yawOffset, -yawOffsetMax, yawOffsetMax);
        
        float centralYaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        camConfig.yaw = centralYaw + yawOffset;
        return camConfig;
    }

   
    
}
