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
    public static CameraController instance;
    public Camera camera;
    
    //CamConfig
    public CameraConfiguration actualConfig;
    public CameraConfiguration targetConfig;
    public bool moveTheCam;
    public float speed;
   

    private List<AView> activeViews = new List<AView>();
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        
        foreach (var view in GetComponents<AView>())
        {
            AddView(view);
        }
    }

    private void Start()
    {
        
    }

    void Update()
    {
        if (activeViews.Count == 0)
        {
            print("y a rienne");
        }
        MoveCamToTarget(actualConfig, targetConfig);
        ApplyConfiguration(camera, actualConfig);
    }

    //CamConfig
    public void ApplyConfiguration(Camera cam, CameraConfiguration config)
    {
        if (!moveTheCam)
        {
            config.pitch = ComputeAveragePitch();
            config.yaw = ComputeAverageYaw();
            config.roll = ComputeAverageRoll();
            config.fov = ComputeAverageFOV();
        }
        
        
        cam.transform.position = config.GetPosition();
        cam.transform.rotation = config.GetRotation();
        cam.fieldOfView = config.fov;
    }
    
    public float ComputeAverageFOV()
    {
        float sum = 0;
        float sumWeight = 0;
        for (int i = 0; i < activeViews.Count; i++)
        {
            CameraConfiguration config = activeViews[i].GetConfiguration();
            sum += (config.fov * activeViews[i].weight);
            sumWeight += activeViews[i].weight;
        }
        
        return sum/sumWeight;
    }
 
    public float ComputeAverageYaw()
    {
        Vector2 sum = Vector2.zero;
        foreach (AView view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            sum += new Vector2(Mathf.Cos(config.yaw * Mathf.Deg2Rad),
                Mathf.Sin(config.yaw * Mathf.Deg2Rad)) * view.weight;
        }
        return Vector2.SignedAngle(Vector2.right, sum);
    }
    
    public float ComputeAveragePitch()
    {
        Vector2 sum = Vector2.zero;
        foreach (AView view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            sum += new Vector2(Mathf.Cos(config.pitch * Mathf.Deg2Rad),
                Mathf.Sin(config.pitch * Mathf.Deg2Rad)) * view.weight;
        }
        return Vector2.SignedAngle(Vector2.right, sum);
    }
    
    public float ComputeAverageRoll()
    {
        Vector2 sum = Vector2.zero;
        foreach (AView view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            sum += new Vector2(Mathf.Cos(config.roll * Mathf.Deg2Rad),
                Mathf.Sin(config.roll * Mathf.Deg2Rad)) * view.weight;
        }
        return Vector2.SignedAngle(Vector2.right, sum);
    }
    
    //Smoothing
    private void MoveCamToTarget(CameraConfiguration actual, CameraConfiguration target)
    {
        
        if (speed * Time.deltaTime < 1)
        {
            actual.pivot += (target.pivot - actual.pivot) * (speed * Time.deltaTime);
        }
        else
        {
            actual.pivot = target.pivot;
        }
        
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

    //Gizmo

    private void OnDrawGizmos()
    {
        actualConfig.DrawGizmos(Color.green);
        if (moveTheCam)
        {
            targetConfig.DrawGizmos(Color.red);
        }
    }
}
