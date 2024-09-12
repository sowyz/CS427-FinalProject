using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieAttack zombieAttack;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = zombieAttack.damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
