using UnityEngine;

public abstract class CharacterBehaviour : MonoBehaviour
{
    protected virtual void Awake() {}

    protected virtual void Start() {}

    protected virtual void Update() {}

    protected virtual void LateUpdate() {}

    public abstract Camera GetCameraWorld();
    
    public abstract InventoryBehaviour GetInventory();

    public abstract bool IsCrosshairVisible();

    public abstract bool IsRunning();
    
    public abstract bool IsAiming();

    public abstract bool IsCursorLocked();

    public abstract bool IsTutorialTextVisible();

    public abstract Vector2 GetInputMovement();

    public abstract Vector2 GetInputLook();

    public abstract void EjectCasing();

    public abstract void FillAmmunition(int amount);

    public abstract void SetActiveMagazine(int active);
    
    public abstract void AnimationEndedReload();

    public abstract void AnimationEndedInspect();

    public abstract void AnimationEndedHolster();
}