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
        //selectedTilePanel.gameObject.SetActive(selectedTile != null);
        


    }

    public void SaveMapData()
    {
        Debug.Log("Save");
        FileManager.Save(MapManager.Instance.MapData);
    }

    public void AddTile(HexMapTile tempTile)
    {
        MapManager.Instance.hexMap.AddTile(tempTile);

    }

    public void LoadMapData()
    {
        Debug.Log("Load");
        HexMap newMap = new HexMap();
        newMap = FileManager.LoadMapData(0);
        MapManager.Instance.MapData = newMap;
        MapManager.Instance.mapSpawner.DrawMapTiles();
    }

    public void DrawEmptyMap()
    {
        mapSpawner.DrawEmptyMap();
    }

}
