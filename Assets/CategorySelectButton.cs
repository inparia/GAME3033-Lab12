using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelectButton : MonoBehaviour
{
    [SerializeField]
    private ItemCategory Category;

    //references
    private Button SelectButton;
    private InvenotryWidget InvenotryWidget;

    private void Awake()
    {
        SelectButton = GetComponent<Button>();
        SelectButton.onClick.AddListener(OnClick);
    }

    public void Initialize(InvenotryWidget invenotryWidget)
    {
        InvenotryWidget = invenotryWidget;
    }


    private void OnClick()
    {
        if (!InvenotryWidget) return;

        InvenotryWidget.SelectCategory(Category);
    }
}
