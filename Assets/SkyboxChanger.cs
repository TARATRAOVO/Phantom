using System;
using UnityEngine;
     
public class SkyboxChanger : MonoBehaviour 
{
    public bool isNight = false;
    public Material skyboxDay;
    public Material skyboxNight;

    protected void Start()
    {
        RenderSettings.skybox = skyboxDay;
    }
     
    protected void Update()
    {
        if (isNight)
        {
            RenderSettings.skybox = skyboxNight;
        }
        else
        {
            RenderSettings.skybox = skyboxDay;
        }
    }
}