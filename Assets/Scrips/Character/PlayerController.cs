using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerHealthComponent))]
public class PlayerController : MonoBehaviour, IPausable
{

    public bool IsFiring;
    public bool IsReloading;
    public bool IsJumping;
    public bool IsRunning;
    public bool InInventory;
    public CrossHairScript CrossHair => CrossHairComponent;
    [SerializeField] private CrossHairScript CrossHairComponent;


    public HealthComponent Health => HealthComponent;
    [SerializeField] private HealthComponent HealthComponent;

    public InventoryComponent Inventory => InventoryComponent;
    [SerializeField] private InventoryComponent InventoryComponent;


    public WeaponHolder WeaponHolder => WeaponHolderComponent;
    [SerializeField] private WeaponHolder WeaponHolderComponent;

   // [SerializeField]
   // private ConsummableScript Consume;


    private GameUIController UIController;

    private PlayerInput PlayerInput;
    private void Awake()
    {
        UIController = FindObjectOfType<GameUIController>();
        PlayerInput = GetComponent<PlayerInput>();
        if (Health == null)
        {
            HealthComponent = GetComponent<HealthComponent>();
        }
        if (WeaponHolder == null)
        {
            WeaponHolderComponent = GetComponent<WeaponHolder>();
        }
        if (Inventory == null)
        {
            InventoryComponent = GetComponent<InventoryComponent>();
        }
    }

    public void OnPauseGame(InputValue value)
    {
        Debug.Log("Pause Game");
        PauseManager.Instance.PauseGame();
    }

    public void OnUnPauseGame(InputValue value)
    {
        Debug.Log("UnPause Game");
        PauseManager.Instance.UnPauseGame();

    }

    public void OnInventory(InputValue button)
    {
        Debug.Log("Inventory Selected");
        if (InInventory)
        {
            OpenInventory(false);
        }
        else
        {
            OpenInventory(true);

        }

        InInventory = !InInventory;
        
    }

    private void OpenInventory(bool open)
    {
        if (open)
        {
            PauseManager.Instance.PauseGame();
            UIController.EnableInventoryMenu();

        }
        else
        {
            PauseManager.Instance.UnPauseGame();
            UIController.EnableGameMenu();

        }
    }

    public void PauseMenu()
    {
        UIController.EnablePauseMenu();
        PlayerInput.SwitchCurrentActionMap("PauseActionMap");
    }

    public void UnPauseMenu()
    {
        UIController.EnableGameMenu();
        PlayerInput.SwitchCurrentActionMap("PlayerActionMap");
    }
}
