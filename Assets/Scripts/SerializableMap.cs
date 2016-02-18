using System.Collections.Generic;

[System.Serializable]
public class SerializableMap {

    private string _mapName = "derp";
    private List<TileData> _hexTiles = new List<TileData>();
    

    public string MapName
    {
        get { return _mapName; }
        set { _mapName = value; }
    }

    public List<TileData> HexTiles
    {
        get { return _hexTiles; }
        set { _hexTiles = value; }
    }

}
