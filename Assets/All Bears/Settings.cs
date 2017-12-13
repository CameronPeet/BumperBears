using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    private static bool Initialised = false;

    public static bool bDualScreensEnabled;
    public GameObject DualScreensPopup;

    public static int iDetectedScreens;

	// Use this for initialization
	void Start () {

        if (Initialised)
            return;

        DontDestroyOnLoad(this.gameObject);
        iDetectedScreens = Display.displays.Length;

        if(iDetectedScreens > 1)
        {
            Instantiate(DualScreensPopup);
        }

        Initialised = true;
	}
    public static void EnableDualScreenMode()
    {
        bDualScreensEnabled = true;
    }

    public static void DisableDualScreenMode()
    {
        bDualScreensEnabled = false;
    }

    public static void ToggleDualScreenMode()
    {
        bDualScreensEnabled = !bDualScreensEnabled;
    }
	// Update is called once per frame
	void Update () {
		
	}

}
