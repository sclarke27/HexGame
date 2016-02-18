using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class MapManager : MonoBehaviour {

    private static MapManager _instance;
    private HexMapTile selectedTile;
    private HexMap hexMap;
    private HexMapSpawner mapSpawner;
    private int loadedMapFileIndex = -1;
    private Dictionary<string, int> _hexTileLookup = new Dictionary<string, int>();

    private List<Vector2> neighborEvenVectors = new List<Vector2>();
    private List<Vector2> neighborOddVectors = new List<Vector2>();


    public bool inEdtiorMode = false;
    public SelectedTilePanelMngr selectedTilePanel;
    public InputField mapNameInput;

    public static MapManager Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public HexMap MapData
    {
        get { return hexMap; }
        set { hexMap = value; }
    }

    public int LoadedMapcount
    {
        get { return FileManager.LoadedMapCount(); }
    }

    public int LoadedMapFileIndex
    {
        get { return loadedMapFileIndex; }
        set { loadedMapFileIndex = value; }
    }

    public Dictionary<string, int> HexTileLookup
    {
        get { return _hexTileLookup; }
        set { _hexTileLookup = value; }
    }


    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            
        } else
        {
            Destroy(gameObject);
        }
    }

	void Start () {
        Debug.Log("MapManager: start");
        //setup even-q lookup vector lists
        neighborEvenVectors.Add(new Vector2(1, -1));
        neighborEvenVectors.Add(new Vector2(1, 0));
        neighborEvenVectors.Add(new Vector2(0, 1));
        neighborEvenVectors.Add(new Vector2(-1, 0));
        neighborEvenVectors.Add(new Vector2(-1, -1));
        neighborEvenVectors.Add(new Vector2(0, -1));

        neighborOddVectors.Add(new Vector2(1, 0));
        neighborOddVectors.Add(new Vector2(1, 1));
        neighborOddVectors.Add(new Vector2(0, 1));
        neighborOddVectors.Add(new Vector2(-1, 1));
        neighborOddVectors.Add(new Vector2(-1, 0));
        neighborOddVectors.Add(new Vector2(0, -1));

        hexMap = new HexMap();
        mapSpawner = new HexMapSpawner();
        FileManager.CacheMapData();
        
    }
	
	void Update () {
	
	}


    public void SelectMapTile(HexMapTile newSelectedTile)
    {
        if (newSelectedTile == selectedTile)
        {
            selectedTile.SetSelected(false);
            selectedTile = null;
            UpdateSelectedTilePanel();
            return;
        }
        if(selectedTile != null)
        {
            selectedTile.SetSelected(false);
        }
        selectedTile = newSelectedTile;
        selectedTile.SetSelected(true);
        UpdateSelectedTilePanel();
    }

    public void SelectNeighborTiles()
    {
        if (selectedTile == null) return;
        List<HexMapTile> neighbors = ReturnNeighborTiles();
        foreach(HexMapTile neighborTile in neighbors)
        {
            neighborTile.SetSelected(true);
        }
        Debug.Log("Found " + neighbors.Count + " neighbor tiles.");
    }

    public List<HexMapTile> ReturnNeighborTiles()
    {
        List<HexMapTile> newTileList = new List<HexMapTile>();

        if (selectedTile == null) return newTileList;

        int tilesFound = 0;
        List<Vector2> vectorLookupTable = (selectedTile.TileCoords.x % 2 == 0) ? MapManager.Instance.neighborEvenVectors : MapManager.Instance.neighborOddVectors;
        foreach (Vector2 neighborVector in vectorLookupTable)
        {
            int lookupIndex = -1;
            Vector2 lookupVector = new Vector2(selectedTile.TileCoords.x, selectedTile.TileCoords.z) + neighborVector;
            
            if(MapManager.Instance.HexTileLookup.TryGetValue(lookupVector.x + "," + lookupVector.y, out lookupIndex))
            {
                newTileList.Add(MapManager.Instance.MapData.GetTile(lookupIndex));
                tilesFound++;
            }
        }


        return newTileList;
    }

    public void SelectNone()
    {
        foreach(HexMapTile tile in MapManager.Instance.hexMap.HexTiles)
        {
            tile.SetSelected(false);
        }
        selectedTile = null;
    }

    public void SelectAll()
    {
        foreach (HexMapTile tile in MapManager.Instance.hexMap.HexTiles)
        {
            tile.SetSelected(true);
        }
        selectedTile = null;
    }

    private void UpdateSelectedTilePanel()
    {
        selectedTilePanel.SetSelectedTile(selectedTile);
    }

    public void SaveMapData()
    {
        Debug.Log("Save");

        SerializableMap saveFile = new SerializableMap();
        saveFile.MapName = mapNameInput.text;

        int savedTileCount = 0;
        for (int i = 0; i < hexMap.HexTiles.Count; i++)
        {
            HexMapTile currTile = hexMap.HexTiles[i];

            TileData tempData = new TileData();
            tempData.TileType = currTile.HexTileData.TileType;
            tempData.TileCoordX = currTile.TileCoords.x;
            tempData.TileCoordY = currTile.TileCoords.y;
            tempData.TileCoordZ = currTile.TileCoords.z;

            saveFile.HexTiles.Add(tempData);
            savedTileCount++;
        }

        if(loadedMapFileIndex < 0)
        {
            FileManager.SaveNew(saveFile);
        } else
        {
            FileManager.SaveAtIndex(loadedMapFileIndex, saveFile);
        }
        
        Debug.Log("Saved " + savedTileCount + " total tiles");
    }

    public void AddTile(HexMapTile tempTile)
    {
        hexMap.AddTile(tempTile);
        HexTileLookup.Add(tempTile.TileCoords.x + "," + tempTile.TileCoords.z, MapManager.Instance.hexMap.HexTiles.Count - 1);
    }

    public void ClearMapTiles()
    {
        mapSpawner.ClearMapRoot();
        selectedTile = null;
        hexMap = new HexMap();
        loadedMapFileIndex = -1;
        _hexTileLookup = new Dictionary<string, int>();

    }

    public void LoadMapData(int mapNumber)
    {
        ClearMapTiles();
        mapSpawner.ClearMapRoot();
        mapSpawner.DrawMapRoot();

        int mapCount = LoadedMapcount;
        if(mapNumber > mapCount)
        {
            Debug.Log("Map index out of range");
            return;
        }
        LoadedMapFileIndex = mapNumber;
        Debug.Log("Load Map #" + LoadedMapFileIndex);
        SerializableMap loadedData = new SerializableMap();
        
        loadedData = FileManager.LoadMapData(LoadedMapFileIndex);
        Debug.Log("Load Named: " + loadedData.MapName);
        mapNameInput.text = loadedData.MapName;
        for (int i = 0; i < loadedData.HexTiles.Count; i++)
        {
            HexMapTile tempTile = mapSpawner.SpawnNewTile();
            tempTile.SetTileCoords(new Vector3(loadedData.HexTiles[i].TileCoordX, loadedData.HexTiles[i].TileCoordY, loadedData.HexTiles[i].TileCoordZ));
            tempTile.SetTileType(loadedData.HexTiles[i].TileType);
            AddTile(tempTile);
        }

        MapData = hexMap;
        mapSpawner.RefreshMapTiles();

    }

    public void DrawEmptyMap()
    {
        mapSpawner.DrawEmptyMap();
    }

    public List<SerializableMap> CachedMapList
    {
        get { return FileManager.CachedMapList; }
    }

}
