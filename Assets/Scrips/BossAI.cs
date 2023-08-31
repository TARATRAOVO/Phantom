using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private BossAttack bossAttack;
    private bool isAttacking;
    public GameObject player;
    // distance between player and boss
    private float bossPlayerDistance;
    public bool isMoving;
    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        bossAttack = GetComponent<BossAttack>();
        isAttacking = false;
        isMoving = true;
        
    }

    // Update is called once per frame
    protected void Update()
    {
        // update the distance between player and boss
        bossPlayerDistance = Vector3.Distance(transform.position, player.transform.position);

        if (isMoving)
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
        
        if (bossAttack.currentEventState == "Recovery")
        {
            isAttacking = false;
        }

        if (bossPlayerDistance < 2.0f && isAttacking == false)
        {
            isAttacking = true;
            bossAttack.OnAttack();
            return;
        }

        // if (bossPlayerDistance > 10.0f && isAttacking == false)
        // {
        //     isAttacking = false;
        //     bossAttack.OnAttack();
        //     return;
        // }


        
    }

}
