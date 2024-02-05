using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AView : MonoBehaviour
{
    protected CameraConfiguration camConfig = new CameraConfiguration();
    public float weight;
    public bool isActiveOnStart;

    public void Start()
    {
        if (isActiveOnStart)
        {
            SetActive(isActiveOnStart);
        }
    }

    public virtual CameraConfiguration GetConfiguration()
    {
        return camConfig;
    }

    public void SetActive(bool isActive)
    {
        
    }
}
