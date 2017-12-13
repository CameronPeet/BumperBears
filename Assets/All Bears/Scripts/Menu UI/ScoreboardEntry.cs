using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score
{
    public string PlayerName;
    public int Score1;
    public int Score2;
    public int Score3;
}

public class ScoreboardEntry : MonoBehaviour {


    public Text Player;
    public Text Score_1;
    public Text Score_2;
    public Text Score_3;

    public string PlayerName;
    public int Score1;
    public int Score2;
    public int Score3;

    public int PlayerID;

    public bool updateFlag = false;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(updateFlag)
        {
            UpdateUI();
            updateFlag = false;
        }
	}

    private void UpdateUI()
    {
        Player.text = PlayerName;
        Score_1.text = "" + Score1;
        Score_2.text = "" + Score2;
        Score_3.text = "" + Score3;
    }
}
