using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Lobby : MonoBehaviour {

    public int SplitScreenSize = 4;
    public bool AttemptDualMonitor = false;
    public List<Player> BearsAvailable;
    public AutoDisable[] PlayerJoinLogo;
    public GameObject[] ReadyLogo;
    public CountdownTimer CountdownTimer;

    public List<Player> Players;
    private List<int> InputsLeft;
    private int ReadyCount = 0;


    bool IntroFinished = false;


    //Audio
    public AudioSource AudioSource;
    public AudioClip ReadyStamp;
    public AudioClip PressStartToJoin;

    public void Awake()
    {

    }




    // Use this for initialization
    void Start () {

        Players = new List<Player>(SplitScreenSize);
        InputsLeft = new List<int>(4);
        //Add Input Numbers 1 to 4 for buttons udner Input "Start_N" where N = Input number 1 - 4
        InputsLeft.Add(1);
        InputsLeft.Add(2);
        InputsLeft.Add(3);
        InputsLeft.Add(4);

        foreach(AutoDisable intro in PlayerJoinLogo)
        {
            intro.gameObject.SetActive(true);
            intro.transform.localScale = Vector3.zero;
            intro.transform.DOScale(Vector3.one, 1.0f);
        }
        AudioSource.PlayOneShot(PressStartToJoin, 0.5f);
    }

    // Update is called once per frame
    void Update ()
    {
        HandlePlayersJoining();
        HandlePlayersJoined();
	}

    void HandlePlayersJoining()
    {
        foreach (int i in InputsLeft)
        {
            if (Input.GetButtonDown("Start_" + i))
            {
                CreatePlayer(i);
                return;
            }
        }

        for(int i = 1; i <= InputsLeft.Count; i++)
        {
            if (!InputsLeft.Contains(i))
            {
                if(Input.GetButtonDown("Start_" + i))
                {
                    PauseMenu p = FindObjectOfType<PauseMenu>();
                    if(p != null)
                    {
                        p.Pause();
                    }
                }
            }
        }
    }

    void HandlePlayersJoined()
    {
        foreach (Player player in Players)
        {
            if(!player.AxisReturned)
            {
                return;
            }
            
            if(player.Ready == false)
            {
                float horizontal = Input.GetAxis(player.Input.LAnalogXAxis);
                if (player.AxisReturned && (horizontal >= 0.9f || horizontal <= -0.9f))
                {
                    SwitchBear(player);
                 
                    return;
                }

                if (Input.GetButtonDown(player.Input.AButton))
                {
                    ReadyUpPlayer(player);
                }

                if (Input.GetButtonDown(player.Input.BButton))
                {
                    InputsLeft.Add(player.InputNumber);
                    RemovePlayer(player);
                    return;
                }
            }

            else if(player.Ready)
            {
                if (Input.GetButtonDown(player.Input.BButton))
                {

                    UnReadyPlayer(player);
                    return;
                }
            }
        }
    }

    void UnReadyPlayer(Player player)
    {
        GameObject readyImage = ReadyLogo[player.PlayerNumber - 1];
        readyImage.SetActive(false);
        player.Ready = false;

        if (ReadyCount == Players.Count && ReadyCount > 1)
        {
            CountdownTimer.gameObject.SetActive(false);
        }

        ReadyCount--;
    }
    void ReadyUpPlayer(Player player)
    {
        PlayerJoinLogo[player.PlayerNumber - 1].gameObject.SetActive(false);
        GameObject readyImage = ReadyLogo[player.PlayerNumber - 1];
        readyImage.SetActive(true);
        readyImage.transform.DOPunchScale(Vector3.one, 0.2f);
        player.Ready = true;
        ReadyCount++;

        if (ReadyCount == Players.Count && ReadyCount > 1)
        {
            CountdownTimer.gameObject.SetActive(true);
            CountdownTimer.Restart();
        }

        AudioSource.PlayOneShot(ReadyStamp, 0.5f);
    }

    void RemovePlayer(Player bear)
    {
        ReadyLogo[bear.PlayerNumber - 1].gameObject.SetActive(false);
        PlayerJoinLogo[bear.PlayerNumber - 1].gameObject.SetActive(true);
        PlayerJoinLogo[bear.PlayerNumber - 1].transform.localScale = Vector3.zero;
        PlayerJoinLogo[bear.PlayerNumber - 1].transform.DOScale(Vector3.one, 1.0f);

        ResetPlayerLayers(bear);
        BearsAvailable.Add(bear);
        Players.Remove(bear);

        bear.InputNumber = 0;
        bear.PlayerNumber = 0;
        ResetPlayerLayers(bear);
    }

    void SwitchBear(Player bear)
    {
        if (BearsAvailable.Count == 0)
            return;

        Player newbear = BearsAvailable[0];

        int p = bear.PlayerNumber;
        int id = bear.InputNumber;

        BearsAvailable.Remove(newbear);
        BearsAvailable.Add(bear);

        ResetPlayerLayers(bear);
        bear.PlayerNumber = 0;
        bear.InputNumber = 0;

        newbear.InitialiseInput(id);
        newbear.PlayerNumber = p;

        SetPlayerLayers(newbear);

        newbear.DisableInputFor(0.5f);
        newbear.Animator.Play("Idle");
        Players.Remove(bear);
        Players.Add(newbear);

        newbear.gameObject.transform.DOKill(true);
        newbear.gameObject.transform.DOPunchScale(Vector3.one * 1.25f, 0.5f, 2);
    }

    void ResetPlayerLayers(Player bear)
    {
        string layer = "Default";
        bear.gameObject.layer = LayerMask.NameToLayer(layer);
        foreach (Transform child in bear.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layer);
        }
    }
    void SetPlayerLayers(Player bear)
    {
        string layer = "Player " + bear.PlayerNumber.ToString();
        bear.gameObject.layer = LayerMask.NameToLayer(layer);
        foreach (Transform child in bear.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layer);
        }
    }

    void CreatePlayer(int inputNumber)
    {
        if(BearsAvailable.Count == 0)
            return;

        int FirstFree = 1;
        for(int i = 1; i <= 4; i++)
        {
            FirstFree = i;
            bool available = true;
            for(int j = 0; j < Players.Count; j++)
            {
                if(i  == Players[j].PlayerNumber)
                {
                    available = false;
                }
            }
            if (available)
            {
                i = 50; //Exit
            }
        }

        Player bear = BearsAvailable[0];
        Players.Add(bear);
        BearsAvailable.Remove(bear);

        int cross_index = FirstFree - 1;
        int player_number = FirstFree;

        // +1 because it hasn't been added yet
        bear.InitialiseInput(inputNumber);
        bear.PlayerNumber = player_number;


        // Use DOTween to punchscale the join logo, then use the AutoDisable function to disable it in 0.3 seconds
        //NOT SURE IF I WANT THIS.
        //PlayerJoinLogo[cross_index].transform.DOPunchScale(Vector3.one, 0.3f, 1, 1);
        //PlayerJoinLogo[cross_index].Disable(0.4f);

        PlayerJoinLogo[cross_index].gameObject.SetActive(false);

        SetPlayerLayers(bear);
        bear.DisableInputFor(1.0f);
        InputsLeft.Remove(inputNumber);
        bear.Animator.Play("Idle");
    }

    public void StartFromLobby()
    {
        var LevelManager = GameObject.FindObjectOfType<LevelManager>();
        LevelManager.PassPlayersFromLobby(Players);
        LevelManager.StartGame();
    }
    private IEnumerator Intro()
    {
        float timer = 0.0f;
        int count = 0;
        int maxCount = 4;

        while(timer < 1.0f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0.0f;
        while(count < maxCount)
        {
            timer += Time.deltaTime;
            if(timer >= 0.5f)
            {

                timer = 0.0f;
                count++;
            }

            yield return null;
        }

        IntroFinished = true;
    }
}
