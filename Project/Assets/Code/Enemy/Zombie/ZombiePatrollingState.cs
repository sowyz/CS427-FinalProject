using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrollingState : StateMachineBehaviour
{
    private float timer;
    public float patrollingTime = 0f;

    private Transform player;
    private NavMeshAgent navMeshAgent;

    public float detectionRange = 12f;
    public float patrolSpeed = 2f;

    List<Transform> wayPoints = new List<Transform>();

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = animator.GetComponent<NavMeshAgent>();

        navMeshAgent.speed = patrolSpeed;
        timer = 0f;

        // Move to first way point
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoint");
        foreach(Transform child in waypointCluster.transform)
        {
            wayPoints.Add(child);
        }

        Vector3 nextDestination = wayPoints[Random.Range(0, wayPoints.Count)].position;
        navMeshAgent.SetDestination(nextDestination);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Move to next way point
        if(navMeshAgent.remainingDistance <= 0.1f)
        {
            Vector3 nextDestination = wayPoints[Random.Range(0, wayPoints.Count)].position;
            navMeshAgent.SetDestination(nextDestination);
        }

        // Transition to Idle State
        timer += Time.deltaTime;
        if(timer >= patrollingTime)
        {
            animator.SetBool("isPatrolling", false);
        }

        // Transition to Chasing State
        float distanceToPlayer = Vector3.Distance(animator.transform.position, player.position);
        if(distanceToPlayer <= detectionRange)
        {
            animator.SetBool("isChasing", true);
        }

        // Play sound
        if(SoundManager.instance.zombieAudioSource.isPlaying == false)
        {
            SoundManager.instance.zombieAudioSource.clip = SoundManager.instance.zombieWalk;
            SoundManager.instance.zombieAudioSource.PlayDelayed(1f);
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
