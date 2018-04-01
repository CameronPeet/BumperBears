using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public Canvas Menu;


    void Update()
    {
        if(Input.GetButtonDown("Start"))
        {
            Pause();
        }
    }
	// Update is called once per frame
	void Pause()
    {
	
        if(Menu.gameObject.activeInHierarchy == false)
        {
            Menu.gameObject.SetActive(true);
            Time.timeScale = 0;

        }
        else
        {
            Menu.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
	}
}
