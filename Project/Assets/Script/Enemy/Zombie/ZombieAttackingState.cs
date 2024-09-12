using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackingState : StateMachineBehaviour
{
    private Transform player;
    private NavMeshAgent navMeshAgent;

    public float attackingRange = 2.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LookAtPlayer();

        // Stop attacking
        float distanceToPlayer = Vector3.Distance(animator.transform.position, player.position);
        if(distanceToPlayer > attackingRange)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (player.position - navMeshAgent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
