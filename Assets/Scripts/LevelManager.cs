using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{

    private string levelName;
    private GameData gameData;
    private GameHUD gameHUD;
    private Cursor cursor;
    //private MusicPlayer musicPlayer;

    //public GoogleAnalyticsV3 googleAnalytics;


    void Awake()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
        gameHUD = GameObject.FindObjectOfType<GameHUD>();
        //musicPlayer = GameObject.FindObjectOfType<MusicPlayer>();
        
    }

    public void LoadLevel(string name)
    {

        //gameData.PauseGame(false);
        
        levelName = name;
        if (levelName.IndexOf("Level") >= 0)
        {
            //musicPlayer.SetInMenu(false);
        }
        else
        {
            //musicPlayer.SetInMenu(true);
            //we are in a menu screen
            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
            {
                Cursor.visible = true;
            }
        }
        SceneManager.LoadScene(levelName);
    }

    public void StartGame()
    {
        //Screen.showCursor = false;
        gameData.PauseGame(false);
        gameData.ResetPlayerLives();
        gameData.ResetPlayerScore();
        //musicPlayer.SetInMenu(false);
        gameData.SetPlayerReady(false);
        
        SceneManager.LoadScene(1);
    }

    public void RestartLevel()
    {
        if (gameData.GetPlayerRemainingLives() <= 0)
        {
            gameData.ResetPlayerScore();
            gameData.ResetPlayerLives();
        }
        gameData.SetPaddle("right", false);
        gameData.SetPaddle("left", false);
        gameData.SetPlayerReady(false);

        gameData.PauseGame(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetPlayer()
    {
        //gameData.PauseGame(false);
        gameData.SetPaddle("right", false);
        gameData.SetPaddle("left", false);
        gameData.SetPlayerReady(false);
    }

    public void LoadNextLevel()
    {
        //musicPlayer.SetInMenu(false);
        gameData.PauseGame(false);
        gameData.SetPlayerReady(false);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        LoadLevel("StartScreen");
        gameData.SetPlayerReady(false);
        //musicPlayer.SetInMenu(true);
    }

    public void QuitRequest()
    {
        Application.Quit();
    }

    public void ShowLevelComplete()
    {
        gameData.SetPaddle("right", false);
        gameData.SetPaddle("left", false);
        //musicPlayer.SetInMenu(true);
        gameData.PauseGame(true);
        gameHUD.ShowLevelComplete();
        gameData.SetPlayerReady(false);
        gameData.GainOneLife();
    }


}
