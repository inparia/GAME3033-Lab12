using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayPanel : MonoBehaviour
{
    private RectTransform RectTransform;

    [SerializeField]
    private GameObject ItemSlotPrefab;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        WipeChildren();
    }

    public void PopulatePanel(List<ItemScriptable> itemList)
    {
        WipeChildren();

        foreach (ItemScriptable item in itemList)
        {
            IconSlot Icon = Instantiate(ItemSlotPrefab, RectTransform).GetComponent<IconSlot>();
            Icon.Initialize(item);
        }

    }


    private void WipeChildren()
    {
        foreach (RectTransform child in RectTransform)
        {
            Destroy(child.gameObject);
        }
        RectTransform.DetachChildren();
    }
}
