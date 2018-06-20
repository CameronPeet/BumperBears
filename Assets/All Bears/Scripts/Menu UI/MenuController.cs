using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
	[Header ("Sub menus")]
	[SerializeField]
	private RectMenuItem mainMenu;
	[SerializeField]
	private RectMenuItem optionsMenu;
	[SerializeField]
	private RectMenuItem creditsMenu;
	[SerializeField]
	private RectMenuItem currentMenu;
	private bool canChangeMenu = true;

	[Header ("Main Menu")]
	[SerializeField]
	private Button[] mainMenuButtons;
	[SerializeField]
	private Button mainMenuChoice;
	private int mainMenuIndex = 0;

	private float menuScrollSpeed = 0.15f;
	private float menuScrollTimer = 0.0f;
	private bool canScroll = true;

    EventSystem eventSystem;
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start ()
	{
		SelectCurrentMainMenuOption (0);
		LoadMenuItem(mainMenu, false, true);


    }

	// Update is called once per frame
	void Update ()
	{
		if (canScroll)
		{
            if (Input.GetButtonDown("Submit"))
            {
                ClickButtonFromMainMenu();
            }
            // Controller
            int menuInput = (int)Input.GetAxisRaw("Vertical");

            if (menuInput != 0)
            {
                canScroll = false;
            }
            // Go up?
            if (menuInput == 1)
            {
                mainMenuIndex--;
                if (mainMenuIndex < 0)
                {
                    mainMenuIndex = mainMenuButtons.Length - 1;
                }
                SelectCurrentMainMenuOption(mainMenuIndex);
            }
            // Go down
            else if (menuInput == -1)
            {
                mainMenuIndex++;
                if (mainMenuIndex > mainMenuButtons.Length - 1)
                {
                    mainMenuIndex = 0;
                }
                SelectCurrentMainMenuOption(mainMenuIndex);
            }

            // Keyboard
            if (Input.anyKey)
			{
				canScroll = false;

				if (Input.GetKey (KeyCode.Return))
				{
					ClickButtonFromMainMenu ();
					return;
				}

				if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow))
				{
					mainMenuIndex--;
					if (mainMenuIndex < 0)
					{
						mainMenuIndex = mainMenuButtons.Length - 1;
					}
				}

				if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow))
				{
					mainMenuIndex++;
					if (mainMenuIndex > mainMenuButtons.Length - 1)
					{
						mainMenuIndex = 0;
					}
				}

				SelectCurrentMainMenuOption (mainMenuIndex);
			}
		}
		else
		{
			menuScrollTimer += Time.deltaTime;

			if (menuScrollTimer >= menuScrollSpeed)
			{
				menuScrollTimer = 0.0f;
				canScroll = true;
			}
		}
	}

	void SelectCurrentMainMenuOption (int _input)
	{
		mainMenuIndex = _input;

		mainMenuChoice = mainMenuButtons[mainMenuIndex];

		foreach (Button b in mainMenuButtons)
		{
			ColorBlock cb = b.colors;
			if (b == mainMenuChoice)
			{
				cb.colorMultiplier = 5;
				cb.normalColor = Color.white;
			}
			else
			{
				cb.colorMultiplier = 1;
				cb.normalColor = new Color (0.8f, 0.8f, 0.8f);
			}
			b.colors = cb;
		}
	}

	void ClickButtonFromMainMenu ()
	{
		if (currentMenu == mainMenu)
		{
			mainMenuChoice.onClick.Invoke ();

			switch (mainMenuIndex)
			{
				// Play
				case 0:
					{ }
					break;
					// Options
				case 1:
					{
						LoadMenuItem (optionsMenu);
					}
					break;
					// Credits
				case 2:
					{
						LoadMenuItem (creditsMenu);
					}
					break;
					// Exit
				case 3:
					{ }
					break;
			}
		}
		else
		{
			LoadMenuItem(mainMenu, true);
		}

	}

	void LoadMenuItem (RectMenuItem _item, bool animate = false, bool firstTime = false)
	{
		if (!canChangeMenu)
		{
			return;
		}

		if (_item == mainMenu)
		{
			mainMenu.Show();

			if (animate)
			{
				optionsMenu.Hide(false, false);
				creditsMenu.Hide(true, false);
			}
			else
			{
				optionsMenu.Hide(false, true);
				creditsMenu.Hide(true, true);
			}
		}
		else if (_item == optionsMenu)
		{
			optionsMenu.Show();
			mainMenu.Hide(true, false);
		}
		else if (_item == creditsMenu)
		{
			creditsMenu.Show();
			mainMenu.Hide(false, false);
		}

		currentMenu = _item;
		canChangeMenu = false;

		if (!firstTime)
		{
			StartCoroutine(ResetCanChangeMenu());
		}
		else
		{
			canChangeMenu = true;
		}
	}

	IEnumerator ResetCanChangeMenu()
	{
		yield return new WaitForSeconds(1);
		canChangeMenu = true;
	}
}