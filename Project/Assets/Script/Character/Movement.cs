using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Movement : MovementBehaviour
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalking;

    [SerializeField]
    private AudioClip audioClipRunning;

    [Header("Speeds")]
    [SerializeField]
    private float speedWalking = 10.0f;

    [SerializeField]
    private float speedRunning = 20.0f;

    private Vector3 Velocity
    {
        get => rigidBody.velocity;
        set => rigidBody.velocity = value;
    }

    private Rigidbody rigidBody;
    private CapsuleCollider capsule;
    private AudioSource audioSource;
    private bool grounded;
    private CharacterBehaviour playerCharacter;
    //private WeaponBehaviour equippedWeapon;
    private readonly RaycastHit[] groundHits = new RaycastHit[8];

    protected override void Awake()
    {
        playerCharacter = GetComponent<CharacterBehaviour>();
    }

    protected override void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        capsule = GetComponent<CapsuleCollider>();

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClipWalking;
        audioSource.loop = true;
    }

    private void OnCollisionStay()
    {
        Bounds bounds = capsule.bounds;
        Vector3 extents = bounds.extents;
        float radius = extents.x - 0.01f;

        Physics.SphereCastNonAlloc(bounds.center, radius, Vector3.down, groundHits, extents.y - radius * 0.5f, ~0, QueryTriggerInteraction.Ignore);

        if (!groundHits.Any(hit => hit.collider != null && hit.collider != capsule))
            return;

        for (var i = 0; i < groundHits.Length; i++)
            groundHits[i] = new RaycastHit();

        grounded = true;
    }

    protected override void FixedUpdate()
    {
        MoveCharacter();
        grounded = false;
    }

    protected override void Update()
    {
        //equippedWeapon = playerCharacter.GetInventory().GetEquipped();
        PlayFootstepSounds();
    }

    private void MoveCharacter()
    {
        Vector2 frameInput = playerCharacter.GetInputMovement();
        Debug.Log($"Input Movement: {frameInput}");

        var movement = new Vector3(frameInput.x, 0.0f, frameInput.y);
        Debug.Log($"Initial Movement Vector: {movement}");

        if (playerCharacter.IsRunning())
            movement *= speedRunning;
        else
            movement *= speedWalking;

        movement = transform.TransformDirection(movement);
        Debug.Log($"Transformed Movement Vector: {movement}");

        Velocity = new Vector3(movement.x, 0.0f, movement.z);
        Debug.Log($"Set Velocity: {Velocity}");
    }

    private void PlayFootstepSounds()
    {
        if (grounded && rigidBody.velocity.sqrMagnitude > 0.1f)
        {
            audioSource.clip = playerCharacter.IsRunning() ? audioClipRunning : audioClipWalking;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else if (audioSource.isPlaying)
            audioSource.Pause();
    }
}
