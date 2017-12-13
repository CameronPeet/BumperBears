using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Scoreboard : MonoBehaviour {

    public Canvas canvas;
    public ScoreboardEntry PlayerRowPrefab;
    public GameObject ScoreBoardPanel;

    public bool TestEntry = false;
    public string Username;
    public string Type;
    public int Score;

    //For use in debug scenarios where Level manager tries to spawn players and create scores from the Start()
    //Level manager is executed first in SEOrder, and so need to call subbing manually.
    private bool Subscribed = false;

    Dictionary<string, ScoreboardEntry> playerScores;

    
    // Use this for initialization
    void Start () {

        if (!Subscribed)
        {
            SubscribeToManager();
        }

        if(playerScores == null)
            playerScores = new Dictionary<string, ScoreboardEntry>();

        //Disable panel, activated while player holds correct button / key.
        ScoreBoardPanel.gameObject.SetActive(false);
    }

    public void SubscribeToManager()
    {
        if(Subscribed)
        {
            return;
        }

        //Subscribe to score change delegates
        ScoreboardManager.Scoreboard.onPlayerScoreChange += OnPlayerScoreChange;
        ScoreboardManager.Scoreboard.onPlayerAdded += OnPlayerAdded;

        Subscribed = true;
    }
    public void Activate()
    {
        ScoreBoardPanel.gameObject.SetActive(true);
    }

    public void DeActivate()
    {
        ScoreBoardPanel.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    public void OnPlayerScoreChange(string username)
    {

        if (playerScores.ContainsKey(username) == true)
        {
            /// print(username);

            Score entry = ScoreboardManager.Scoreboard.playerScores[username];

            playerScores[username].Score1 = 100 + playerScores[username].Score1;
            playerScores[username].Score2 = entry.Score2;
            playerScores[username].Score3 = entry.Score3;
            playerScores[username].updateFlag = true;
        }
    }



    public void OnPlayerAdded(string username)
    {
        if(playerScores == null)
        {
            playerScores = new Dictionary<string, ScoreboardEntry>();
        }

        ScoreboardEntry tablerow = Instantiate(PlayerRowPrefab, ScoreBoardPanel.transform);
        playerScores.Add(username, tablerow);

        tablerow.PlayerName = username;
        tablerow.Score1 = 0;
        tablerow.Score2 = 0;
        tablerow.Score3 = 0;
        tablerow.updateFlag= true;
        //Scorebuffer buf = new Scorebuffer();
        //buf.playertoadd = username;
        //buffer.Add(buf);
    }
}
