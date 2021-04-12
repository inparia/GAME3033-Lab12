using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotEquippedWidget : MonoBehaviour
{
    EquipableScriptable Equipable;
    [SerializeField]
    private Image EnabledImage;

    private void Awake()
    {
        HideWidget();
    }

    public void ShowWidget()
    {
        gameObject.SetActive(true);
    }

    public void HideWidget()
    {
        gameObject.SetActive(false);

    }

    internal void Initialize(ItemScriptable item)
    {
        if (!(item is EquipableScriptable eqItem)) return;

        Equipable = eqItem;
        ShowWidget();
        Equipable.OnEquipStatusChange += OnEquipmentChanged;
        OnEquipmentChanged();
    }

    private void OnEquipmentChanged()
    {
        EnabledImage.gameObject.SetActive(Equipable.Equipped);
        
    }

    private void OnDisable()
    {
        if (Equipable) Equipable.OnEquipStatusChange -= OnEquipmentChanged;
        
    }
}
