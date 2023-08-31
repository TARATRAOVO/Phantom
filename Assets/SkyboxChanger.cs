using System;
using UnityEngine;
     
public class SkyboxChanger : MonoBehaviour 
{
    public bool isNight = false;
    public Material skyboxDay;
    public Material skyboxNight;
    private int index = 0;

    void Start()
    {
        RenderSettings.skybox = skyboxDay;
    }
     
    void Update()
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