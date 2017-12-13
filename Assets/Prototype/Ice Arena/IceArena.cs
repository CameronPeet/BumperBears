using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArena : Arena {

    bool RoundOver = false;
    bool DoOnce = true;

    //-- Resets
    Vector3 Position;
    Vector3 Rotation;
    public override void OnArenaStart()
    {
        //LevelManager = GameObject.FindObjectOfType<LevelManager>();
        //PlayerSpawner = GameObject.FindObjectOfType<PlayerSpawner>();

        //Players = LevelManager.Players;

        //m_Lives = LevelManager.LivesPerRound;
        //m_Rounds = LevelManager.RoundsPerLevel;

        //foreach (Profile player in Players)
        //{
        //    player.SetLives(m_Lives);

        //    Rigidbody body = player.GetComponent<Rigidbody>();
        //    body.useGravity = true;
        //    body.drag = 1.0f;
        //}

        //Position = transform.position;
        //Rotation = transform.rotation.eulerAngles;

        //m_PlayersLeft = Players.Length;

        //BeginRound();
    }

    public override void OnArenaExit()
    {
        foreach (Rigidbody body in GameObject.FindObjectsOfType<Rigidbody>())
        {
            if (body.gameObject.layer == LayerMask.NameToLayer("Characters"))
            {
                body.drag = 2.5f;
            }
        }

        LevelManager.LoadNextLevel();
    }
}