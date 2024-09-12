using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   

public class ZombieChasingState : StateMachineBehaviour
{
    private Transform player;
    private NavMeshAgent navMeshAgent;

    public float chasingSpeed = 4f;
    public float stopChasingRange = 18f;
    public float attackingRange = 2.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = animator.GetComponent<NavMeshAgent>();

        navMeshAgent.speed = chasingSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Chase the player
        navMeshAgent.SetDestination(player.position);
        animator.transform.LookAt(player);

        // Stop chasing
        float distanceToPlayer = Vector3.Distance(animator.transform.position, player.position);
        if(distanceToPlayer > stopChasingRange)
        {
            animator.SetBool("isChasing", false);
        }

        // Transition to Attacking State
        if(distanceToPlayer <= attackingRange)
        {
            animator.SetBool("isAttacking", true);
        }

        // Play sound
        if(SoundManager.instance.zombieAudioSource.isPlaying == false)
        {
            SoundManager.instance.zombieAudioSource.PlayOneShot(SoundManager.instance.zombieChase);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Stop moving
        navMeshAgent.SetDestination(navMeshAgent.transform.position);

        SoundManager.instance.zombieAudioSource.Stop();
    }
}
