using UnityEngine;
using MxM;
using System.Collections.Generic;
using System.Collections;

public class PlayerAttack : Attack
{
    public GameObject test;
    [HideInInspector] public CameraControl cameraLookAtController;
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
    public bool phantomIsNight = false;
    private float phantomAttackTimeLeft = 0;
    private GameObject tempPhantom = null;
    private bool AttackTrigger = false;
    private int phantomEventID;
    private BossAnimationController enemyAnimationController;
    private BossAttack enemyAttack;
    // how long the button is pressed
    private float pressedTime = 0;
    public float phantomAttackCDLeft = 0;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        mmSeriesAttackLeft = new List<MxMEventDefinition>(mmSeriesAttack);
        phantomAttackTimeLeft = phantomAttackTime;
        cameraLookAtController = GameObject.FindGameObjectWithTag("CameraLookAt").GetComponent<CameraControl>();
        enemyAnimationController = enemy.GetComponent<BossAnimationController>();
        enemyAttack = enemy.GetComponent<BossAttack>();
    }



    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (currentEventId == 0)
        {
            mmSeriesAttackLeft = new List<MxMEventDefinition>(mmSeriesAttack);
        }

        if (this.tag == "Phantom")
        {
            if (phantomIsNight == skyboxChanger.isNight)
            {
                foreach (SkinnedMeshRenderer meshRenderer in thisMeshRenderers)
                {
                    meshRenderer.enabled = true;
                }
            }
            else
            {
                foreach (SkinnedMeshRenderer meshRenderer in thisMeshRenderers)
                {
                    meshRenderer.enabled = false;
                }
            }
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
        if (tag == "Phantom")
        {
            return;
        }

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

        if (phantom)
        {
            tempPhantom = Instantiate(gameObject, transform.position, Quaternion.identity);
            tempPhantom.tag = "TempPhantom";
            tempPhantom.GetComponentInChildren<Animator>().enabled = false;
            skyboxChanger.isNight = phantomIsNight;
            // change the camera's forward direction to the phantom's forward direction
            cameraLookAtController.Yaw = phantom.transform.eulerAngles.y;

            // if phantom is already created, teleport it to the attack location

            if (phantomEventID != 0)
            {
                mmAnimator.BeginEvent(phantomEventID);
                transform.position = CalculateAttackLocation();
                LookAtEnemy();
            }
            else
            {
                transform.position = phantom.transform.position;
                transform.forward = phantom.transform.forward;
            }
            Destroy(phantom);
            return;
        }

        if (phantomAttackCDLeft <= 0)
        {
            // if phantom is not created, create it
            phantomIsNight = skyboxChanger.isNight;
            phantom = Instantiate(gameObject, CalculateAttackLocation(), Quaternion.identity);
            phantom.tag = "Phantom";
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
                Destroy(gameObject);
            }
        }
    }
    void LookAtEnemy() // if there is an enemy, look at the enemy
    {
        print(tag + " LookAtEnemy");
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

    public Vector3 CalculateAttackLocation()
    {
        Vector3 attackLocation;
        if (enemy && enemyAttack.isVisibleNow)
        {
            // behind enemy
            Vector3 playerLocation = transform.position;
            Vector3 enemyLocation = enemy.transform.position;
            playerLocation.y = 0;
            enemyLocation.y = 0;
            attackLocation = enemyLocation + (enemyLocation - playerLocation).normalized * 2.0f;
        }
        else
        {
            attackLocation = transform.position + transform.forward * 2;
        }
        return attackLocation;
    }
}

