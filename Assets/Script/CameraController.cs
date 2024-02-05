using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CameraConfiguration
{
    public float yaw;
    public float pitch;
    public float roll;
    
    public Vector3 pivot;
    public Vector3 distance;
    public float fov;

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(pitch, yaw, roll);
    }

    public Vector3 GetPosition()
    {
        return pivot + distance;
    }
    
    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(pivot, 0.25f);
        Vector3 position = GetPosition();
        Gizmos.DrawLine(pivot, position);
        Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, fov, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;
    }

}

public class CameraController : MonoBehaviour
{
    public Camera camera;
    CameraConfiguration configuration;
    public static CameraController instance;

    private List<AView> activeViews = new List<AView>();
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ApplyConfiguration(camera, configuration);
    }

    //CamConfig
    public void ApplyConfiguration(Camera cam, CameraConfiguration config)
    {
        cam.transform.position = config.GetPosition();
        cam.transform.rotation = config.GetRotation();
        cam.fieldOfView = config.fov;
    }

    void OnDrawGizmos()
    {
        configuration.DrawGizmos(Color.red);
    }
    
    //Active Views

    public void AddView(AView view)
    {
        activeViews.Add(view);
    }
    
    public void RemoveView(AView view)
    {
        activeViews.Remove(view);
    }

    
}
