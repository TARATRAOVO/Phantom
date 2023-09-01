using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using MxM;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Health health;
    public MxMAnimator mmAnimator;
    public float originAnimPlaybackSpeed = 0.7f;
    public bool isDeathAnimEnd = false;
    public bool isEvadeAnimEnd = true;
    public bool isEvadeState = false;
    public bool isUped = false;
    public bool isKnockDown = false;
    public float evadeTime = 1.0f;
    public MxMEventDefinition[] deathEvents;
    public MxMEventDefinition[] evadeForwardEvents;
    public MxMEventDefinition[] evadeBackwardEvents;
    public MxMEventDefinition beHitEvent;
    private bool lastDeathState;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = GetComponent<Health>();
        mmAnimator = GetComponent<MxMAnimator>();
        lastDeathState = health.isDead;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        TickState();

        if (health.isDead && !lastDeathState)
        {
            OnDeath();
            lastDeathState = health.isDead;
        };

        if (health.isDead && mmAnimator.CurrentEventState.ToString() == "FollowThrough")
        {
            isDeathAnimEnd = true;
        }

        if (mmAnimator.CurrentEventState.ToString() == "FollowThrough")
        {
            isEvadeAnimEnd = true;
        }


    }
    public void OnDeath()
    {
        mmAnimator.BeginEvent(deathEvents[0]);
    }

    public void OnEvadeForward()
    {
        mmAnimator.BeginEvent(evadeForwardEvents[0]);
        StartCoroutine(Evade());
        isEvadeAnimEnd = false;
    }
    public void OnEvadeBackward()
    {
        mmAnimator.BeginEvent(evadeBackwardEvents[0]);
        StartCoroutine(Evade());
        isEvadeAnimEnd = false;
    }
    public void OnBeHit()
    {
        if (isEvadeState || isUped || isKnockDown || health.isDead)
        {
            return;
        }
        mmAnimator.BeginEvent(beHitEvent);
    }

    public void TickState()
    {
        if (mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            isKnockDown = false;
            isUped = false;
        }

    }

    IEnumerator Evade()
    {
        isEvadeState = true;
        yield return new WaitForSeconds(evadeTime);
        isEvadeState = false;
    }
}
