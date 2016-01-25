using UnityEngine;
using System.Collections;

public class HexMapSpawner {

    private HexMapTile baseTileObject;
    //private MeshRenderer baseTileMesh;
    private bool mapDirty = true;
    private int totalTiles = 500;
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

    private void DrawMapRoot()
    {
        mapWidth = (int)Mathf.Sqrt((float)totalTiles);
        totalTilesX = Mathf.RoundToInt(mapWidth - (mapWidth * 0.75f));
        totalTilesZ = mapWidth;

        mapRootObject = new GameObject();
        mapRootObject.name = "HexTilesParent";
        mapRootObject.transform.position = new Vector3(0f, 0f, 0f);

    }

    private void ClearMapRoot()
    {
        if(mapRootObject != null)
        {
            GameObject.Destroy(mapRootObject);
        }
        
        mapRootObject = null;
    }

    public void DrawMapTiles()
    {
        ClearMapRoot();
        DrawMapRoot();

        MapManager mapManager = MapManager.Instance;
        HexMap mapData = mapManager.MapData;
        foreach(HexMapTile tileData in mapManager.MapData.HexTiles)
        {
            HexMapTile tempTile = GameObject.Instantiate(tileData, new Vector3(0, 0, 0), Quaternion.identity) as HexMapTile;
            tempTile.transform.parent = mapRootObject.transform;
            tempTile.SetTileCoords(new Vector3(tileData.TileCoords.x, 0f, tileData.TileCoords.z));
        }
        /*
        int tilecount = 0;
        baseTileObject.gameObject.SetActive(true);
        MapManager mapManager = MapManager.Instance;

		for (var x = -totalTilesX; x < totalTilesX; x++)
        {
			for (var z = -totalTilesZ; z < totalTilesZ; z++)
            {
                HexMapTile tempTile = GameObject.Instantiate(baseTileObject, new Vector3(0, 0, 0), Quaternion.identity) as HexMapTile;
                tempTile.transform.parent = mapRootObject.transform;
                tempTile.SetTileCoords(new Vector3(x, 0f, z));
                mapManager.AddTile(tempTile);
                tilecount++;
            }

        }
        Debug.Log("Total Tites:" + tilecount);
        baseTileObject.gameObject.SetActive(false);
        */
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
                HexMapTile tempTile = GameObject.Instantiate(baseTileObject, new Vector3(0, 0, 0), Quaternion.identity) as HexMapTile;
                tempTile.transform.parent = mapRootObject.transform;
                tempTile.SetTileCoords(new Vector3(x, 0f, z));
                mapManager.AddTile(tempTile);
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
