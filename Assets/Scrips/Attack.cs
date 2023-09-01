using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MxM;
using Mono.Cecil;
using Unity.VisualScripting;
using System;
using UnityEngine.UIElements;
using System.Net.Http.Headers;
using MxMEditor;

public class Attack : MonoBehaviour
{
    [HideInInspector] public MxMAnimator mmAnimator;
    public LayerMask targetLayer;
    protected GameObject player;
    private readonly int[] attackEventIDs = { 9, 10, 11, 12, 13, 14, 5, 21, 22, 23, 24, 25, 26, 27, 28 };
    private readonly float[] attackDamags = { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    private readonly float[] attackStunss = { 0f, 0f, 0f, 1.0f, 0f, 0f, 0f, 0f, 0f, 1.0f, 0f, 0f, 0f, 0f, 0f };
    private readonly float[] attackUpedss = { 0f, 0f, 1.0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1.0f, 0f, 0f, 0f, 0f };
    private readonly float[] attackPushss = { 1.0f, 1.0f, 1.0f, 0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 0f, 0f, 1.0f, 1.0f, 1.0f, 1.0f };
    public Dictionary<int, float> attackDamag;
    public Dictionary<int, float> attackStuns;
    public Dictionary<int, float> attackUpeds;
    public Dictionary<int, float> attackPushs;
    public GameObject[] weapons;
    public bool ifInvisibleWeapon = false;
    [HideInInspector] public String currentEventState;
    public int currentEventId;
    [HideInInspector] public List<Collider> weaponColliders;
    [HideInInspector] public List<SkinnedMeshRenderer> weaponMeshRenderers;
    [HideInInspector] public GameObject myEventController;
    [HideInInspector] public SkyboxChanger skyboxChanger;
    [HideInInspector] public AnimationController animationController;
    [HideInInspector] public float originAnimPlaybackSpeed;
    protected SkinnedMeshRenderer[] thisMeshRenderers;
    protected virtual void Start()
    {
        attackDamag = new Dictionary<int, float>();
        attackStuns = new Dictionary<int, float>();
        attackUpeds = new Dictionary<int, float>();
        attackPushs = new Dictionary<int, float>();
        mmAnimator = GetComponent<MxMAnimator>();
        myEventController = GameObject.FindGameObjectWithTag("MyEventController");
        skyboxChanger = myEventController.GetComponent<SkyboxChanger>();
        thisMeshRenderers = this.GetComponentsInChildren<SkinnedMeshRenderer>();
        animationController = GetComponent<AnimationController>();
        originAnimPlaybackSpeed = animationController.originAnimPlaybackSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        IniAttactProperty();
        IniWeaponCollider();
    }

    protected virtual void Update()
    {
        currentEventState = mmAnimator.CurrentEventState.ToString();
        currentEventId = mmAnimator.CurrentEvent.EventId;
        if (mmAnimator.IsEventComplete)
        {
            currentEventId = 0;
        }
    }

    // control the weapon collider
    public void TickWeaponCollider()
    {
        if (currentEventState == "Action" && IsCurrentEventAttack())
        {
            EnableWeaponCollider();
            return;
        }
        DisableWeaponCollider();
    }
    public void EnableWeaponCollider()
    {
        foreach (Collider weaponCollider in weaponColliders)
        {
            weaponCollider.enabled = true;
        }
        if (ifInvisibleWeapon)
        {
            foreach (SkinnedMeshRenderer weaponMeshRenderer in weaponMeshRenderers)
            {
                weaponMeshRenderer.enabled = true;
            }
        }

    }
    public void DisableWeaponCollider()

    {
        foreach (Collider weaponCollider in weaponColliders)
        {
            weaponCollider.enabled = false;
        }
        if (ifInvisibleWeapon)
        {
            foreach (SkinnedMeshRenderer weaponMeshRenderer in weaponMeshRenderers)
            {
                weaponMeshRenderer.enabled = false;
            }
        }

    }

    public bool IsCurrentEventAttack()
    {

        foreach (int attackEventID in attackEventIDs)
        {
            if (currentEventId == attackEventID)
            {
                return true;
            }
        }
        return false;
    }

    private void IniAttactProperty()
    {
        for (int i = 0; i < attackEventIDs.Length; i++)
        {
            attackDamag.Add(attackEventIDs[i], attackDamags[i]);
            attackStuns.Add(attackEventIDs[i], attackStunss[i]);
            attackUpeds.Add(attackEventIDs[i], attackUpedss[i]);
            attackPushs.Add(attackEventIDs[i], attackPushss[i]);
        }
    }

    private void IniWeaponCollider()
    {
        foreach (GameObject weapon in weapons)
        {
            weaponColliders.AddRange(weapon.GetComponentsInChildren<Collider>());
            weaponMeshRenderers.AddRange(weapon.GetComponentsInChildren<SkinnedMeshRenderer>());
        }
    }

    public void LookAtTarget(GameObject targetObject)
    {
        Vector3 lookAtPosition = targetObject.transform.position;
        lookAtPosition.y = this.transform.position.y;
        this.transform.LookAt(lookAtPosition);
    }

}
