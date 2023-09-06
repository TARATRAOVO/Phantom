using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class WeaponSettings : MonoBehaviour
{

    private readonly float damage = 100f;
    private readonly float stun = 100f;
    private readonly float uped = 100f;
    private LayerMask targetLayer;
    [Header("Weapon Holder's Status (Read Only))")]
    [SerializeField] public GameObject weaponHolder;
    [SerializeField] public Attack attack;
    public UnityEvent onHitTarget;
    public UnityEvent onHitEvade;


    protected virtual void Awake()
    {
        // weapon holder is the ultimate parent of the weapon
        weaponHolder = transform.root.gameObject;
        attack = weaponHolder.GetComponent<Attack>();
        targetLayer = attack.targetLayer;
    }

    // damage the target layer
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (targetLayer == (targetLayer | (1 << other.gameObject.layer)))
        {
            Transform target = other.transform.root;
            onHitTarget.Invoke();
            if (target.GetComponent<AnimationController>().isEvadeState)
            {
                onHitEvade.Invoke();
                return;
            }
            Vector3 direction = other.gameObject.transform.position - weaponHolder.transform.position;
            direction.y = 0;
            target.gameObject.GetComponent<Health>().TakeDamage(damage * attack.attackDamag[attack.currentEventId], stun * attack.attackStuns[attack.currentEventId], uped * attack.attackUpeds[attack.currentEventId]);
            StartCoroutine(WaitForAttackAnimEnd(direction, target));
        }
    }

    IEnumerator WaitForAttackAnimEnd(Vector3 direction, Transform target)
    {
        while (!attack.currentEventState.Equals("FollowThrough"))
        {
            yield return null;
        }

        float targetPush;
        try
        {
            int eventId = attack.currentEventId;
            targetPush = attack.attackPushs[eventId];
        }
        catch
        {
            targetPush = 0;
        }

        float currentPush = 0;

        while (currentPush < targetPush)
        {
            currentPush += Time.deltaTime * 10;
            target.position += direction.normalized * currentPush * 0.01f;
            yield return null;
        }
    }

}
