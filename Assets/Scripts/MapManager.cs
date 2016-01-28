using UnityEngine;
using System.Collections;
using System;

public class MapManager : MonoBehaviour {

    private HexMapTile selectedTile;
    private static MapManager _instance;
    private HexMap hexMap;
    private HexMapSpawner mapSpawner;

    public bool inEdtiorMode = false;
    public SelectedTilePanelMngr selectedTilePanel;

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


	// Use this for initialization
	void Start () {
        Debug.Log("MapManager: start");
        hexMap = new HexMap();
        mapSpawner = new HexMapSpawner();
        FileManager.CacheMapData();
        
    }
	
	// Update is called once per frame
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
        saveFile.MapName = "New Derp";
        int savedTileCount = 0;
        for (int i = 0; i < hexMap.HexTiles.Count; i++)
        {
            HexMapTile currTile = hexMap.HexTiles[i];
            TileData tempData = new TileData();
            tempData.TileType = currTile.HexTileData.TileType;
            tempData.TileCoordX = currTile.TileCoords.x;
            tempData.TileCoordY = currTile.TileCoords.y;
            tempData.TileCoordZ = currTile.TileCoords.z;
            Debug.Log("Saved tile: " + currTile.TileCoords.x + "," + currTile.TileCoords.z);
            saveFile.HexTiles.Add(tempData);
            savedTileCount++;
        }
        FileManager.Save(saveFile);
        Debug.Log("Saved " + savedTileCount + " total tiles");
    }

    public void AddTile(HexMapTile tempTile)
    {
        MapManager.Instance.hexMap.AddTile(tempTile);
    }

    public void ClearMapTiles()
    {
        MapManager.Instance.mapSpawner.ClearMapRoot();
        MapManager.Instance.mapSpawner.DrawMapRoot();
    }

    public void ShowLoadMapDialog()
    {
        SavedGamesPanel.Instance.Show();
    }

    public void LoadMapData(int mapNumber)
    {
        int mapCount = FileManager.LoadedMapCount();
        if(mapNumber > mapCount)
        {
            Debug.Log("Map index out of range");
            return;
        }
        Debug.Log("Load Map #" + mapNumber);

        SerializableMap loadedData = new SerializableMap();
        HexMap newMap = new HexMap();
        loadedData = FileManager.LoadMapData(mapNumber);
        Debug.Log(loadedData.MapName);
        MapManager.Instance.ClearMapTiles();
        for (int i = 0; i < loadedData.HexTiles.Count; i++)
        {
            Debug.Log(loadedData.HexTiles[i].TileCoordX + "'" + loadedData.HexTiles[i].TileCoordZ + " type:" + loadedData.HexTiles[i].TileType);
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

}
