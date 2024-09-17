using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteZombies
{
    public class ZombieIdleState : StateMachineBehaviour
    {
        private float timer;
        public float idleTime = 0f;

        private Transform player;

        public float detectionRange = 10f;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            timer = 0f;
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Transition to Patrolling State
            timer += Time.deltaTime;
            if(timer >= idleTime)
            {
                animator.SetBool("isPatrolling", true);
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
                SoundManager.instance.zombieAudioSource.clip = SoundManager.instance.zombieIdle;
                SoundManager.instance.zombieAudioSource.PlayDelayed(1f);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        SoundManager.instance.zombieAudioSource.Stop();
        }


    }
}