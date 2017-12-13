using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LocalLobby : MonoBehaviour {

    public CountdownTimer CountdownTimer;
    public GameObject PlayerPrefab;
    public AutoDisable[] PlayerJoin;

    public GameObject[] ReadyUp_NotReady;
    public GameObject[] ReadyUp_IsReady;

    private bool[] PlayerActive;
    //Instiated Players
    public List<Profile> Players;
    //Instantiate Karts
    public List<GameObject> PlayerKarts;

    //Kart selection
    public GameObject[] KartPreviews;
    //Profile Prefab
    public Profile ProfilePrefab;

    int ReadyCount = 0;
	// Use this for initialization
	void Start () {
        PlayerActive = new bool[4];
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 1; i <= 4; i++)
        {
            //Joining Section
            if (Input.GetButtonDown("Start_" + i))
            {
                foreach (Profile alreadyJoined in Players)
                {
                    if (alreadyJoined.InputNumber == i)
                    {
                        return; //Player already exists
                    }
                }

                //Instantiate a new player
                Profile profile = Instantiate(ProfilePrefab, transform);
                //Set up the profile
                profile.InputNumber = i;
                profile.InitInputIDs();
                Players.Add(profile);
                profile.PlayerNumber = Players.Count;

                StartCoroutine(CreatePlayer(profile, 0.5f));
                PlayerJoin[profile.PlayerNumber - 1].transform.DOPunchScale(Vector3.one, 0.3f, 1, 1);
                PlayerJoin[profile.PlayerNumber - 1].Disable(0.3f);
                PlayerActive[i - 1] = true;
            }

        }
        foreach(Profile profile in Players)
        {
            float horizontal = Input.GetAxis(profile.Input.LAnalogXAxis);
            if ((horizontal >= 0.9f || horizontal <= -0.9f) && profile.AxisReturned)
            {

                //profile.ChangeSkin(Mathf.RoundToInt(horizontal));
                ////Tween Shake
                //{
                //    profile.transform.DOShakeScale(0.2f);
                //}
                //profile.DisableInputFor(0.5f);
            }

            if (Input.GetButtonDown(profile.Input.AButton))
            {
                ReadyUp_NotReady[profile.PlayerNumber - 1].SetActive(false);
                ReadyUp_IsReady[profile.PlayerNumber - 1].SetActive(true);
                profile.Ready = true;
                ReadyCount++;

                if (ReadyCount == Players.Count)
                {
                    CountdownTimer.gameObject.SetActive(true);
                }
            }
        }
	}

   public void StartFromLobby()
    {
        //var LevelManager = GameObject.FindObjectOfType<LevelManager>();
        ////LevelManager.PassPlayersFromLobby(Players);
        //LevelManager.LoadNextLevel();
    }


    private IEnumerator CreatePlayer(Profile profile, float ExecuteIn = 1.0f)
    {
        float timer = 0.0f;
        while(timer < ExecuteIn)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        GameObject preview = GameObject.Instantiate<GameObject>(KartPreviews[0]);
        //Tween scale
        {
            preview.transform.localScale = new Vector3(0, 0, 0);
            preview.transform.DOScale(1.0f, 2.15f);
        }

        PlayerKarts.Add(preview);
        //Get the prfile


        //Change the profile layer so only their camera can see it.
        string layer = "Player " + profile.PlayerNumber.ToString();
        preview.layer = LayerMask.NameToLayer(layer);
        foreach (Transform child in preview.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layer);
        }

        ReadyUp_NotReady[profile.PlayerNumber - 1].SetActive(true);
    }
}
