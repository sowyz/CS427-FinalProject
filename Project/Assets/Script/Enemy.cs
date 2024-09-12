using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int health = 100;
    
    private Animator anim;

    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateAnimaton();
    }

    private void UpdateAnimaton()
    {
        if(navMeshAgent.velocity.magnitude > 0.1f)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            int random = Random.Range(0, 2);
            if(random == 0)
            {
                anim.SetTrigger("Die1");
            }
            else
            {
                anim.SetTrigger("Die2");
            }
        }
        else
        {
            anim.SetTrigger("Damaged");
        }
    }
}
