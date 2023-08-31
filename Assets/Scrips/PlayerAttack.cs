using UnityEngine;
using MxM;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Linq;
using System.Collections;
using Unity.VisualScripting;

public class PlayerAttack : Attack
{
    [HideInInspector]
    public CameraControl cameraLookAtController;
    // how long the button can be pressed
    [Header("Player Attack Settings")]
    public Dictionary<string, int> weaponActionDict;
    public MxMEventDefinition testEventDef;
    public float heavyAttackTimeLimit = 0.3f;
    public float QTETimeLimit = 0.3f;
    public float phantomAttackTime = 0.2f;
    public float phantomCounterTime = 2.0f;
    public float phantomAttackCD = 5.0f;
    public List<MxMEventDefinition> mmSeriesAttack;
    public List<MxMEventDefinition> mmSeriesAttackLeft;
    public MxMEventDefinition mmHeavyAttackEventDef;
    public MxMEventDefinition mmQTEAttackEventDef;
    public GameObject enemy = null;
    private bool phantomAttack = false;
    private GameObject phantom = null;
    private bool phantomIsNight = false;
    private float phantomAttackTimeLeft = 0;
    private GameObject tempPhantom = null;
    private bool AttackTrigger = false;
    private int phantomEventID;
    private BossAnimationController enemyAnimationController;
    // how long the button is pressed
    private float pressedTime = 0;
    public float phantomAttackCDLeft = 0;
    private GameObject player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        mmSeriesAttackLeft = new List<MxMEventDefinition>(mmSeriesAttack);
        phantomAttackTimeLeft = phantomAttackTime;
        cameraLookAtController = GameObject.FindGameObjectWithTag("CameraLookAt").GetComponent<CameraControl>();
        enemyAnimationController = enemy.GetComponent<BossAnimationController>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.F))
        {
            RefreshPhantomAttack();
        }

        if (currentEventId == 0)
        {
            mmSeriesAttackLeft = new List<MxMEventDefinition>(mmSeriesAttack);
        }



        if (Input.GetButtonDown("Fire2"))
        {
            PhantomAttack();
        }

        if (phantomAttackCDLeft > 0)
        {
            phantomAttackCDLeft -= Time.deltaTime;
        }

        AttackTick();
        TempPhantomTick();
        TickWeaponCollider();
    }

    // Check if normal attack button is pressed
    void AttackTick()
    {
        EEventState eEventState;
        // heavy attack
        if (Input.GetButton("Fire1"))
        {
            pressedTime += Time.deltaTime;
            if (enemyAnimationController.isUped && pressedTime > QTETimeLimit && !IsAttacking())
            {
                pressedTime = 0;
                mmAnimator.BeginEvent(mmQTEAttackEventDef);
                LookAtEnemy();
            }
            if (pressedTime > heavyAttackTimeLimit && !IsAttacking())
            {
                pressedTime = 0;
                mmAnimator.BeginEvent(mmHeavyAttackEventDef);
                LookAtEnemy();
            }
        }
        else
        {
            pressedTime = 0;
        }

        eEventState = mmAnimator.CurrentEventState;
        if (Input.GetButtonUp("Fire1") && (eEventState.Equals(EEventState.FollowThrough) || eEventState.Equals(EEventState.Recovery) || eEventState.Equals(EEventState.Windup)))
        {
            AttackTrigger = true;
        }
        if (AttackTrigger && !IsAttacking())
        {
            AttackTrigger = false;
            SeriesAttack();
            LookAtEnemy();
        }
    }
    void SeriesAttack() // 
    {
        if (mmSeriesAttackLeft.Count > 0)
        {
            mmAnimator.BeginEvent(mmSeriesAttackLeft[0]);
            mmSeriesAttackLeft.RemoveAt(0);
        }
        else
        {
            mmSeriesAttackLeft = new List<MxMEventDefinition>(mmSeriesAttack);
        }
    }
    void PhantomAttack()
    {
        if (!this.CompareTag("Player"))
        {
            return;
        }

        Vector3 attackLocation;
        if (enemy)
        {
            // behind enemy
            Vector3 playerLocation = this.transform.position;
            Vector3 enemyLocation = enemy.transform.position;
            playerLocation.y = 0;
            enemyLocation.y = 0;
            attackLocation = enemyLocation + (enemyLocation - playerLocation).normalized * 1.5f;
        }
        else
        {
            attackLocation = this.transform.position + this.transform.forward * 2;
        }

        if (phantom)
        {
            tempPhantom = Instantiate(gameObject, this.transform.position, Quaternion.identity);
            tempPhantom.tag = "TempPhantom";
            tempPhantom.GetComponentInChildren<Animator>().enabled = false;
            skyboxChanger.isNight = phantomIsNight;
            // change the camera's forward direction to the phantom's forward direction
            cameraLookAtController.Yaw = phantom.transform.eulerAngles.y;

            // if phantom is already created, teleport it to the attack location
            this.transform.position = phantom.transform.position;
            this.transform.forward = phantom.transform.forward;
            if (phantomEventID != 0)
            {
                this.transform.position = attackLocation;
                mmAnimator.BeginEvent(phantomEventID);
                LookAtEnemy();
            }

            Destroy(phantom);

            return;
        }

        if (phantomAttackCDLeft <= 0)
        {
            // if phantom is not created, create it
            phantom = Instantiate(gameObject, attackLocation, Quaternion.identity);
            phantom.tag = "Phantom";
            phantomIsNight = skyboxChanger.isNight;
            // cd for phantom attack
            phantomAttackCDLeft = phantomAttackCD;
            // get attack event id when phantom is created
            phantomEventID = mmAnimator.CurrentEvent.EventId;
            if (mmAnimator.IsEventComplete)
            {
                phantomEventID = 0;
            }
            // phantom should look at enemy
            if (enemy)
            {
                Vector3 lookAt = enemy.transform.position - phantom.transform.position;
                lookAt.y = 0;
                phantom.transform.forward = lookAt.normalized;
            }
            phantom.GetComponentInChildren<Animator>().enabled = false;
        }
    }
    // called when taken damage
    public void PhantomCounter(GameObject whichWeapon)
    {
        if (this.CompareTag("TempPhantom") && !phantomAttack)
        {
            RefreshPhantomAttack();
            phantomAttackTimeLeft += phantomCounterTime;
            phantomAttack = true;
            StartCoroutine(OnPhantomCounter(whichWeapon));
        }
    }
    IEnumerator OnPhantomCounter(GameObject whichWeapon)
    {
        BossWeaponSettings weaponSettings = whichWeapon.GetComponentInChildren<BossWeaponSettings>();
        weaponSettings.LoseFunction();
        yield return new WaitForSeconds(phantomCounterTime);
        weaponSettings.GainFunction();
    }
    void TempPhantomTick()
    {
        if (this.CompareTag("TempPhantom"))
        {
            phantomAttackTimeLeft -= Time.deltaTime;
            if (phantomAttackTimeLeft < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    void LookAtEnemy() // if there is an enemy, look at the enemy
    {
        Vector3 attackLookAt = transform.forward;
        if (enemy)
        {
            attackLookAt = enemy.transform.position - transform.position;
        }
        attackLookAt.y = 0;
        transform.forward = attackLookAt;
    }
    public bool IsAttacking()
    {
        return !(currentEventState.Equals("Recovery") || currentEventId == 0);
    }
    public void RefreshPhantomAttack()
    {
        player.GetComponent<PlayerAttack>().phantomAttackCDLeft = 0;
    }
}

