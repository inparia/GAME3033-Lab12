using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemScriptable : ScriptableObject
{
    public string Name = "Item";

    public ItemCategory ItemCategory = ItemCategory.None;

    public GameObject ItemPrefab;
    public bool Stackable;
    public int MaxStack =1;

    public delegate void AmountChange();
    public event AmountChange OnAmountChange;

    public delegate void ItemDestroyed();
    public event ItemDestroyed OnItemDestroyed;

    public delegate void ItemDropped();
    public event ItemDropped OnItemDropped;

    public int Amount => m_Amount;
    private int m_Amount = 1;

    public PlayerController Controller { get; private set; }


    public virtual void Initialize(PlayerController controller)
    {
        Controller = controller;
    }

    public abstract void UseItem(PlayerController controller);

    public virtual void DeleteItem(PlayerController controller)
    {
        OnItemDestroyed?.Invoke();
        controller.Inventory.DeleteItem(this);
    }

    public virtual void DropItem(PlayerController controller)
    {
        OnItemDropped?.Invoke();
    }

    public void ChangeAmount(int amount)
    {
        m_Amount += amount;
        OnAmountChange?.Invoke();
    }

    public void SetAmount(int amount)
    {
        m_Amount = amount;
        OnAmountChange?.Invoke();
    }

}


public enum ItemCategory
{
    None,
    Weapon,
    Consumable,
    Equipment,
    Ammo
}
