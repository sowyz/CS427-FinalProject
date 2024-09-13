using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; set; }
    [Header("Player Sounds")]
    public AudioSource playerAudioSource;
    public AudioClip playerHurt;
    public AudioClip playerDie;
    
    [Header("Music")]
    public AudioClip playerDieMusic;

    [Header("Zombie Sounds")]
    public AudioSource zombieAudioSource;
    public AudioClip zombieWalk;
    public AudioClip zombieAttack;
    public AudioClip zombieDie;
    public AudioClip zombieHit;
    public AudioClip zombieChase;
    public AudioClip zombieIdle;
    // Start is called before the first frame update
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
