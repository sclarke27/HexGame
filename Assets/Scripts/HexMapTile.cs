using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HexMapTile : MonoBehaviour {

    private MeshRenderer tileMesh;
    private float _tileWidth = 2;
    private float _tileHeight = 2;
	private bool _isSelected = false;
	private Vector3 _tileCoords = new Vector2();
	private Color defaultColor;
    private Canvas uiCanvas;
	public Text coordText;
	public Text posText;

    public Material[] hexMaterials;

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
    
	public Vector3 TileCoords 
	{
		get { return _tileCoords; }
		set { _tileCoords = value; }
	}

    // Use this for initialization
	void Start () {
		tileMesh = transform.GetComponentInChildren<MeshRenderer>();
        uiCanvas = transform.GetComponentInChildren<Canvas>();
        
        if (tileMesh == null) {
			Debug.LogError ("tile mesh not found", tileMesh);
			return;
		}

        Material newMat = hexMaterials[Mathf.RoundToInt(Random.Range(0, hexMaterials.Length))];
        tileMesh.material = newMat;
        defaultColor = tileMesh.GetComponent<Renderer>().material.color;
    }

	// Update is called once per frame
	void Update () {
        //Debug.Log(tileMesh);
	}

	public void SetTilePos(Vector3 pos) {
		transform.position = pos;
		posText.text = Mathf.Round(pos.x) + "," + Mathf.Round(pos.z);
	}

	public void SetTileCoords(Vector3 coords) {
		TileCoords = coords;
		float rowNum = (TileCoords.z < 0) ? -TileCoords.z : TileCoords.z;
		float currX = ((TileWidth * (TileWidth * 0.75f)) * TileCoords.x) + ((rowNum%2!=1) ? (TileWidth*0.75f) : 0);
		float currZ = ((TileWidth / 2.3f) * TileCoords.z);
		SetTilePos (new Vector3 (currX, transform.position.y, currZ));
		coordText.text = TileCoords.x + "," + TileCoords.z;
	}

	void OnMouseDown() {
		Debug.Log ("click");
		IsSelected = !IsSelected;
		Color tempColor = new Color ();
		if (IsSelected) {
            tempColor = new Color(1f, 0f, 0f, tileMesh.material.color.a);
		} else {
			tempColor = defaultColor;
		}
		tileMesh.material.color = tempColor;
	}
}
