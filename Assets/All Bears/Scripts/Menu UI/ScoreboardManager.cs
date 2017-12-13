using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour {

    public static ScoreboardManager Scoreboard;
    static bool Initialised = false;

    public delegate void OnPlayerScoreChange(string username);
    public event OnPlayerScoreChange onPlayerScoreChange;

    public delegate void OnPlayerAdded(string username);
    public event OnPlayerAdded onPlayerAdded;

    public Dictionary<string, Score> playerScores;

    private void Awake()
    {
        if (Scoreboard)
        {
            Destroy(this.gameObject);
        }
        else
            Scoreboard = this;

        Init();
    }
    // Use this for initialization
    void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartScoreboard()
    {

        if(playerScores != null)
        {
            playerScores.Clear();
        }

        foreach(Player player in FindObjectsOfType<Player>())
        {
            ScoreboardManager.Scoreboard.AddPlayer(player.gameObject.name);
        }
    }

    void Init()
    {
        playerScores = new Dictionary<string, Score>();
        Initialised = true;
        DontDestroyOnLoad(this.gameObject);
    }

    public int GetScore(string username, string scoreType)
    {
        if (!Initialised)
        {
            Init();
        }
        return playerScores[username].Score1;
    }

    public void ChangeScore(string username, string scoreType, int amount)
    {
        if (!Initialised)
        {
            Init();
        }
        int currScore = GetScore(username, scoreType);
        SetScore(username, scoreType, currScore + amount);
    }

    public void AddPlayer(string username)
    {
        //print("Addig Player");

        if(!Initialised)
        {
            Init();
        }

        if (playerScores.ContainsKey(username) == false)
        {
            Score tablerow = new Score();
            playerScores.Add(username, tablerow);
            onPlayerAdded(username);
        }
    }

    public void SetScore(string username, string scoreType, int amount)
    {
        if (!Initialised)
        {
            Init();
        }
        if (playerScores.ContainsKey(username) == false)
        {
            Score tablerow = new Score();
            playerScores.Add(username, tablerow);
            if (onPlayerAdded != null)
                onPlayerAdded(username);
        }

        playerScores[username].PlayerName = username;
        playerScores[username].Score1 = amount;

        if(onPlayerScoreChange != null)
            onPlayerScoreChange(username);
    }
}
