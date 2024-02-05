using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedView : AView
{
    public float pitch;
    public float yaw;
    public float roll;
    public float fov;

    public override CameraConfiguration GetConfiguration()
    {
        camConfig.pitch = pitch;
        camConfig.yaw = yaw;
        camConfig.roll = roll;
        
        camConfig.fov = fov;

        camConfig.pivot = transform.position;
        camConfig.distance = Vector3.zero;

        return camConfig;
    }
}
