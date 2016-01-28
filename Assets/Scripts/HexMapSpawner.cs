using UnityEngine;
using System.Collections;

public class HexMapSpawner {

    private HexMapTile baseTileObject;
    //private MeshRenderer baseTileMesh;
    private bool mapDirty = true;
    private int totalTiles = 100;
    private int mapWidth = 0;
    private int totalTilesX = 0;
    private int totalTilesZ = 0;
    //private float tileWidth = 0f;
    private GameObject mapRootObject;


    // Use this for initialization
    public HexMapSpawner() {
        Debug.Log("Hex Map Spawner: start");
        baseTileObject = GameObject.FindObjectOfType<HexMapTile>();
        //baseTileMesh = GameObject.FindObjectOfType<MeshRenderer>();

    }

    public void DrawMapRoot()
    {
        mapWidth = (int)Mathf.Sqrt((float)totalTiles);
        totalTilesX = Mathf.RoundToInt(mapWidth - (mapWidth * 0.75f));
        totalTilesZ = mapWidth;

        mapRootObject = new GameObject();
        mapRootObject.name = "HexTilesParent";
        mapRootObject.transform.position = new Vector3(0f, 0f, 0f);

    }

    public void ClearMapRoot()
    {
        if(mapRootObject != null)
        {
            GameObject.Destroy(mapRootObject);
        }
        
        mapRootObject = null;
    }

    public HexMapTile SpawnNewTile()
    {
        return GameObject.Instantiate(baseTileObject, new Vector3(0, 0, 0), Quaternion.identity) as HexMapTile;
    }

    public void DrawMapTiles()
    {
        ClearMapRoot();
        DrawMapRoot();

        MapManager mapManager = MapManager.Instance;
        HexMap mapData = mapManager.MapData;

        int tilecount = 0;
        baseTileObject.gameObject.SetActive(true);
        foreach (HexMapTile mapTile in mapManager.MapData.HexTiles)
        {
            HexMapTile tempTile = (mapTile.transform == null) ? SpawnNewTile() : mapTile;
            tempTile.transform.parent = mapRootObject.transform;
            tempTile.gameObject.SetActive(true);
            tempTile.SetTileCoords(new Vector3(mapTile.TileCoords.x, 0f, mapTile.TileCoords.z));
            tempTile.SetTileType(mapTile.HexTileData.TileType);
            tilecount++;
        }
        Debug.Log("Total Tites:" + tilecount + " loaded");
        baseTileObject.gameObject.SetActive(false);
    }

    public void DrawEmptyMap()
    {
        ClearMapRoot();
        DrawMapRoot();

        int tilecount = 0;
        baseTileObject.gameObject.SetActive(true);
        MapManager mapManager = MapManager.Instance;

        for (var x = -totalTilesX; x < totalTilesX; x++)
        {
            for (var z = -totalTilesZ; z < totalTilesZ; z++)
            {
                HexMapTile tempTile = SpawnNewTile();
                tempTile.transform.parent = mapRootObject.transform;
                mapManager.AddTile(tempTile);
                tempTile.SetTileCoords(new Vector3(x, 0f, z));
                tempTile.SetTileType(HexTileTypes.NONE);
                tilecount++;
            }

        }
        Debug.Log("Total Tites:" + tilecount);
        baseTileObject.gameObject.SetActive(false);
    }



    // Update is called once per frame
    void Update () {
        if (mapDirty)
        {
            mapDirty = false;
            DrawMapTiles();
            
        }

	}
}
