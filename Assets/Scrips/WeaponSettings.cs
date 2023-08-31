using UnityEngine;
using MxM;
using UnityEngine.AI;
using UnityEngine.Events;

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
    public void OnTriggerEnter(Collider other)
    {
        if (targetLayer == (targetLayer | (1 << other.gameObject.layer)))
        {
            onHitTarget.Invoke();
            if (other.gameObject.GetComponent<AnimationController>().isEvadeState)
            {
                onHitEvade.Invoke();
                return;
            }
            Vector3 direction = other.gameObject.transform.position - weaponHolder.transform.position;
            direction.y = 0;
            other.gameObject.GetComponent<Health>().TakeDamage(damage*attack.attackDamag[attack.currentEventId], stun*attack.attackStuns[attack.currentEventId], uped*attack.attackUpeds[attack.currentEventId]);
            other.gameObject.GetComponent<CharacterController>().Move(direction.normalized * attack.attackPushs[attack.currentEventId] * 0.2f);
        }  
    }

}
