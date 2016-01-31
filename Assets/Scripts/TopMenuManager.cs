using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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

    public Dropdown[] menuDropDowns;
    private List<CustomMenuItem> fileMenu = new List<CustomMenuItem>();

    void Start()
    {

        fileMenu.Add(new CustomMenuItem("New Map", "newMap"));
        fileMenu.Add(new CustomMenuItem("Load Map", "loadMap"));
        fileMenu.Add(new CustomMenuItem("Save Map", "saveMap"));
        fileMenu.Add(new CustomMenuItem("Exit", "exitEditor"));

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

    }

    public void onMenuItemSelect(Dropdown selectedDropdown, CustomMenuItem selectedItem)
    {
        Debug.Log("test" + selectedItem.MenuLabelText);
        MapManager mapManager = MapManager.Instance; ;
        switch (selectedItem.SelectionHandler)
        {
            case "newMap":
                mapManager.DrawEmptyMap();
                break;
            case "loadMap":
                mapManager.ShowLoadMapDialog();
                break;
            case "saveMap":
                mapManager.SaveMapData();
                break;
            case "exitEditor":
                
                break;
        }
    }
}
