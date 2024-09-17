using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace InfiniteZombies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private int health = 100;
        
        private Animator anim;

        private NavMeshAgent navMeshAgent;

        public bool isDead = false;
        public enum EnemyType
        {
            Zombie,
        }
        public EnemyType enemyType;
        private AudioSource audioSource;

        // Start is called before the first frame update
        private void Start()
        {
            anim = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            audioSource = enemyType switch
            {
                EnemyType.Zombie => SoundManager.instance.zombieAudioSource,
                _ => null
            };
        }

        // Update is called once per frame
        private void Update()
        {
        
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if(health <= 0)
            {
                isDead = true;
                int random = Random.Range(0, 2);
                if(random == 0)
                {
                    anim.SetTrigger("Die1");
                }
                else
                {
                    anim.SetTrigger("Die2");
                }
                
                if (audioSource != null)
                {
                    audioSource.clip = SoundManager.instance.zombieDie;
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource != null)
                {
                    audioSource.clip = SoundManager.instance.zombieHit;
                    audioSource.Play();
                }

                anim.SetTrigger("Damaged");
            }
        }

        private void OnDrawGizmos()
        {
            // Draw attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 2f);
            // Draw chasing range
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 15f);
            // Draw detection range
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 10f);
        }
    }
}