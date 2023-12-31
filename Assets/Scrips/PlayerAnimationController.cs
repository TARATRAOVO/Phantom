using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    public GameObject enemy;
    private Camera mainCamera;

    protected override void Start()
    {
        base.Start();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        mainCamera = Camera.main;
    }
    protected override void Update()
    {
        base.Update();

        if (tag != "Player")
        {
            return;
        }

        if (isDeathAnimEnd)
        {
            OnPlayerDeathAnimEnd();
        }

        if ((Input.GetKeyDown(KeyCode.Space) && IsInputAxis()) && isEvadeAnimEnd && health.isDead == false)
        {
            LookAtInput();
            print("space");
            OnEvadeForward();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isEvadeAnimEnd && health.isDead == false)
        {
            print("space");
            LookAtInput();
            OnEvadeBackward();
        }
    }

    void OnPlayerDeathAnimEnd()
    {
        mmAnimator.enabled = false;
    }

    public void OnPlayerBeHit()
    {
        OnBeHit();
        if (isEvadeState || isUped || isKnockDown || health.isDead || this.tag != "Player")
        {
            return;
        }
        LookAtEnemy();
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

    void LookAtInput()
    {
        // transform input direction to world space by main camera
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 inputDirectionWorldSpace = mainCamera.transform.TransformDirection(inputDirection);
        inputDirectionWorldSpace.y = 0;
        transform.forward = inputDirectionWorldSpace;
    }

    public bool IsInputAxis()
    {
        return (Math.Abs((Input.GetAxis("Vertical"))) > 0 || Math.Abs((Input.GetAxis("Horizontal"))) > 0);
    }


}
