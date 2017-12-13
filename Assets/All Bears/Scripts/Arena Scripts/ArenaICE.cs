using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ArenaICE :  IArena
{

    //Pre-requisites
    protected LevelManager LevelManager;
    protected PlayerSpawner PlayerSpawner;


    protected Player[] Players;

    protected int m_PlayersLeft;
    protected int m_Rounds = 0;
    protected bool m_RoundOver = false;
    protected bool m_RoundInProgress = false;

    public ParticleSystem SpawningParticle;

    //-- Winner animation
    Player WinningPlayer;
    Camera WinningCamera;

    // Use this for initialization
    void Start()
    {
        OnArenaStart();
        StartCoroutine("LevelStart");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {

        }
        if (!m_RoundInProgress)
        {
            //Wait for input, stop winner animation, start next round or start next level.
            if (Input.GetButtonDown("Submit"))
            {
                EndRound();
            }
        }

        if (m_RoundOver && !m_RoundInProgress)
        {
            if (LevelManager.ScreenFader.FadingIn)
            {
                BeginRound();
            }
        }

        foreach (Player player in Players)
        {
            if (player.transform.position.y < -5.0f)
            {
                OnPlayerDeath(player.PlayerNumber);
            }
        }
    }

    public virtual void OnArenaStart()
    {
        LevelManager = GameObject.FindObjectOfType<LevelManager>();
        PlayerSpawner = GameObject.FindObjectOfType<PlayerSpawner>();

        Players = GameObject.FindObjectsOfType<Player>();

        //foreach (Player player in Players)
        //{
        //    player.transform.position = PlayerSpawner.GetFreeSpawnPos().position;
        //    player.transform.LookAt(PlayerSpawner.transform);
        //}

        //Players = LevelManager.Players;

        m_PlayersLeft = Players.Length;
    }

    public virtual void OnArenaExit()
    {
        //LevelManager.LoadNextLevel();
    }

    public virtual void OnPlayerDeath(int PlayerID)
    {
        foreach (Player player in Players)
        {
            if (player.PlayingAndEnabled)
                if (player.PlayerNumber == PlayerID)
                {
                    player.Lives--;
                    if (player.Lives <= 0)
                    {
                        m_PlayersLeft--;
                        //print("bPlayers Left = " + m_PlayersLeft);
                        player.PlayingAndEnabled = false;
                        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    }
                    else
                    {
                        player.GetComponent<Transform>().position = PlayerSpawner.GetFreeSpawnPos().position;
                        Vector3 LookAt = new Vector3(0, player.transform.position.y, 0);
                        player.transform.LookAt(LookAt);
                    }
                }
        }

        if (m_PlayersLeft == 1)
        {
            m_RoundInProgress = false;
            foreach (Player player in Players)
            {
                if (player.PlayingAndEnabled)
                {
                    WinningPlayer = player;
                    //WinningPlayer.DisablePlay();
                    player.GetComponent<HoverCarControl>().BearCamera.depth = 100;
                    player.GetComponent<Transform>().position = new Vector3(0, 0.5f, 0);
                    WinningPlayer.GetComponent<HoverCarControl>().BearCamera.DORect(new Rect(0, 0, 1, 1), 1.0f);
                    WinningPlayer.PlayingAndEnabled = false;
                    WinningPlayer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    //player.GetComponent<PlayerAnimator>().StartRotateCameraAroundPlayer();
                    return;
                }

            }
        }
    }

    public virtual void EndRound()
    {
        m_Rounds--;
        m_RoundOver = true;

        WinningPlayer.GetComponent<HoverCarControl>().BearCamera.DORect(WinningPlayer.GetComponent<HoverCarControl>().cameraRect, 1.0f);


        LevelManager.FadeScreen();

        if (m_Rounds == 0)
        {
            OnArenaExit();
        }
    }

    public virtual void BeginRound()
    {
        if (WinningPlayer)
            WinningPlayer.GetComponent<HoverCarControl>().BearCamera.DORect(WinningPlayer.GetComponent<HoverCarControl>().cameraRect, 1.0f);

        foreach (Player player in Players)
        {
            player.transform.position = PlayerSpawner.GetFreeSpawnPos().position;
            player.transform.LookAt(PlayerSpawner.transform);
            player.PlayingAndEnabled = true;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        }

        m_PlayersLeft = Players.Length;
        m_RoundInProgress = true;

        m_RoundOver = false;
    }


    private IEnumerator LevelStart()
    {
        m_PlayersLeft = Players.Length;
        m_RoundInProgress = true;
        m_RoundOver = false;

        float timer = 0.0f;

        foreach (Player player in Players)
        {
            player.PlayingAndEnabled = false;
        }

        while (timer < 0.4f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0.0f;
        int count = 0;
        while (count < Players.Length)
        {
            timer += Time.deltaTime;
            if (timer > 1.3f)
            {
                timer = 0.0f;
                Vector3 Scale = Players[count].transform.localScale;
                Players[count].transform.localScale = Vector3.zero;
                Players[count].transform.DOScale(Scale, 0.5f);
                Players[count].transform.position = PlayerSpawner.GetFreeSpawnPos().position;
                Players[count].transform.LookAt(PlayerSpawner.transform);
                Instantiate(SpawningParticle, Players[count].transform.position + Vector3.up, Players[count].transform.rotation);
                Players[count].gameObject.GetComponent<Animator>().Play("Idle");
                count++;
            }
            yield return null;
        }


        foreach (Player player in Players)
        {
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.GetComponent<Rigidbody>().drag = 2.0f;
            player.GetComponent<Player>().PlayingAndEnabled = true;
        }

        Camera.main.DORect(Rect.zero, 1.0f);
        StartCoroutine("EndIntro");
    }

    private IEnumerator EndIntro()
    {
        float timer = 0.0f;

        while (timer < 0.8f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Camera.main.gameObject.SetActive(false);



    }
}


