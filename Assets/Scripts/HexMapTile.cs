using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

[System.Serializable]
public enum HexTileTypes
{
    GRASS = 4,
    SAND = 3,
    STONE = 2,
    WATER = 1,
    NONE = 0

}

[System.Serializable]
public class TileData
{
    private float _tileCoordX = 0;
    private float _tileCoordY = 0;
    private float _tileCoordZ = 0;
    private HexTileTypes _tileType = HexTileTypes.NONE;

    public HexTileTypes TileType
    {
        get { return _tileType; }
        set { _tileType = value; }
    }

    public float TileCoordX
    {
        get { return _tileCoordX; }
        set { _tileCoordX = value; }
    }

    public float TileCoordY
    {
        get { return _tileCoordY; }
        set { _tileCoordY = value; }
    }

    public float TileCoordZ
    {
        get { return _tileCoordZ; }
        set { _tileCoordZ = value; }
    }

    public TileData ()
    {
        
    }    
}

[System.Serializable]
public class HexMapTile : MonoBehaviour {

    [SerializeField]
    private Vector3 _tileCoords = new Vector3();
    private TileData _tileData;
    private MeshRenderer tileMesh;
    private bool _isSelected = false;
    private float _tileWidth = 2;
    private float _tileHeight = 2;


    private Color defaultColor;
	public Text coordText;
	public Text posText;

    public Material[] hexMaterials;
    private MapManager mapManager;

    public float TileWidth
    {
        get { return _tileWidth; }
        set { _tileWidth = value; }
    }

    public float TileHeight
    {
        get { return _tileHeight; }
        set { _tileHeight = value; }
    }

    public bool IsSelected 
	{
		get { return _isSelected; }
		set { _isSelected = value; }
	}

    [SerializeField]
    public TileData HexTileData
    {
        get { return _tileData; }
        set { _tileData = value; }
    }

    public Vector3 TileCoords
    {
        get { return _tileCoords; }
        set { _tileCoords = value; }
    }

    public HexMapTile()
    {
        _tileData = new TileData();
        _tileCoords = new Vector2();
        
    }

    // Use this for initialization
	void Start () {
        mapManager = MapManager.Instance;
        tileMesh = transform.GetComponentInChildren<MeshRenderer>();
        //uiCanvas = transform.GetComponentInChildren<Canvas>();
        Debug.Log("tile started");
        if (tileMesh == null) {
			Debug.LogError ("tile mesh not found", tileMesh);
			return;
		}

        Material newMat = hexMaterials[Mathf.RoundToInt(Random.Range(0, hexMaterials.Length))];
        //tileMesh.material = newMat;
        defaultColor = tileMesh.GetComponent<Renderer>().material.color;
    }

    private void UpdateTile()
    {
        tileMesh = transform.GetComponentInChildren<MeshRenderer>();
        if (tileMesh == null)
        {
            Debug.LogError("tile mesh STILL not found", tileMesh);
            return;
        }
        switch (HexTileData.TileType)
        {
            case HexTileTypes.GRASS:
                defaultColor = new Color(0f, 1f, 0f, 1f);
                break;
            case HexTileTypes.NONE:
                defaultColor = new Color(1f, 1f, 1f, 1f);
                break;
            case HexTileTypes.SAND:
                defaultColor = new Color(0.83f, 0.63f, 0.56f, 1f);
                break;
            case HexTileTypes.STONE:
                defaultColor = new Color(0.54f, 0.54f, 0.54f, 1f);
                break;
            case HexTileTypes.WATER:
                defaultColor = new Color(0f, 0f, 1f, 1f);
                break;

        }
        if (!IsSelected)
        {
            tileMesh.material.color = defaultColor;
        } else
        {
            tileMesh.material.color = new Color(1f, 0f, 0f, tileMesh.material.color.a); ;
        }
        SetTilePos();
    }

	public void SetTilePos() {
        float rowNum = (TileCoords.z < 0) ? -TileCoords.z : TileCoords.z;
        float currX = ((TileWidth * (TileWidth * 0.75f)) * TileCoords.x) + ((rowNum % 2 != 1) ? (TileWidth * 0.75f) : 0);
        float currZ = ((TileWidth / 2.3f) * TileCoords.z);
        float currY = 0f;
        Vector3 pos = new Vector3(currX, currY, currZ);

        transform.position = pos;
		posText.text = "World Pos: " + pos.x + "," + pos.z;
	}

	public void SetTileCoords(Vector3 coords) {
        TileCoords = coords;
		coordText.text = "Tile Coords: " + TileCoords.x + "," + TileCoords.z;
        UpdateTile();
	}

    public void SetSelected(bool selected)
    {
        Debug.Log("Selected " + TileCoords.x + "," + TileCoords.z);
        IsSelected = selected;
        Color tempColor = new Color();
        if (IsSelected)
        {
            tempColor = new Color(1f, 0f, 0f, tileMesh.material.color.a);
        }
        else {
            tempColor = defaultColor;
        }
        tileMesh.material.color = tempColor;
        UpdateTile();
    }

    public void SetTileType(HexTileTypes newType)
    {
        HexTileData.TileType = newType;
        UpdateTile();
    }

    void OnMouseDown() {
        EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
        if (eventSystem.IsPointerOverGameObject()) return;
            
        mapManager.SelectMapTile(this);
    }
}
