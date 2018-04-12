using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using DG.Tweening;

public class PauseMenu : MonoBehaviour {

    public Canvas Menu;

    public bool InLobby = false;

    public GameObject defualtSelectedMain;

    void Update()
    {
        if(!InLobby)
        {
            if (Input.GetButtonDown("Start"))
            {
                Pause();
            }
        }
    }
    // Update is called once per frame
    public void Pause()
    {

        if (Menu.gameObject.activeInHierarchy == false)
        {
            Menu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Menu.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

       FindObjectOfType<EventSystem>().SetSelectedGameObject(defualtSelectedMain);
        
    }

    public void QuitApp()
    {
        Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
