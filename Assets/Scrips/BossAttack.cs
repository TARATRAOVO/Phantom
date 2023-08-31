using UnityEngine;
using MxM;
using System;
using System.Collections;

public class BossAttack : Attack
{
    public MxMEventDefinition[] attackEventDefs;
    public MxMEventDefinition phantomAttackEventDef;
    [HideInInspector]public GameObject bossPhantomLeft;
    [HideInInspector]public GameObject bossPhantomRight;
    private GameObject player;
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    protected override void Update()
    {

        base.Update();
        TickWeaponCollider();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnBossPhantomAttack();
        }

        if (this.tag == "BossPhantom" && mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            Destroy(gameObject);
            skyboxChanger.isNight = false;
        }
        if (this.tag == "BossPhantom" && !skyboxChanger.isNight)
        {
            Destroy(gameObject);
        }

    }

    public void OnBossAttack()
    {
        MxMEventDefinition attackEventDef = attackEventDefs[UnityEngine.Random.Range(0, attackEventDefs.Length)];
        mmAnimator.BeginEvent(attackEventDef);
    }

    public void OnBossPhantomAttack()
    {
        mmAnimator.BeginEvent(phantomAttackEventDef);
        StartCoroutine(WaitForBossPhantomAttackAnim());
    }

    IEnumerator WaitForBossPhantomAttackAnim()
    {
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        // create phantom after the animation, near the player
        skyboxChanger.isNight = true;
        Vector3 playerPos = player.transform.position;
        Vector3 bossPos = transform.position;
        Vector3 phantomPos = new Vector3(playerPos.x + (bossPos.x - playerPos.x) / 2, bossPos.y, playerPos.z + (bossPos.z - playerPos.z) / 2);
        bossPhantomLeft = Instantiate(gameObject, phantomPos, transform.rotation);
        bossPhantomRight = Instantiate(gameObject, phantomPos, transform.rotation);
        bossPhantomLeft.tag = "BossPhantom";
        bossPhantomRight.tag = "BossPhantom";
    }

    

}
