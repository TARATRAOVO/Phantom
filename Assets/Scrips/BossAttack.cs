using UnityEngine;
using MxM;
using System.Collections;
using System.Collections.Generic;

public class BossAttack : Attack
{
    public float frameFreezeTimeBeforeAttack = 0.4f;
    public List<MxMEventDefinition> attackEventDefs;
    private List<MxMEventDefinition> attackEventDefsLeft;
    public MxMEventDefinition phantomAttackEventDef;
    public MxMEventDefinition invisibleAttackEventDef;
    public MxMEventDefinition fastAttackEventDef;
    public MxMEventDefinition invisibleAttack1EventDef;
    public MxMEventDefinition invisibleAttack2EventDef;
    public MxMEventDefinition invisibleAttack3EventDef;
    public MxMEventDefinition invisibleAttack4EventDef;
    public MxMEventDefinition invisibleAttack5EventDef;
    public MxMEventDefinition invisibleAttack6EventDef;
    public MxMEventDefinition invisibleAttack7EventDef;
    public MxMEventDefinition invisibleAttack8EventDef;
    [HideInInspector] public GameObject bossPhantomLeft;
    [HideInInspector] public GameObject bossPhantomRight;
    [HideInInspector] public GameObject bossPhantomMiddle;
    public List<MxMEventDefinition> phantomAttackEventDefs;
    private bool ifPhantomAttacked = false;
    public bool isSepecialAttack = false;
    private AnimationController bossAnimationController;
    [Header ("Boss State (Read Only)")]
    public bool isVisibleNow = true;

    protected override void Start()
    {
        base.Start();
        attackEventDefsLeft = new List<MxMEventDefinition>(attackEventDefs);
        bossAnimationController = GetComponent<AnimationController>();
    }

    protected override void Update()
    {
        base.Update();

        TickWeaponCollider();

        if (attackEventDefsLeft.Count == 0)
        {
            attackEventDefsLeft = new List<MxMEventDefinition>(attackEventDefs);
        }

        

        if (tag == "BossPhantom" && !ifPhantomAttacked)
        {
            mmAnimator.BeginEvent(phantomAttackEventDefs[0]);
            ifPhantomAttacked = true;
        }

        if (tag == "BossPhantom" && mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            Destroy(gameObject);
            if (GameObject.FindGameObjectsWithTag("BossPhantom").Length == 1)
            {
                skyboxChanger.isNight = false;
            }
        }
        if (tag == "BossPhantom" && !skyboxChanger.isNight)
        {
            Destroy(gameObject);
        }

    }

    public void OnBossAttack()
    {
        int whichAttack = Random.Range(0, attackEventDefsLeft.Count);
        MxMEventDefinition attackEventDef = attackEventDefsLeft[whichAttack];
        attackEventDefsLeft.RemoveAt(whichAttack);
        mmAnimator.BeginEvent(attackEventDef);
    }

    public void OnBossPhantomAttack()
    {
        StartCoroutine(bossAnimationController.Evade());
        isSepecialAttack = true;
        mmAnimator.BeginEvent(phantomAttackEventDef);
        mmAnimator.PlaybackSpeed = 0.5f;
        StartCoroutine(WaitForBossPhantomAttackAnim());
    }

    IEnumerator WaitForBossPhantomAttackAnim()
    {
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        mmAnimator.PlaybackSpeed = originAnimPlaybackSpeed;
        // create phantom after the animation, near the player
        skyboxChanger.isNight = true;
        Vector3 playerPos = player.transform.position;
        Vector3 bossPos = transform.position;
        Vector3 phantomPos = Vector3.Lerp(playerPos, bossPos, 0.1f);
        phantomPos.y = 0;
        bossPhantomLeft = Instantiate(gameObject, phantomPos, transform.rotation);
        bossPhantomLeft.tag = "BossPhantom";
        phantomAttackEventDefs.RemoveAt(0);
        bossPhantomRight = Instantiate(gameObject, phantomPos, transform.rotation);
        bossPhantomRight.tag = "BossPhantom";
        phantomAttackEventDefs.RemoveAt(0);
        bossPhantomMiddle = Instantiate(gameObject, phantomPos, transform.rotation);
        bossPhantomMiddle.tag = "BossPhantom";
        phantomAttackEventDefs.RemoveAt(0);
        isSepecialAttack = false;
    }

