using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SelectedTilePanelMngr : MonoBehaviour {

    private HexMapTile selectedTile;

    public Text xCoordLabel;
    public Text yCoordLabel;
    public Dropdown typeDropdown;

    public void SetSelectedTile(HexMapTile tile)
    {
        selectedTile = tile;
        gameObject.SetActive(selectedTile != null);
        if (selectedTile != null)
        {
            xCoordLabel.text = selectedTile.TileCoords.x.ToString();
            yCoordLabel.text = selectedTile.TileCoords.z.ToString();
            typeDropdown.value = (int)selectedTile.HexTileData.TileType;
        } else
        {
            typeDropdown.value = 0;
        }
        
    }

    void Start()
    {
        List<Dropdown.OptionData> typeOptions = new List<Dropdown.OptionData>();
        foreach(HexTileTypes tileType in HexTileTypes.GetValues(typeof(HexTileTypes)))
        {
            Dropdown.OptionData newOption = new Dropdown.OptionData();
            newOption.text = tileType.ToString();
            typeOptions.Add(newOption);
        }
        typeDropdown.AddOptions(typeOptions);
        typeDropdown.onValueChanged.AddListener(delegate { onTypeValueChanged(typeDropdown); });
    }

    private void onTypeValueChanged(Dropdown drop)
    {
        selectedTile.SetTileType((HexTileTypes)drop.value);
    }

}
