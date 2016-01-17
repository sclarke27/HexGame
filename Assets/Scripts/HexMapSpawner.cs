using UnityEngine;
using System.Collections;

public class HexMapSpawner : MonoBehaviour {

    private HexMapTile baseTileObject;
    private MeshRenderer baseTileMesh;
    private bool mapDirty = true;
    private int totalTiles = 500;
    private int mapWidth = 0;
    private int totalTilesX = 0;
    private int totalTilesZ = 0;
    private float tileWidth = 0f;
    private GameObject mapRootObject;

	// Use this for initialization
	void Start () {
        Debug.Log("Start Hex Map");
        baseTileObject = GameObject.FindObjectOfType<HexMapTile>();
        baseTileMesh = GameObject.FindObjectOfType<MeshRenderer>();

        tileWidth = 2.0f;
        mapWidth = (int)Mathf.Sqrt((float)totalTiles);
        totalTilesX = Mathf.RoundToInt(mapWidth - (mapWidth * 0.75f));
        totalTilesZ = mapWidth;

        mapRootObject = new GameObject();
        mapRootObject.name = "HexTilesParent";
        mapRootObject.transform.position = new Vector3(0f, 0f, 0f);
    }

    private void DrawMapTiles()
    {
        float startX = 0; //((tileWidth * totalTilesX) / 2) * -1;
        float startZ = 0; //((tileWidth * totalTilesZ) / 2) * -1;
        int tilecount = 0;
        baseTileObject.gameObject.SetActive(true);

		for (var x = -totalTilesX; x < totalTilesX; x++)
        {
			for (var z = -totalTilesZ; z < totalTilesZ; z++)
            {
				int rowNum = (z < 0) ? -z : z;
				float currX = startX + ((tileWidth * (tileWidth * 0.75f)) * x) + ((rowNum%2!=1) ? (tileWidth*0.75f) : 0);
				float currZ = startZ + ((tileWidth / 2.3f) * z);

                HexMapTile tempTile = Instantiate(baseTileObject, new Vector3(0, 0, 0), Quaternion.identity) as HexMapTile;
                tempTile.transform.parent = mapRootObject.transform;
                tempTile.SetTileCoords(new Vector3(x, 0f, z));
                //tempTile.SetTilePos(new Vector3(-currX, 0, -currZ));
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
