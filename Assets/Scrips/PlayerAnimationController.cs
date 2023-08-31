using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    public GameObject enemy;
    protected override void Start()
    {
        base.Start();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }
    protected override void Update()
    {
        base.Update();
        if (isDeathAnimEnd)
        {
            OnPlayerDeathAnimEnd();
        }

        if ((Input.GetKeyDown(KeyCode.Space) && IsInputAxis()) && isEvadeAnimEnd)
        {
            OnEvadeForward();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isEvadeAnimEnd)
        {
            LookAtEnemy();
            OnEvadeBackward();
        }
    }

    void OnPlayerDeathAnimEnd()
    {
        mmAnimator.enabled = false;
    }

    void LookAtEnemy() // if there is an enemy, look at the enemy
    {
        Vector3 attackLookAt = transform.forward;
        if (enemy)
        {
            attackLookAt = enemy.transform.position - transform.position;
        }
        attackLookAt.y = 0;
        transform.forward = attackLookAt;
    }

    public bool IsInputAxis()
    {
        return (Math.Abs((Input.GetAxis("Vertical"))) > 0 || Math.Abs((Input.GetAxis("Horizontal"))) > 0);
    }
    

}