using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private string StartingMenuName = "Main Menu";
    [SerializeField]
    private string RootMenu = "Main Menu";

    private MenuWidget ActiveWidget;

    private Dictionary<string, MenuWidget> Menus = new Dictionary<string, MenuWidget>();
    // Start is called before the first frame update
    void Start()
    {
        DisableAllMenus();
        EnableMenu(StartingMenuName);

        AppEvents.Invoke_OnMouseCursorEnable(true);
    }

    

    public void AddMenu(string menuName, MenuWidget menuWidget)
    {
        if (string.IsNullOrEmpty(menuName)) return;

        if (Menus.ContainsKey(menuName))
        {
            Debug.LogError("menu already exists in dictionary!");
            return;
        }

        if (menuWidget == null) return;

        Menus.Add(menuName, menuWidget);
    }

    public void EnableMenu(string menuName)
    {
        if (string.IsNullOrEmpty(menuName)) return;

        if (Menus.ContainsKey(menuName))
        {
            DisableActiveMenu();

            ActiveWidget = Menus[menuName];
            ActiveWidget.EnableWidget();
        }
        else
        {
            Debug.LogError("Menu is not available in Dictionary!");

        }
    }

    public void DisableMenu(string menuName)
    {
        if (string.IsNullOrEmpty(menuName)) return;

        if (Menus.ContainsKey(menuName))
        {
            Menus[menuName].DisableWidget();
        }
        else
        {
            Debug.LogError("Menu is not available in Dictionary!");

        }
    }

    public void ReturnToRootMenu()
    {
        EnableMenu(RootMenu);
    }

    private void DisableActiveMenu()
    {
        if (ActiveWidget)
        {
        ActiveWidget.DisableWidget();

        }

    }

    private void DisableAllMenus()
    {

        foreach (MenuWidget menu in Menus.Values)
        {
            menu.DisableWidget();
        }

    }
}

public class MenuWidget : MonoBehaviour
{
    [SerializeField]
    private string MenuName;

    protected MenuController MenuController;


    private void Awake()
    {
        MenuController = FindObjectOfType<MenuController>();

        if (MenuController)
        {
            MenuController.AddMenu(MenuName, this);
        }
        else
        {
            Debug.LogError("Menu Controller Not Found!!");
        }
    }

    public void EnableWidget()
    {
        gameObject.SetActive(true);
    }

    public void DisableWidget()
    {
        gameObject.SetActive(false);

    }

    public void ReturnToRootMenu()
    {
        if (MenuController)
        {
            MenuController.ReturnToRootMenu();

        }
    }

}