using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MxM;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class BossAnimationController : AnimationController
{
    public MxMEventDefinition hitUpEvent;

    public MxMEventDefinition hitInAirEvent;
    public MxMEventDefinition knockDownEvent;
    [HideInInspector] public BossAI bossAI;
    [HideInInspector] public NavMeshAgent agent;
    private bool currentUpState;
    private float originY;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        bossAI = GetComponent<BossAI>();
        originY = this.transform.position.y;
        mmAnimator.PlaybackSpeed = originAnimPlaybackSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (currentUpState != isUped)
        {
            OnBossUped();
            currentUpState = isUped;
        }

        if (isUped)
        {
            this.transform.position = new Vector3(this.transform.position.x, 1.2f, this.transform.position.z); 
        }
        else
        {
            math.lerp(this.transform.position.y, originY, 0.01f);
        }


        base.Update();
        if (isDeathAnimEnd)
        {
            OnBossDeathAnimEnd();
        }
        BossTickState();

    }

    public void OnBossDeathAnimEnd()
    {
        agent.enabled = false;
        mmAnimator.enabled = false;
        bossAI.enabled = false;
    }

    public void OnBossUped()
    {
        if (isKnockDown && !health.isDead)
        {
            mmAnimator.BeginEvent(hitUpEvent);
            bossAI.isMoving = false;
            isUped = true;
            mmAnimator.PlaybackSpeed = 0.5f;
        }
    }

    public void OnBossHitInAir()
    {
        if (isUped && !health.isDead)
        {
            mmAnimator.BeginEvent(hitInAirEvent);
            bossAI.isMoving = false;
            isUped = true;
        }
    }

    public void OnBossKnockDown()
    {
        if (!health.isDead)
        {
            mmAnimator.BeginEvent(knockDownEvent);
            bossAI.isMoving = false;
            isKnockDown = true;
        }

    }

    public void BossTickState()
    {
        if (mmAnimator.CurrentEventState.ToString() == "FollowThrough")
        {
            bossAI.isMoving = true;
        }
    }

    public void OnBossUpedEnd()
    {
        OnBossKnockDown();
        mmAnimator.PlaybackSpeed = originAnimPlaybackSpeed;
    }
}
