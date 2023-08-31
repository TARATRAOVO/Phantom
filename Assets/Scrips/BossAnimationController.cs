using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MxM;
using UnityEngine;
using UnityEngine.AI;

public class BossAnimationController : AnimationController
{
    public MxMEventDefinition hitUpEvent;
    public MxMEventDefinition hitInAirEvent;
    public MxMEventDefinition knockDownEvent;
    [HideInInspector]public BossAI bossAI;
    [HideInInspector]public NavMeshAgent agent;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        bossAI = GetComponent<BossAI>();
    }

    // Update is called once per frame
    protected override void Update()
    {
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
        if (isKnockDown)
        {
            mmAnimator.BeginEvent(hitUpEvent);
            bossAI.isMoving = false;
            isUped = true;
        }
    }

    public void OnBossHitInAir()
    {
        if (isUped)
        {
            mmAnimator.BeginEvent(hitInAirEvent);
            bossAI.isMoving = false;
            isUped = true;
        }
    }

    public void OnBossKnockDown()
    {
        mmAnimator.BeginEvent(knockDownEvent);
        bossAI.isMoving = false;
        isKnockDown = true;
    }

    public void BossTickState()
    {
        if (mmAnimator.CurrentEventState.ToString() == "FollowThrough")
        {
            bossAI.isMoving = true;
        }
    }
}
