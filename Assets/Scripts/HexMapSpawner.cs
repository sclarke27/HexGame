using UnityEngine;
using System.Collections.Generic;

public class HexMapSpawner {

    private HexMapTile baseTileObject;
    private bool mapDirty = true;
    private int mapRadius = 0;
    private GameObject mapRootObject;

    public HexMapSpawner() {
        Debug.Log("Hex Map Spawner: start");
        baseTileObject = GameObject.FindObjectOfType<HexMapTile>();
        baseTileObject.gameObject.SetActive(false);

    }

    public void DrawMapRoot()
    {
        mapRadius = 3;

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
        return GameObject.Instantiate(baseTileObject, new Vector3(0, 0, 0), baseTileObject.transform.localRotation) as HexMapTile;
    }

    public void RefreshMapTiles()
    {
        MapManager mapManager = MapManager.Instance;

        int tilecount = 0;
        baseTileObject.gameObject.SetActive(true);
        foreach (HexMapTile mapTile in mapManager.MapData.HexTiles)
        {
            HexMapTile tempTile = mapTile;
            tempTile.transform.parent = mapRootObject.transform;
            tempTile.gameObject.SetActive(true);
            tempTile.SetTileCoords(new Vector3(mapTile.TileCoords.x, 0f, mapTile.TileCoords.z));
            tempTile.SetTileType(mapTile.HexTileData.TileType);
            tilecount++;
        }
        Debug.Log("Total Tites:" + tilecount + " refreshed");
        baseTileObject.gameObject.SetActive(false);
    }

    public float FindTileDistance(Vector3 tile1, Vector3 tile2)
    {
        return (Mathf.Abs(tile1.x - tile2.x) + Mathf.Abs(tile1.y - tile2.y) + Mathf.Abs(tile1.z - tile2.z)) / 2;
    }

    public void DrawEmptyMap()
    {
        ClearMapRoot();
        DrawMapRoot();

        int tilecount = 0;
        

        baseTileObject.gameObject.SetActive(true);
        MapManager mapManager = MapManager.Instance;


        for(int x = -mapRadius; x <= mapRadius; x++)
        {
            for (int y = -mapRadius; y <= mapRadius; y++)
            {
                HexMapTile tempTile = SpawnNewTile();
                tempTile.transform.parent = mapRootObject.transform;
                tempTile.SetTileCoords(new Vector3(x, 0f, y));
                tempTile.SetTileType(HexTileTypes.NONE);
                mapManager.AddTile(tempTile);
                tilecount++;
            }
        }

        Debug.Log("Total Tiles:" + tilecount);
        baseTileObject.gameObject.SetActive(false);
    }


    void Update () {
        if (mapDirty)
        {
            mapDirty = false;
            RefreshMapTiles();
            
        }

	}
}
