using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Settings : MonoBehaviour {

    private static bool Initialised = false;

    public static bool bDualScreensEnabled;
    public GameObject DualScreensPopup;

    public static int iDetectedScreens;

	// Use this for initialization
	void Start () {

        if (Initialised)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        iDetectedScreens = Display.displays.Length;

        if(iDetectedScreens > 1)
        {
            GameObject popup = Instantiate(DualScreensPopup);
            EventSystem es = FindObjectOfType<EventSystem>();
            if(es)
            {
                es.SetSelectedGameObject(popup.GetComponentInChildren<Button>().gameObject);
            }

        }

        Initialised = true;
	}
    public static void EnableDualScreenMode()
    {
        print("Enabled");
        bDualScreensEnabled = true;
    }

    public static void DisableDualScreenMode()
    {
        print("Disabled");

        bDualScreensEnabled = false;
    }

    public static void ToggleDualScreenMode()
    {
        bDualScreensEnabled = !bDualScreensEnabled;
    }

    public static void CheckboxDualScreenMode(Toggle toggle)
    {
        toggle.isOn = bDualScreensEnabled;
    }

    public static void CheckboxFullScreen(Toggle toggle)
    {
        
    }
	// Update is called once per frame
	void Update () {
		
	}

}
