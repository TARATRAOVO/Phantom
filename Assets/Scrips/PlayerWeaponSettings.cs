using UnityEngine;
using MxM;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerWeaponSettings : WeaponSettings
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (weaponHolder.tag == "Player")
        {
            base.OnTriggerEnter(other);
        }
    }

}
