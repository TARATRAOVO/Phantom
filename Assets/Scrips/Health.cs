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
    [HideInInspector]private bool boolStunded = false;
    [HideInInspector]private bool boolUped = false;
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
            boolStunded = true;
            OnStuned.Invoke();
        }
        currentStunValue = maxStunValue;
        boolStunded = false;

        if (currentUpValue <= 0)
        {
            boolUped = true;
            OnUped.Invoke();
        }
        currentUpValue = maxUpValue;
        boolUped = false;

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
