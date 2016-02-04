using UnityEngine;
using UnityEngine.UI;

public class SavedGamesPanel : MonoBehaviour {

    public ScrollRect scrollField;
    public Button rowTemplate;
    private static SavedGamesPanel _instance;
    private int selectedIndex = 0;

    public static SavedGamesPanel Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start () {
        
        
	}

	
	// Update is called once per frame
	void Update () {
	
	}

    private void ClearContent()
    {
        Component[] buttons = scrollField.content.transform.GetComponentsInChildren(typeof(Button));
        
        for (int i=0; i < buttons.Length; i++)
        {
            DestroyImmediate(buttons[i].gameObject);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        MapManager mapManager = MapManager.Instance;
        ClearContent();
        int rowOffset = -30;
        int index = 0;
        foreach(SerializableMap saveGame in mapManager.CachedMapList)
        {
            Button tempButton = GameObject.Instantiate(rowTemplate, new Vector3(0, 0, 0), Quaternion.identity) as Button;
            tempButton.transform.SetParent(scrollField.content.transform);
            tempButton.transform.localPosition = new Vector3(137.2f, -19f + (rowOffset * index), 0);
            Text buttText = tempButton.GetComponentInChildren<Text>();
            buttText.text = saveGame.MapName;

            tempButton.gameObject.SetActive(true);
            tempButton.name = index.ToString();
            tempButton.onClick.AddListener(delegate { selectedIndex = int.Parse(tempButton.name); });
            index++;
        }
    }

    public void LoadSelectedMap()
    {
        Debug.Log("Load map index: " + selectedIndex);
        MapManager mapManager = MapManager.Instance;
        mapManager.LoadMapData(selectedIndex);
        Close();
    }

    public void Close()
    {
        ClearContent();
        selectedIndex = 0;
        gameObject.SetActive(false);
    }
}
