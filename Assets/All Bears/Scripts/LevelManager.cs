using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    static bool s_Instantiated;
    public bool DebugMode = false;

    //Variables for the game setup
    public int PlayerCount;
    public int LivesPerRound;
    public int RoundsPerLevel;
    public int LevelsPerGame;

    public GameObject Fader;
    public ScreenFader ScreenFader; 
    private GameObject Canvas;

    //Scenes in the game
    private string CurrentLevelName;
    private string CurrentSceneName;

    //Players Stuff
    public Player[] Players;

    //Levels Stuff
    public string[] LevelNames;
    private List<string> LevelsPlayed;
    private List<string> LevelsLeft;
    private List<string> AllLevels;

    int LevelIndex = 0;

    void Awake()
    {
        if (s_Instantiated)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        s_Instantiated = true;

    }

    void Start () 
    {
        //Create a canvas and level fader
        Canvas = GetComponentInChildren<Canvas>().gameObject;

        //Set up the level lists
        AllLevels = new List<string>();
        LevelsLeft = new List<string>();
        LevelsPlayed = new List<string>();

        if(LevelNames.Length > 0)
        {
            for(int i = 0; i < LevelNames.Length; i++)
            {
                AllLevels.Add(LevelNames[i]);
            }
            CurrentLevelName = AllLevels[Random.Range(0, LevelNames.Length)];
        }

        if (DebugMode)
        {
            CreateDebugPlayers();
            print("DebugMode");
        }
    }
    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void LoadNextLevel()
    {
        //print(AllLevels[LevelIndex]);
        if(LevelIndex >= AllLevels.Count - 1)
        {
            LevelIndex = 0;
        }
        ScreenFader.LoadScene(AllLevels[LevelIndex]);
        SceneManager.LoadScene(AllLevels[LevelIndex]);
        //SceneManager.LoadScene("IceArena");
        LevelIndex++;
    }

    public void FadeScreen()
    {
        ScreenFader.FadeScreen();
    }

    public void FadeScreenI()
    {
        ScreenFader.FadePanelOut();
    }
    public void PassPlayersFromLobby(List<Player> players)
    {
        Players = new Player[players.Count];
        players.CopyTo(Players);
        PlayerCount = players.Count;

        if (Settings.bDualScreensEnabled)
        {
            int monitor_count = Display.displays.Length;

             for (int i = 1; i < monitor_count; i++)
             {
                 Display.displays[i].Activate();
             }

            if (PlayerCount == 2)
            {
                Camera cam1 = Players[0].GetComponent<HoverCarControl>().BearCamera;
                Camera cam2 = Players[1].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();

                Rect rect = new Rect();
                rect.x = 0.0f;
                rect.width = 1.0f;
                rect.y = 0.0f;
                rect.height = 1.0f;
                cam1.rect = rect;
                cam1.targetDisplay = 0;
                Players[0].GetComponent<HoverCarControl>().cameraRect = rect;

                rect.x = 0.0f;
                rect.width = 1.0f;
                rect.y = 0.0f;
                rect.height = 1.0f;
                cam2.rect = rect;
                cam2.targetDisplay = 1;
                Players[1].GetComponent<HoverCarControl>().cameraRect = rect;
            }

            else if (PlayerCount == 3)
            {
                Camera cam1 = Players[0].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                Camera cam2 = Players[1].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                Camera cam3 = Players[2].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                CameraSetter.Set1of4(cam1, Players[0].GetComponent<HoverCarControl>());
                CameraSetter.Set2of4(cam2, Players[1].GetComponent<HoverCarControl>());
                CameraSetter.Set3of4(cam3, Players[2].GetComponent<HoverCarControl>());

                Rect rect = new Rect();
                rect.x = 0.0f;
                rect.width = 1.0f;
                rect.y = 0.0f;
                rect.height = 1.0f;
                cam1.rect = rect;
                cam1.targetDisplay = 0;
                Players[0].GetComponent<HoverCarControl>().cameraRect = rect;

                rect.x = 0.0f;
                rect.width = .5f;
                rect.y = 0.0f;
                rect.height = .5f;
                cam2.rect = rect;
                cam2.targetDisplay = 1;
                Players[1].GetComponent<HoverCarControl>().cameraRect = rect;

                rect.x = 0.5f;
                rect.width = 1.0f;
                rect.y = 0.5f;
                rect.height = 1.0f;
                cam3.rect = rect;
                cam3.targetDisplay = 1;
                Players[2].GetComponent<HoverCarControl>().cameraRect = rect;
            }

            else if (PlayerCount == 4)
            {
                Camera cam1 = Players[0].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                Camera cam2 = Players[1].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                Camera cam3 = Players[2].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                Camera cam4 = Players[3].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                CameraSetter.Set1of4(cam1, Players[0].GetComponent<HoverCarControl>());
                CameraSetter.Set2of4(cam2, Players[1].GetComponent<HoverCarControl>());
                CameraSetter.Set3of4(cam3, Players[2].GetComponent<HoverCarControl>());
                CameraSetter.Set4of4(cam4, Players[3].GetComponent<HoverCarControl>());

                Rect rect = new Rect();
                rect.x = 0.0f;
                rect.width = .5f;
                rect.y = 0.0f;
                rect.height = 1.0f;
                cam1.rect = rect;
                cam1.targetDisplay = 0;
                Players[0].GetComponent<HoverCarControl>().cameraRect = rect;

                rect.x = 0.5f;
                rect.width = 1.0f;
                rect.y = 0.0f;
                rect.height = 1.0f;
                cam2.rect = rect;
                cam2.targetDisplay = 0;
                Players[1].GetComponent<HoverCarControl>().cameraRect = rect;

                rect.x = 0.0f;
                rect.width = .5f;
                rect.y = 0.0f;
                rect.height = 1.0f;
                cam3.rect = rect;
                cam3.targetDisplay = 1;
                Players[2].GetComponent<HoverCarControl>().cameraRect = rect;

                rect.x = 0.5f;
                rect.width = 1.0f;
                rect.y = 0.0f;
                rect.height = 1.0f;
                cam4.rect = rect;
                cam4.targetDisplay = 1;
                Players[3].GetComponent<HoverCarControl>().cameraRect = rect;
            }
        }
        else
        {
            if (PlayerCount == 2)
            {
                Camera cam1 = Players[0].GetComponent<HoverCarControl>().BearCamera;
                Camera cam2 = Players[1].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                CameraSetter.Set1of2(cam1, Players[0].GetComponent<HoverCarControl>());
                CameraSetter.Set2of2(cam2, Players[1].GetComponent<HoverCarControl>());
            }

            else if (PlayerCount == 3)
            {
                Camera cam1 = Players[0].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                Camera cam2 = Players[1].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                Camera cam3 = Players[2].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                CameraSetter.Set1of4(cam1, Players[0].GetComponent<HoverCarControl>());
                CameraSetter.Set2of4(cam2, Players[1].GetComponent<HoverCarControl>());
                CameraSetter.Set3of4(cam3, Players[2].GetComponent<HoverCarControl>());
            }

            else if (PlayerCount == 4)
            {
                Camera cam1 = Players[0].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                Camera cam2 = Players[1].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                Camera cam3 = Players[2].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                Camera cam4 = Players[3].GetComponent<HoverCarControl>().HoverFollowCamera.GetComponent<Camera>();
                CameraSetter.Set1of4(cam1, Players[0].GetComponent<HoverCarControl>());
                CameraSetter.Set2of4(cam2, Players[1].GetComponent<HoverCarControl>());
                CameraSetter.Set3of4(cam3, Players[2].GetComponent<HoverCarControl>());
                CameraSetter.Set4of4(cam4, Players[3].GetComponent<HoverCarControl>());
            }
        }


        foreach (Player player in Players)
        {
            DontDestroyOnLoad(player.gameObject);
            DontDestroyOnLoad(player.GetComponent<HoverCarControl>().BearCamera.gameObject);
            ScoreboardManager.Scoreboard.AddPlayer(player.gameObject.name);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("IceArena");
        //SceneManager.LoadScene(LevelNames[Random.Range(0, 3)]);

    }

    public void CreateDebugPlayers()
    {
        Player[] players = FindObjectsOfType<Player>();
        List<Player> tempList = new List<Player>(players);
        int i = 1;
        foreach (Player player in tempList)
        {
            player.PlayerNumber = i;
            player.InputNumber = i;
            player.InitialiseInput(i);
            i++;

            player.CreateScoreboardVerticalScreen();
            player.Scoreboard.SubscribeToManager();
        }

        PassPlayersFromLobby(tempList);
    }

    public void ShowScoreBoard(Player player)
    {

    }

    public void Quit()
    {
        Application.Quit();
    }


    public void EnableDualMonitors()
    {
        int monitor_count = Display.displays.Length;

#if !UNITY_EDITOR
                 for (int i = 1; i < monitor_count; i++)
                 {
                     Display.displays[i].Activate();
                 }
#endif

        Settings.bDualScreensEnabled = true;
    }
}