    public void OnBossInvisibleAttack()
    {
        StartCoroutine(bossAnimationController.Evade());
        isSepecialAttack = true;
        StartCoroutine(InvisibleAttack());
    }

    public IEnumerator InvisibleAttack()
    {
        mmAnimator.BeginEvent(invisibleAttackEventDef);
        mmAnimator.PlaybackSpeed = 0.2f;
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        mmAnimator.PlaybackSpeed = originAnimPlaybackSpeed;
        isSepecialAttack = true;
        
        SetVisibility(false);
        TeleportBossToPlayerRadius(20.0f);
        yield return new WaitForSeconds(4.3f);
        skyboxChanger.isNight = true;

        // combo
        
        yield return new WaitForSeconds(0.1f);
        SetVisibility(true);
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack1EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        TeleportBossToPlayerRadius(20.0f);
        skyboxChanger.isNight = false;
        yield return new WaitForSeconds(1.0f);
        
        //combo
        skyboxChanger.isNight = true;
        yield return new WaitForSeconds(0.1f);
        SetVisibility(true);
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack2EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        TeleportBossToPlayerRadius(20.0f);
        skyboxChanger.isNight = false;
        yield return new WaitForSeconds(1.0f);

        //combo
        skyboxChanger.isNight = true;
        yield return new WaitForSeconds(0.1f);
        SetVisibility(true);
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack3EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        TeleportBossToPlayerRadius(20.0f);
        skyboxChanger.isNight = false;
        yield return new WaitForSeconds(2.0f);

        
        skyboxChanger.isNight = true;
        yield return new WaitForSeconds(0.2f);
        SetVisibility(true);
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack4EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        TeleportBossToPlayerRadius(20.0f);
        skyboxChanger.isNight = false;
        yield return new WaitForSeconds(0.2f);

        skyboxChanger.isNight = true;
        yield return new WaitForSeconds(0.2f);
        SetVisibility(true);
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack5EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        TeleportBossToPlayerRadius(20.0f);
        skyboxChanger.isNight = false;
        yield return new WaitForSeconds(0.2f);

        skyboxChanger.isNight = true;
        yield return new WaitForSeconds(0.2f);
        SetVisibility(true);
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack6EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        TeleportBossToPlayerRadius(20.0f);
        skyboxChanger.isNight = false;
        yield return new WaitForSeconds(0.2f);

        skyboxChanger.isNight = true;
        yield return new WaitForSeconds(0.2f);
        SetVisibility(true);
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack7EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        TeleportBossToPlayerRadius(20.0f);
        skyboxChanger.isNight = false;
        yield return new WaitForSeconds(0.2f);

        skyboxChanger.isNight = true;
        yield return new WaitForSeconds(0.2f);
        SetVisibility(true);
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack8EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        TeleportBossToPlayerRadius(10.0f);

        yield return new WaitForSeconds(2.0f);
        SetVisibility(true);
        skyboxChanger.isNight = false;
        isSepecialAttack = false;
    }
    void SetVisibility(bool isVisible)
    {
        isVisibleNow = isVisible;
        foreach (SkinnedMeshRenderer meshRenderer in thisMeshRenderers)
        {
            meshRenderer.enabled = isVisible;
        }
    }

    public void TeleportBossToPlayerRadius(float radius = 1.0f)
    {
        
        Vector2 randomPos = Random.insideUnitCircle.normalized * radius;
        Vector3 targetPos = new Vector3(randomPos.x, 0, randomPos.y) + player.transform.position; // update y if necessary
        // Teleport boss (current object)
        transform.position = targetPos;
    }


}
