using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public BGMController bgmController;
    private NavMeshAgent agent;
    private BossAttack bossAttack;
    private Health health;

    // distance between player and boss

    private bool knockDownBool = false;
    public float nearDistanceTimeLimit = 3.0f;
    public float medianDistanceTimeLimit = 5.0f;
    public float farDistanceTimeLimit = 10.0f;
    public float nearDistance = 3.0f;
    public float medianDistance = 5.0f;

    [Header("Boss State (Read Only)")]
    public bool isAttacking;
    public bool isMoving;
    public float bossPlayerDistance;

    private float nearDistanceTime = 0;
    private float medianDistanceTime = 0;
    private float farDistanceTime = 0;

    private int bossPhantomAttackCount = 0;
    private int bossInvisibleAttackCount = 0;
    [HideInInspector] public GameObject player;
    [HideInInspector] public BossAnimationController bossAnimationController;
    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        bossAnimationController = GetComponent<BossAnimationController>();
        health = GetComponent<Health>();
        player = GameObject.FindGameObjectWithTag("Player");
        bossAttack = GetComponent<BossAttack>();
        isAttacking = false;
        isMoving = true;
    }

    // Update is called once per frame
    protected void Update()
    {
        DrawCircle(nearDistance);
        DrawCircle(medianDistance);

        // update the time of distance between player and boss
        if (bossPlayerDistance < nearDistance)
        {
            nearDistanceTime += Time.deltaTime;
            medianDistanceTime = 0;
            farDistanceTime = 0;
        }
        else if (bossPlayerDistance < medianDistance)
        {
            medianDistanceTime += Time.deltaTime;
            nearDistanceTime = 0;
            farDistanceTime = 0;
        }
        else
        {
            farDistanceTime += Time.deltaTime;
            nearDistanceTime = 0;
            medianDistanceTime = 0;
        }


        // update the distance between player and boss
        bossPlayerDistance = Vector3.Distance(transform.position, player.transform.position);

        // update the state of boss
        if (bossPlayerDistance > 0.5 && !bossAnimationController.isKnockDown && !bossAnimationController.isUped && bossAnimationController.isEvadeAnimEnd)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (bossAnimationController.isKnockDown)
        {
            isMoving = false;
        }

        if (bossAttack.currentEventState == "Recovery")
        {
            isMoving = true;
        }

        isAttacking = IsBossAttacking();

        if ((health.currentHealth / health.maxHealth < 0.60f) && bossPhantomAttackCount == 0 && !isAttacking && !bossAnimationController.isKnockDown && !bossAnimationController.isUped && bossAttack.currentEventId == 0)
        {
            bossAttack.OnBossPhantomAttack();
            bossPhantomAttackCount++;
        }


        if ((health.currentHealth / health.maxHealth < 0.35f) && bossInvisibleAttackCount == 0 && !isAttacking && !bossAnimationController.isKnockDown && !bossAnimationController.isUped && bossAttack.currentEventId == 0)
        {
            bossAttack.OnBossInvisibleAttack();
            bgmController.PlayAudio();
            bossInvisibleAttackCount++;
        }


        if ((health.currentHealth / health.maxHealth < 0.20f) && bossInvisibleAttackCount == 1 && bossPhantomAttackCount == 1 && !isAttacking && !bossAnimationController.isKnockDown && !bossAnimationController.isUped && bossAttack.currentEventId == 0 && !bossAttack.isSepecialAttack)
        {
            bossAttack.OnBossPhantomAttack();
            bossPhantomAttackCount++;
        }


        if (nearDistanceTime > nearDistanceTimeLimit && !isAttacking && !bossAnimationController.isUped && !bossAnimationController.isKnockDown && !bossAttack.isSepecialAttack)
        {
            bossAnimationController.OnBossEvadeBackward();
            nearDistanceTime = 0;
            isMoving = false;
        }

        if (farDistanceTime > farDistanceTimeLimit && !isAttacking && !bossAnimationController.isUped && !bossAnimationController.isKnockDown)
        {
            bossAttack.TeleportBossToPlayerRadius(2.0f);
            farDistanceTime = 0;

        }
        // Attack when player is near and boss is not knocked down
        if (bossPlayerDistance < nearDistance && !bossAnimationController.isKnockDown && bossAttack.currentEventId == 0 && !bossAttack.isSepecialAttack && bossAnimationController.isEvadeAnimEnd)
        {
            bossAttack.OnBossAttack();
            isAttacking = true;
            isMoving = false;
        }

        if (isMoving)
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }

        void DrawCircle(float radius)
        {
            int numSegments = 360;   // 圆的分段数，可以提高或降低看你需要的准确度
            Vector3 prevPos = Vector3.zero;
            Vector3 newPos;

            for (int i = 0; i <= numSegments; i++)
            {
                float rad = Mathf.Deg2Rad * (i * 360f / numSegments);
                newPos = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius) + transform.position;
                if (i > 0)
                {
                    Debug.DrawLine(prevPos, newPos, Color.red, 0.0f, false);
                }
                prevPos = newPos;
            }
        }

        bool IsBossAttacking()
        {

            for (int i = 0; i < bossAttack.attackEventIDs.Length; i++)
            {
                if (bossAttack.currentEventId == bossAttack.attackEventIDs[i])
                {
                    return true;
                }
            }
            return false;
        }

    }

}
