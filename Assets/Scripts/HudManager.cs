using UnityEngine;

public class HudManager : MonoBehaviour {

    private static HudManager _instance;

    public TopMenuManager topMenu;
    public SavedGamesPanel savedGamesPanel;
    public SelectedTilePanelMngr selectedTilePanel;

    public static HudManager Instance
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
	
	void Update () {
	
	}

    public void ShowLoadMapDialog()
    {
        savedGamesPanel.Show();
    }


}
