using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    [SerializeField]
    private List<ItemScriptable> Items = new List<ItemScriptable>();

    [SerializeField]
    private PlayerController Controller;

    private void Awake()
    {
        Controller = GetComponent<PlayerController>();
    }


    public List<ItemScriptable> GetItemList() => Items;

    public int GetItemCount() => Items.Count;

    public ItemScriptable FindItem(string itemName)
    {
        return Items.Find((invItems) => invItems.Name == itemName);
    }

    public void AddItem(ItemScriptable item, int amount = 0)
    {
        int itemIndex = Items.FindIndex(itemScript => itemScript.Name == item.Name);

        if (itemIndex != -1)
        {
            ItemScriptable listItem = Items[itemIndex];
            if (item.Stackable && listItem.Amount < listItem.MaxStack)
            {
                listItem.ChangeAmount(amount <= 0 ? item.Amount : amount);
            }
        }
        else
        {
            if (item == null) return;

            ItemScriptable itemClone = Instantiate(item);
            itemClone.Initialize(Controller);
            itemClone.SetAmount(amount <= 1? item.Amount : amount);
            Items.Add(itemClone);
        }
    }

    public void DeleteItem(ItemScriptable item)
    {
        int itemIndex = Items.FindIndex(listItem => listItem.Name == item.Name);
        if (itemIndex == -1) return;

        Items.Remove(item);
    }

    public List<ItemScriptable> GetItemsOfCategory(ItemCategory itemCategory)
    {
        if (Items == null || Items.Count <= 0) return null;

        return itemCategory == ItemCategory.None ? 
            Items : Items.FindAll(item => item.ItemCategory == itemCategory);
    }

}
