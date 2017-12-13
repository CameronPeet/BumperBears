using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {

    private Arena Arena;
	// Use this for initialization
	void Start () {
        Arena = GameObject.FindObjectOfType<Arena>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Characters"))
        {
            Profile player = collider.gameObject.GetComponent<Profile>();
            Arena.OnPlayerDeath(player.PlayerNumber);
        }
        else if(collider.gameObject.layer != LayerMask.NameToLayer("Terrain"))
        {
            DestroyObject(collider.gameObject);
        }
    }
}
