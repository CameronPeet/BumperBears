using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct SpawnPos
{
    public Transform SpawnPosition;
    public bool UsedRecently;
}
public class PlayerSpawner : MonoBehaviour {

    public Transform[] SpawnPositions;
    private SpawnPos[] RoundRobin;

    void Start()
    {
        RoundRobin = new SpawnPos[SpawnPositions.Length];
        for(int i = 0; i < SpawnPositions.Length; i++)
        {
            RoundRobin[i].SpawnPosition = SpawnPositions[i];
            RoundRobin[i].UsedRecently = false;
        }

        //var LevelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    public Transform GetFreeSpawnPos()
    {
        int index = Random.Range(0, RoundRobin.Length);
        if(RoundRobin[index].UsedRecently)
        {
            for (int i = 0; i < RoundRobin.Length; i++)
            {
                if(!RoundRobin[i].UsedRecently)
                {
                    index = i;
                    break;
                }
            }
        }

        RoundRobin[index].UsedRecently = true;
        StartCoroutine("RoundRobinReset", index);
        return RoundRobin[index].SpawnPosition;
    }


    IEnumerator RoundRobinReset(int IndexToReset)
    {
        yield return new WaitForSeconds(5.0f);

        RoundRobin[IndexToReset].UsedRecently = false;
    }
}
