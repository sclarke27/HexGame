using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CustomMenuItem {
    private string _selectionHandler;
    private string _menuLabelText;

    public string MenuLabelText
    {
        get { return _menuLabelText; }
        set { _menuLabelText = value; }
    }

    public string SelectionHandler
    {
        get { return _selectionHandler; }
        set { _selectionHandler = value; }
    }

    public CustomMenuItem(string labelText, string handerKey)
    {
        _menuLabelText = labelText;
        _selectionHandler = handerKey;
    }

}

public class TopMenuManager : MonoBehaviour {

    public ArrayList menuItemsData = new ArrayList();

    public Dropdown[] menuDropDowns;
    //private List<CustomMenuItem> fileMenu = new List<CustomMenuItem>();

    void Start()
    {
        LoadMenuItems();
    }

    private void LoadMenuItems ()
    {
        List<CustomMenuItem> fileMenu = new List<CustomMenuItem>();
        fileMenu.Add(new CustomMenuItem("New Map", "newMap"));
        fileMenu.Add(new CustomMenuItem("Load Map", "loadMap"));
        fileMenu.Add(new CustomMenuItem("Save Map", "saveMap"));
        fileMenu.Add(new CustomMenuItem("Clear Map", "clearMap"));
        fileMenu.Add(new CustomMenuItem("Exit", "exitEditor"));

        menuItemsData.Add(fileMenu);

        List<Dropdown.OptionData> typeOptions = new List<Dropdown.OptionData>();
        int index = 0;

        foreach (CustomMenuItem menuItem in fileMenu)
        {
            Dropdown.OptionData newOption = new Dropdown.OptionData();
            newOption.text = menuItem.MenuLabelText;
            typeOptions.Add(newOption);
            index++;
        }
        menuDropDowns[0].AddOptions(typeOptions);
        menuDropDowns[0].onValueChanged.AddListener(delegate { onMenuItemSelect(menuDropDowns[0], fileMenu[menuDropDowns[0].value]); });

        List<CustomMenuItem> selectMenu = new List<CustomMenuItem>();
        selectMenu.Add(new CustomMenuItem("Select All", "selectAll"));
        selectMenu.Add(new CustomMenuItem("Select Neighboring", "selectNeighboring"));
        selectMenu.Add(new CustomMenuItem("Select None", "selectNone"));

        menuItemsData.Add(fileMenu);

        typeOptions = new List<Dropdown.OptionData>();
        index = 0;

        foreach (CustomMenuItem menuItem in selectMenu)
        {
            Dropdown.OptionData newOption = new Dropdown.OptionData();
            newOption.text = menuItem.MenuLabelText;
            typeOptions.Add(newOption);
            index++;
        }
        menuDropDowns[1].AddOptions(typeOptions);
        menuDropDowns[1].onValueChanged.AddListener(delegate { onMenuItemSelect(menuDropDowns[1], selectMenu[menuDropDowns[1].value]); });

    }

    public void onMenuItemSelect(Dropdown selectedDropdown, CustomMenuItem selectedItem)
    {
        Debug.Log("Menu Selected: " + selectedItem.MenuLabelText);
        MapManager mapManager = MapManager.Instance; ;
        HudManager hudManager = HudManager.Instance;
        switch (selectedItem.SelectionHandler)
        {
            case "newMap":
                mapManager.DrawEmptyMap();
                break;
            case "loadMap":
                hudManager.ShowLoadMapDialog();
                break;
            case "saveMap":
                mapManager.SaveMapData();
                break;
            case "clearMap":
                mapManager.ClearMapTiles();
                break;
            case "exitEditor":
                GameObject.FindObjectOfType<LevelManager>().MainMenu();
                break;

            case "selectNeighboring":
                mapManager.SelectNeighborTiles();
                break;
            case "selectNone":
                mapManager.SelectNone();
                break;
            case "selectAll":
                mapManager.SelectAll();
                break;
        }
    }
}
