using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterKinematics))]
public sealed class Character : CharacterBehaviour
{
    [Header("Inventory")]
    [SerializeField]
    private InventoryBehaviour inventory;

    [Header("Cameras")]
    [SerializeField]
    private Camera cameraWorld;

    [Header("Animation")]
    [SerializeField]
    private float dampTimeLocomotion = 0.15f;
    [SerializeField]
    private float dampTimeAiming = 0.3f;

    [Header("Animation Procedural")]
    [SerializeField]
    private Animator characterAnimator;

    private bool aiming;
    private bool running;
    private bool holstered;
    private float lastShotTime;
    private int layerOverlay;
    private int layerHolster;
    private int layerActions;
    private CharacterKinematics characterKinematics;
    private WeaponBehaviour equippedWeapon;
    private WeaponAttachmentManagerBehaviour weaponAttachmentManager;
    private ScopeBehaviour equippedWeaponScope;
    private MagazineBehaviour equippedWeaponMagazine;
    private bool reloading;
    private bool inspecting;
    private bool holstering;
    private Vector2 axisLook;
    private Vector2 axisMovement;
    private bool holdingButtonAim;
    private bool holdingButtonRun;
    private bool holdingButtonFire;
    private bool tutorialTextVisible;
    private bool cursorLocked;

    private static readonly int HashAimingAlpha = Animator.StringToHash("Aiming");
    private static readonly int HashMovement = Animator.StringToHash("Movement");

    protected override void Awake()
    {
        cursorLocked = true;
        UpdateCursorState();
        characterKinematics = GetComponent<CharacterKinematics>();
        //inventory.Init();
        //RefreshWeaponSetup();
    }

    protected override void Start()
    {
        layerHolster = characterAnimator.GetLayerIndex("Layer Holster");
        layerActions = characterAnimator.GetLayerIndex("Layer Actions");
        layerOverlay = characterAnimator.GetLayerIndex("Layer Overlay");
    }

    protected override void Update()
    {
        aiming = holdingButtonAim && CanAim();
        running = holdingButtonRun && CanRun();

        // if (holdingButtonFire)
        // {
        //     if (CanPlayAnimationFire() && equippedWeapon.HasAmmunition() && equippedWeapon.IsAutomatic())
        //     {
        //         if (Time.time - lastShotTime > 60.0f / equippedWeapon.GetRateOfFire())
        //             Fire();
        //     }
        // }

        UpdateAnimator();
    }

    protected override void LateUpdate()
    {
        if (equippedWeapon == null || equippedWeaponScope == null)
            return;

        if (characterKinematics != null)
        {
            characterKinematics.Compute();
        }
    }

    public override Camera GetCameraWorld() => cameraWorld;
    public override InventoryBehaviour GetInventory() => inventory;
    public override bool IsCrosshairVisible() => !aiming && !holstered;
    public override bool IsRunning() => running;
    public override bool IsAiming() => aiming;
    public override bool IsCursorLocked() => cursorLocked;
    public override bool IsTutorialTextVisible() => tutorialTextVisible;
    public override Vector2 GetInputMovement() => axisMovement;
    public override Vector2 GetInputLook() => axisLook;

    private void UpdateAnimator()
    {
        characterAnimator.SetFloat(HashMovement, Mathf.Clamp01(Mathf.Abs(axisMovement.x) + Mathf.Abs(axisMovement.y)), dampTimeLocomotion, Time.deltaTime);
        characterAnimator.SetFloat(HashAimingAlpha, Convert.ToSingle(aiming), 0.25f / 1.0f * dampTimeAiming, Time.deltaTime);
        characterAnimator.SetBool("Aim", aiming);
        characterAnimator.SetBool("Running", running);
    }

    private void Inspect()
    {
        inspecting = true;
        characterAnimator.CrossFade("Inspect", 0.0f, layerActions, 0);
    }

    private void Fire()
    {
        lastShotTime = Time.time;
        equippedWeapon.Fire();
        characterAnimator.CrossFade("Fire", 0.05f, layerOverlay, 0);
    }

    private void PlayReloadAnimation()
    {
        string stateName = equippedWeapon.HasAmmunition() ? "Reload" : "Reload Empty";
        characterAnimator.Play(stateName, layerActions, 0.0f);
        reloading = true;
        equippedWeapon.Reload();
    }

    private IEnumerator Equip(int index = 0)
    {
        if (!holstered)
        {
            SetHolstered(holstering = true);
            yield return new WaitUntil(() => holstering == false);
        }
        SetHolstered(false);
        characterAnimator.Play("Unholster", layerHolster, 0);
        inventory.Equip(index);
        RefreshWeaponSetup();
    }

