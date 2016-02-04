using System.Collections.Generic;

public class HexMap {

    private List<HexMapTile> _hexTiles = new List<HexMapTile>();
    private string _mapName = "derp";

    public HexMap()
    {
        _hexTiles = new List<HexMapTile>();
        _mapName = "TestMap";
    }
    
    public List<HexMapTile> HexTiles
    {
        get { return _hexTiles;  }
        set { _hexTiles = value; }
    }

    public string MapName
    {
        get { return _mapName; }
        set { _mapName = value; }
    }

    public void AddTile(HexMapTile newTile)
    {
        //_hexTiles.SetValue(newTile, _hexTiles.Length + 1);
        _hexTiles.Add(newTile);
    }


}
