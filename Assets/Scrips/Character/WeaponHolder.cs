using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    [Header("Weapon To Spawn")]
    [SerializeField]
    private WeaponScriptable WeaponToSpawn;

    [SerializeField]
    private Transform WeaponSocketLocation;


    private Transform GripIKLocation;


    private bool WasFiring = false;
    private bool FiringPressed = false;

    //components
    public PlayerController Controller => PlayerController;
    private PlayerController PlayerController;
    private CrossHairScript PlayerCrosshair;
    public CrossHairScript Corsshair => PlayerCrosshair;

    public void UnEquipItem()
    {
        Destroy(EquippedWeapon.gameObject);
        EquippedWeapon = null;
    }

    private Animator PlayerAnimator;

    //Ref
    private Camera ViewCamera;

    public void EquipWeapon(WeaponScriptable weaponScriptable)
    {
        if (weaponScriptable == null) return;

        GameObject spawnedWeapon = Instantiate(weaponScriptable.ItemPrefab, WeaponSocketLocation.position, WeaponSocketLocation.rotation, WeaponSocketLocation);

        if (spawnedWeapon)
        {
            EquippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
            if (EquippedWeapon)
            {
                EquippedWeapon.Initialize(this, weaponScriptable);

                PlayerEvents.Invoke_OnWEaponEquipped(EquippedWeapon);

                GripIKLocation = EquippedWeapon.GripLocation;
                PlayerAnimator.SetInteger(WeaponTypeHash, (int)EquippedWeapon.WeaponInformation.WeaponType);
            }
        }
    }

    private WeaponComponent EquippedWeapon;


    private static readonly int AimHorizontalHash = Animator.StringToHash("AimHorizontal");
    private static readonly int AimVerticalHash = Animator.StringToHash("AimVertical");
    private static readonly int IsReloadingHash = Animator.StringToHash("IsReloading");
    private static readonly int IsFiringHash = Animator.StringToHash("IsFiring");
    private static readonly int WeaponTypeHash = Animator.StringToHash("WeaponType");

    private void Awake()
    {
        PlayerAnimator = GetComponent<Animator>();
        PlayerController = GetComponent<PlayerController>();
        if (PlayerController)
        {
            PlayerCrosshair = PlayerController.CrossHair;
        }

        ViewCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (WeaponToSpawn) EquipWeapon(WeaponToSpawn);

        //GameObject spawnedWeapon = Instantiate(WeaponToSpawn.ItemPrefab, WeaponSocketLocation.position, WeaponSocketLocation.rotation, WeaponSocketLocation);

        //if (spawnedWeapon)
        //{
        //    EquippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
        //    if (EquippedWeapon)
        //    {
        //        EquippedWeapon.Initialize(this, WeaponToSpawn);

        //        PlayerEvents.Invoke_OnWEaponEquipped(EquippedWeapon);

        //        GripIKLocation = EquippedWeapon.GripLocation;
        //        PlayerAnimator.SetInteger(WeaponTypeHash, (int)EquippedWeapon.WeaponInformation.WeaponType);
        //    }
        //}
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (GripIKLocation == null) return;

        PlayerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        PlayerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, GripIKLocation.position);
    }


    public void OnReload(InputValue pressed)
    {
        if (EquippedWeapon == null) return;

        StartReloading();

    }

    public void StartReloading()
    {
        if (EquippedWeapon == null) return;

        if (EquippedWeapon.WeaponInformation.BulletsAvailable <= 0 && PlayerController.IsFiring)
        {
            StopFiring();
            return;
        }

        if (PlayerController.IsFiring)
        {
            WasFiring = true;
            StopFiring();

            

        }

        PlayerController.IsReloading = true;
        PlayerAnimator.SetBool(IsReloadingHash, true);
        EquippedWeapon.StartReloading();

        InvokeRepeating(nameof(StopReloading), 0.0f, 0.1f);
    }

    public void StopReloading()
    {
        if (EquippedWeapon == null) return;

        if (PlayerAnimator.GetBool(IsReloadingHash)) return;

        PlayerController.IsReloading = false;
        EquippedWeapon.StopReloading();
        CancelInvoke(nameof(StopReloading));

        if (WasFiring && FiringPressed)
        {
            StartFiring();
            WasFiring = false;
        }
    }


    public void OnFire(InputValue pressed)
    {
        if (EquippedWeapon == null) return;

        FiringPressed = pressed.isPressed;
        if (pressed.isPressed)
        {
            StartFiring();
        }
        else
        {
            StopFiring();
        }
        //Debug.Log("Firing");
    }
    private void StartFiring()
    {
        if (EquippedWeapon == null) return;


        //TODO: wepon seems to reload after no bullets left
        if (EquippedWeapon.WeaponInformation.BulletsAvailable <= 0 && EquippedWeapon.WeaponInformation.BulletsInClip <= 0) return;
        PlayerController.IsFiring = true;
        PlayerAnimator.SetBool(IsFiringHash, true);
        EquippedWeapon.StartFiringWeapon();
    }


    private void StopFiring()
    {
        if (EquippedWeapon == null) return;

        PlayerController.IsFiring = false;
        PlayerAnimator.SetBool(IsFiringHash, false);
        EquippedWeapon.StopFiringWeapon();

    }

    public void OnLook(InputValue delta)
    {
        Vector3 independentMousePosition = ViewCamera.ScreenToViewportPoint(PlayerCrosshair.CurrentAimPosition);
        //Debug.Log(independentMousePosition);

        PlayerAnimator.SetFloat(AimHorizontalHash, independentMousePosition.x);
        PlayerAnimator.SetFloat(AimVerticalHash, independentMousePosition.y);

    }

    

}
