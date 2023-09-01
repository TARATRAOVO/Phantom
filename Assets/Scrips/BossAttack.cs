using UnityEngine;
using MxM;
using System.Collections;

public class BossAttack : Attack
{
    public MxMEventDefinition[] attackEventDefs;
    public MxMEventDefinition phantomAttackEventDef;
    public MxMEventDefinition invisibleAttack1EventDef;
    public MxMEventDefinition invisibleAttack2EventDef;
    public MxMEventDefinition invisibleAttack3EventDef;
    public MxMEventDefinition invisibleAttack4EventDef;
    public MxMEventDefinition invisibleAttack5EventDef;
    [HideInInspector] public GameObject bossPhantomLeft;
    [HideInInspector] public GameObject bossPhantomRight;
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {

        base.Update();

        TickWeaponCollider();

        if (Input.GetKeyDown(KeyCode.P))
        {
            OnBossInvisibleAttack();
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
        mmAnimator.PlaybackSpeed = 0.5f;
        print(mmAnimator.PlaybackSpeed);
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
        bossPhantomRight = Instantiate(gameObject, phantomPos, transform.rotation);
        bossPhantomLeft.tag = "BossPhantom";
        bossPhantomRight.tag = "BossPhantom";
    }

    public void OnBossInvisibleAttack()
    {
        StartCoroutine(InvisibleAttack());
    }

    IEnumerator InvisibleAttack()
    {
        SetVisibility(false);
        yield return new WaitForSeconds(1.0f);

        SetVisibility(true);
        skyboxChanger.isNight = true;
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack1EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        skyboxChanger.isNight = false;
        yield return new WaitForSeconds(1.0f);

        SetVisibility(true);
        skyboxChanger.isNight = true;
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack2EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        skyboxChanger.isNight = false;
        yield return new WaitForSeconds(1.0f);

        SetVisibility(true);
        skyboxChanger.isNight = true;
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack3EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        skyboxChanger.isNight = false;
        yield return new WaitForSeconds(0.3f);

        SetVisibility(true);
        skyboxChanger.isNight = true;
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack4EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        skyboxChanger.isNight = false;
        yield return new WaitForSeconds(0.3f);

        SetVisibility(true);
        skyboxChanger.isNight = true;
        TeleportBossToPlayerRadius();
        LookAtTarget(player);
        mmAnimator.BeginEvent(invisibleAttack5EventDef);
        while (!mmAnimator.CurrentEventState.Equals(EEventState.FollowThrough))
        {
            yield return null;
        }
        SetVisibility(false);
        skyboxChanger.isNight = false;

    }
    void SetVisibility(bool isVisible)
    {
        foreach (SkinnedMeshRenderer meshRenderer in thisMeshRenderers)
        {
            meshRenderer.enabled = isVisible;
        }
    }

    private void TeleportBossToPlayerRadius()
    {
        // Get random position on circle with radius 2
        Vector2 randomPos = Random.insideUnitCircle.normalized * 1.0f;
        Vector3 targetPos = new Vector3(randomPos.x, 0, randomPos.y) + player.transform.position; // update y if necessary
        // Teleport boss (current object)
        transform.position = targetPos;
    }

}
