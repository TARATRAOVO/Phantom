using UnityEngine;
using MxM;
using UnityEngine.AI;
using UnityEngine.Events;

public class BossWeaponSettings : WeaponSettings
{    
    [Header("Status (Read Only))")]
    [SerializeField] private MxMAnimator mmAnimator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private BossAI bossAI;

    protected override void Awake()
    {
        base.Awake();
        // weapon holder is the ultimate parent of the weapon
        mmAnimator = weaponHolder.GetComponent<MxMAnimator>();
        agent = weaponHolder.GetComponent<NavMeshAgent>();
        bossAI = weaponHolder.GetComponent<BossAI>();
    }

    public void LoseFunction()
    {
        mmAnimator.enabled = false;
        agent.enabled = false;
        bossAI.enabled = false;
        attack.enabled = false;
    }
    public void GainFunction()
    {
        mmAnimator.enabled = true;
        agent.enabled = true;
        bossAI.enabled = true;
        attack.enabled = true;
    }

}
