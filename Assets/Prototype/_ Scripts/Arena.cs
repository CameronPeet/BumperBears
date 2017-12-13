using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour {
   
    //Pre-requisites
    protected LevelManager LevelManager;
    protected PlayerSpawner PlayerSpawner;

    //Players in arena
    protected Profile[] Players;

    //Round info
    protected int m_Rounds = 1;
    protected int m_Lives = 1;
    protected int m_PlayersLeft;
    protected bool m_RoundOver = false;
    protected bool m_RoundInProgress = false;

    //-- Winner animation
    Profile WinningPlayer;
    Camera WinningCamera;


    void Start()
    {
        OnArenaStart();
    }

    void Update()
    {
        if(!m_RoundInProgress)
        {
            //Wait for input, stop winner animation, start next round or start next level.
            if(Input.GetButtonDown("Submit"))
            {
                EndRound();
            }
        }

        if(m_RoundOver && !m_RoundInProgress)
        {
            if(LevelManager.ScreenFader.FadingIn)
            {
                BeginRound();
            }
        }
    }

    public virtual void OnArenaStart()
    {
        print("Inside Arena Script");

        LevelManager = GameObject.FindObjectOfType<LevelManager>();
        PlayerSpawner = GameObject.FindObjectOfType<PlayerSpawner>();

        //Players = LevelManager.Players;

        m_Lives = LevelManager.LivesPerRound;
        m_Rounds = LevelManager.RoundsPerLevel;

        m_PlayersLeft = Players.Length;
        print("aPlayers Left = " + m_PlayersLeft);

        BeginRound();
    }
    public virtual void OnArenaExit()
    {
        LevelManager.LoadNextLevel();
    }

    public virtual void OnPlayerDeath(int PlayerID)
    {
        foreach(Profile player in Players)
        {
            if(player.PlayerNumber == PlayerID)
            {
                if(player.OnDeath())
                {
                    m_PlayersLeft--;
                    print("bPlayers Left = " + m_PlayersLeft);
                    player.DisablePlay();
                }
                else
                {
                    player.GetComponent<Transform>().position = PlayerSpawner.GetFreeSpawnPos().position;
                    Vector3 LookAt = new Vector3(0, player.transform.position.y, 0);
                    player.transform.LookAt(LookAt);

                    player.DisableFor(1.5f);
                }
            }
        }

        if (m_PlayersLeft == 1)
        {
            print("cPlayers Left = " + m_PlayersLeft);
            m_RoundInProgress = false;
            foreach(Profile player in Players)
            {
                if(player.GetLives() > 0)
                {
                    WinningPlayer = player;
                    WinningPlayer.DisablePlay();
                    WinningPlayer.GetComponentInChildren<Camera>().depth = 100;
                    player.GetComponent<Transform>().position = new Vector3(0, 0.5f, 0);
                    player.GetComponent<PlayerAnimator>().StartCameraToFullScreen();
                    player.GetComponent<PlayerAnimator>().StartRotateCameraAroundPlayer();
                    return;
                }

            }
        }
    }

    public virtual void EndRound()
    {
        m_Rounds--;
        m_RoundOver = true;

        WinningPlayer.GetComponent<PlayerAnimator>().StartFullScreenToCamera();
        LevelManager.FadeScreen();

        if(m_Rounds == 0)
        {
            OnArenaExit();
        }
    }

    public virtual void BeginRound()
    {
        if(WinningPlayer)
            WinningPlayer.GetComponent<PlayerAnimator>().StopRotateCameraAroundPlayer();

        foreach(Profile player in Players)
        {

            player.GetComponent<Transform>().position = PlayerSpawner.GetFreeSpawnPos().position;
            player.SetLives(m_Lives);
            player.DisableFor(2.0f);


            Vector3 LookAt = new Vector3(0, player.transform.position.y, 0);
            player.transform.LookAt(LookAt);

        }
        m_PlayersLeft = Players.Length;
        m_RoundInProgress = true;
        print("dPlayers Left = " + m_PlayersLeft);

        m_RoundOver = false;
    }
}
