using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectedTilePanelMngr : MonoBehaviour {

    private HexMapTile selectedTile;

    public Text xCoordLabel;
    public Text yCoordLabel;

    public void SetSelectedTile(HexMapTile tile)
    {
        selectedTile = tile;
        gameObject.SetActive(selectedTile != null);
        if (selectedTile != null)
        {
            xCoordLabel.text = selectedTile.TileCoords.x.ToString();
            yCoordLabel.text = selectedTile.TileCoords.z.ToString();
        }
    }


}
