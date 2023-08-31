using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Slider slider;
    public Health targetHealth;
    void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        if (targetHealth != null)
        {
            slider.value = targetHealth.currentHealth / targetHealth.maxHealth;
        }
    }

    
}
