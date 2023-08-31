using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float maxStunValue = 100f;
    public float maxUpValue = 100f;
    [HideInInspector]public bool isDead = false;
    [HideInInspector]public bool isStuned = false;
    [HideInInspector]public bool isUped = false;
    public float currentHealth;
    public float currentStunValue;
    public float currentUpValue;
    public UnityEvent OnTakenDamage;
    public UnityEvent OnStuned;
    public UnityEvent OnUped;

    // Start is called before the first frame update
    protected void Start()
    {
        currentHealth = maxHealth;
        currentStunValue = maxStunValue;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
        }
        if (currentStunValue <= 0)
        {
            isStuned = true;
            OnStuned.Invoke();
        }
        currentStunValue = maxStunValue;
        isStuned = false;
        if (currentUpValue <= 0)
        {
            isUped = true;
            OnUped.Invoke();
        }
        currentUpValue = maxUpValue;
        isUped = false;

    }
    // take damage
    public void TakeDamage(float damage, float stun, float uped)
    {
        currentHealth -= damage;
        currentStunValue -= stun;
        currentUpValue -= uped;
        OnTakenDamage.Invoke();
    }

}