    private void RefreshWeaponSetup()
    {
        if ((equippedWeapon = inventory.GetEquipped()) == null)
            return;

        characterAnimator.runtimeAnimatorController = equippedWeapon.GetAnimatorController();
        weaponAttachmentManager = equippedWeapon.GetAttachmentManager();
        if (weaponAttachmentManager == null)
            return;

        equippedWeaponScope = weaponAttachmentManager.GetEquippedScope();
        equippedWeaponMagazine = weaponAttachmentManager.GetEquippedMagazine();
    }

    private void FireEmpty()
    {
        lastShotTime = Time.time;
        characterAnimator.CrossFade("Fire Empty", 0.05f, layerOverlay, 0);
    }

    private void UpdateCursorState()
    {
        Cursor.visible = !cursorLocked;
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void SetHolstered(bool value = true)
    {
        holstered = value;
        characterAnimator.SetBool("Holstered", holstered);
    }

    private bool CanPlayAnimationFire()
    {
        if (holstered || holstering || reloading || inspecting)
            return false;

        return true;
    }

    private bool CanPlayAnimationReload()
    {
        if (reloading || inspecting)
            return false;

        return true;
    }

    private bool CanPlayAnimationHolster()
    {
        if (reloading || inspecting)
            return false;

        return true;
    }

    private bool CanChangeWeapon()
    {
        if (holstering || reloading || inspecting)
            return false;

        return true;
    }

    private bool CanPlayAnimationInspect()
    {
        if (holstered || holstering || reloading || inspecting)
            return false;

        return true;
    }

    private bool CanAim()
    {
        if (holstered || inspecting || reloading || holstering)
            return false;

        return true;
    }

    private bool CanRun()
    {
        if (inspecting || reloading || aiming || (holdingButtonFire && equippedWeapon.HasAmmunition()) || axisMovement.y <= 0 || Math.Abs(Mathf.Abs(axisMovement.x) - 1) < 0.01f)
            return false;

        return true;
    }

    public void OnTryFire(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
            return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                holdingButtonFire = true;
                break;
            case InputActionPhase.Performed:
                if (!CanPlayAnimationFire())
                    break;

                if (equippedWeapon.HasAmmunition())
                {
                    if (equippedWeapon.IsAutomatic())
                        break;

                    if (Time.time - lastShotTime > 60.0f / equippedWeapon.GetRateOfFire())
                        Fire();
                }
                else
                    FireEmpty();
                break;
            case InputActionPhase.Canceled:
                holdingButtonFire = false;
                break;
        }
    }

    public void OnTryPlayReload(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
            return;

        if (!CanPlayAnimationReload())
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            PlayReloadAnimation();
        }
    }

    public void OnTryInspect(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
            return;

        if (!CanPlayAnimationInspect())
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            Inspect();
        }
    }

    public void OnTryAiming(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
            return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                holdingButtonAim = true;
                break;
            case InputActionPhase.Canceled:
                holdingButtonAim = false;
                break;
        }
    }

    public void OnTryHolster(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            if (CanPlayAnimationHolster())
            {
                SetHolstered(!holstered);
                holstering = true;
            }
        }
    }

    public void OnTryRun(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
            return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                holdingButtonRun = true;
                break;
            case InputActionPhase.Canceled:
                holdingButtonRun = false;
                break;
        }
    }

    public void OnTryInventoryNext(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
            return;

        if (inventory == null)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            float scrollValue = context.valueType.IsEquivalentTo(typeof(Vector2)) ? Mathf.Sign(context.ReadValue<Vector2>().y) : 1.0f;
            int indexNext = scrollValue > 0 ? inventory.GetNextIndex() : inventory.GetLastIndex();
            int indexCurrent = inventory.GetEquippedIndex();

            if (CanChangeWeapon() && (indexCurrent != indexNext))
                StartCoroutine(nameof(Equip), indexNext);
        }
    }

    public void OnLockCursor(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            cursorLocked = !cursorLocked;
            UpdateCursorState();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        axisMovement = cursorLocked ? context.ReadValue<Vector2>() : default;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        axisLook = cursorLocked ? context.ReadValue<Vector2>() : default;
    }

    public void OnUpdateTutorial(InputAction.CallbackContext context)
    {
        tutorialTextVisible = context.phase switch
        {
            InputActionPhase.Started => true,
            InputActionPhase.Canceled => false,
            _ => tutorialTextVisible
        };
    }

    public override void EjectCasing()
    {
        equippedWeapon?.EjectCasing();
    }

    public override void FillAmmunition(int amount)
    {
        equippedWeapon?.FillAmmunition(amount);
    }

    public override void SetActiveMagazine(int active)
    {
        equippedWeaponMagazine.gameObject.SetActive(active != 0);
    }

    public override void AnimationEndedReload()
    {
        reloading = false;
    }

    public override void AnimationEndedInspect()
    {
        inspecting = false;
    }

    public override void AnimationEndedHolster()
    {
        holstering = false;
    }
}