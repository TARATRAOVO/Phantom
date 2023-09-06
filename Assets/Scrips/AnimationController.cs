using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using MxM;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [HideInInspector]public Health health;
    [HideInInspector]public MxMAnimator mmAnimator;
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
    private Attack attack;
    private int lastEventId;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        health = GetComponent<Health>();
        mmAnimator = GetComponent<MxMAnimator>();
        lastDeathState = health.isDead;
        attack = GetComponent<Attack>();
        lastEventId = attack.currentEventId;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        TickState();

        if (health.isDead && !lastDeathState)
        {
            OnDeath();
            attack.enabled = false;
            lastDeathState = health.isDead;
        };

        if (health.isDead && mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            isDeathAnimEnd = true;
        }

        if (mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            isEvadeAnimEnd = true;
        }

        // OnEventComplete
        if (lastEventId != attack.currentEventId && lastEventId != 0)
        {
        }
        lastEventId = attack.currentEventId;
    }
    public void OnDeath()
    {
        mmAnimator.BeginEvent(deathEvents[0]);
        isDeathAnimEnd = false;
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

    public IEnumerator Evade()
    {
        isEvadeState = true;
        yield return new WaitForSeconds(evadeTime);
        isEvadeState = false;
    }
}
