using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    private static MapManager _instance;
    private HexMapTile selectedTile;
    private HexMap hexMap;
    private HexMapSpawner mapSpawner;
    private int loadedMapFileIndex = -1;

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
        MapManager.Instance.hexMap.AddTile(tempTile);
    }

    public void ClearMapTiles()
    {
        MapManager.Instance.mapSpawner.ClearMapRoot();
        //MapManager.Instance.mapSpawner.DrawMapRoot();
    }

    public void LoadMapData(int mapNumber)
    {
        MapManager.Instance.ClearMapTiles();

        int mapCount = LoadedMapcount;
        if(mapNumber > mapCount)
        {
            Debug.Log("Map index out of range");
            return;
        }
        LoadedMapFileIndex = mapNumber;
        Debug.Log("Load Map #" + LoadedMapFileIndex);
        SerializableMap loadedData = new SerializableMap();
        HexMap newMap = new HexMap();
        loadedData = FileManager.LoadMapData(LoadedMapFileIndex);
        Debug.Log(loadedData.MapName);
        mapNameInput.text = loadedData.MapName;
        for (int i = 0; i < loadedData.HexTiles.Count; i++)
        {
            //Debug.Log(loadedData.HexTiles[i].TileCoordX + "'" + loadedData.HexTiles[i].TileCoordZ + " type:" + loadedData.HexTiles[i].TileType);
            HexMapTile tempTile = MapManager.Instance.mapSpawner.SpawnNewTile();
            tempTile.SetTileCoords(new Vector3(loadedData.HexTiles[i].TileCoordX, loadedData.HexTiles[i].TileCoordY, loadedData.HexTiles[i].TileCoordZ));
            tempTile.SetTileType(loadedData.HexTiles[i].TileType);
            newMap.AddTile(tempTile);
        }

        MapManager.Instance.MapData = newMap;
        MapManager.Instance.mapSpawner.DrawMapTiles();

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
