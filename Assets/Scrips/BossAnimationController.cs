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
    private BossAttack bossAttack;
    private Transform playerTransform;
    private CapsuleCollider capsuleCollider;
    private float originCapsulRadius;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        bossAI = GetComponent<BossAI>();
        originY = this.transform.position.y;
        mmAnimator.PlaybackSpeed = originAnimPlaybackSpeed;
        bossAttack = GetComponent<BossAttack>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        originCapsulRadius = capsuleCollider.radius;

    }

    // Update is called once per frame
    protected override void Update()
    {
        mmAnimator.AddRequiredTag("Air");
        if (currentUpState != isUped)
        {
            OnBossUped();
            currentUpState = isUped;
        }

        if (isUped)
        {
            this.transform.position = new Vector3(this.transform.position.x, 1.2f, this.transform.position.z);
            capsuleCollider.radius = 1.5f;

        }
        else
        {
            math.lerp(this.transform.position.y, originY, 0.01f);
            capsuleCollider.radius = originCapsulRadius;

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
        this.enabled = false;
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
        mmAnimator.PlaybackSpeed = originAnimPlaybackSpeed;
    }

    public void OnBossEvadeBackward()
    {
        print("OnEvadeBackward");
        bossAttack.LookAtTarget(playerTransform.gameObject);
        mmAnimator.BeginEvent(evadeBackwardEvents[1]);
        StartCoroutine(Evade());
        StartCoroutine(WaitForEvade());
        isEvadeAnimEnd = false;   
    }
    IEnumerator WaitForEvade()
    {
        mmAnimator.PlaybackSpeed = 1.0f;
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        mmAnimator.PlaybackSpeed = originAnimPlaybackSpeed;
        StartCoroutine(Evade());
        OnBossAttackEvadeBackward();
        isEvadeAnimEnd = false;
        
    }
    public void OnBossAttackEvadeBackward()
    {
        bossAttack.LookAtTarget(playerTransform.gameObject);
        mmAnimator.BeginEvent(bossAttack.fastAttackEventDef);
        StartCoroutine(Evade());
        StartCoroutine(WaitForAttack());
    }


    IEnumerator WaitForAttack()
    {
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        mmAnimator.BeginEvent(evadeBackwardEvents[0]);
        Vector3 direction = (this.transform.position - playerTransform.position).normalized;
        direction.y = 0;
        isEvadeAnimEnd = false;
    }



    IEnumerator MoveToDestionation(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, destination, 0.2f*Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator LookAt(Vector3 target)
    {
        while (Vector3.Angle(transform.forward, target - transform.position) > 0.1f)
        {
            transform.forward = Vector3.Lerp(transform.forward, target - transform.position, 0.1f);
            yield return null;
        }
    }

}
