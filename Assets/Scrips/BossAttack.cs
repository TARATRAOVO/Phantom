using UnityEngine;
using MxM;
using System;

public class BossAttack : Attack
{
    public MxMEventDefinition[] attackEventDefs;
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        TickWeaponCollider();
    }

    public void OnAttack()
    {
        MxMEventDefinition attackEventDef = attackEventDefs[UnityEngine.Random.Range(0, attackEventDefs.Length)];
        mmAnimator.BeginEvent(attackEventDef);

    }

}
