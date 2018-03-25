using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Controller Input associates the variable name, with its content (string)
//E.g AButton = "AButton_1" - AButton_N is set up under Input, and the N corresponds to the player ID
public struct ControllerInput
{

    public string ControllerName;

    public string LAnalogXAxis;
    public string LAnalogYAxis;

    public string RAnalogXAxis;
    public string RAnalogYAxis;

    public string AButton;
    public string XButton;
    public string BButton;
    public string YButton;

    public string DPadXAxis;
    public string DPadYAxis;

    public string LeftTrigger;
    public string LeftBumper;

    public string RightTrigger;
    public string RightBumper;

    public string StartButton;
    public string SelectButton;
}


public class Player : MonoBehaviour
{
    public bool InUse = false;
    //Controller Input variables
    public ControllerInput Input;
    public Animator Animator;
    public Scoreboard Scoreboard;

    private bool bHasScoreboard;

    public int InputNumber;
    public int PlayerNumber;
    public bool AxisReturned = true;

    public int Lives = 0;

    //Player Lobby Variables
    public bool Ready = false;

    //Set to false to disable movement during scenes
    public bool PlayingAndEnabled = false;


    private void Start()
    {
        Animator = GetComponent<Animator>();
        InitialiseInput(InputNumber);

        if(!bHasScoreboard)
        {
            CreateScoreboardVerticalScreen();
        }
    }

    public void CreateScoreboardVerticalScreen()
    {
        Scoreboard = Instantiate(Scoreboard, this.transform).GetComponent<Scoreboard>();
        Scoreboard.GetComponent<Canvas>().worldCamera = GetComponent<HoverCarControl>().HoverFollowCamera.GetComponentInParent<Camera>();
        bHasScoreboard = true;
    }

    public void Update()
    {
        if (PlayingAndEnabled)
        {
            if (UnityEngine.Input.GetButtonDown(Input.SelectButton))
            {
                if(Scoreboard)
                {
                    Scoreboard.Activate();
                }
            }
            if (UnityEngine.Input.GetButtonUp(Input.SelectButton))
            {
                if(Scoreboard)
                {
                    Scoreboard.DeActivate();
                }
            }

            if(UnityEngine.Input.GetButtonDown(Input.AButton))
            {
                //if(Scoreboard)
                //{
                //    ScoreboardManager.Scoreboard.SetScore(gameObject.name, "asd", 10);
                //}
            }
        }

    }

    public void InitialiseInput(int ID)
    {
        InputNumber = ID;

        //Set the Analog stick axis'
        Input.LAnalogXAxis = "Horizontal_" + InputNumber.ToString();
        Input.LAnalogYAxis = "Vertical_" + InputNumber.ToString();
        Input.RAnalogXAxis = "LeftRight_" + InputNumber.ToString();
        Input.RAnalogYAxis = "UpDown_" + InputNumber.ToString();

        //Set the 4 GamePad buttons
        Input.AButton = "AButton_" + InputNumber.ToString();
        Input.BButton = "BButton_" + InputNumber.ToString();
        Input.XButton = "XButton_" + InputNumber.ToString();
        Input.YButton = "YButton_" + InputNumber.ToString();

        //Set the DPad Axis'
        Input.DPadXAxis = "DPadX_" + InputNumber.ToString();
        Input.DPadYAxis = "DPadY_" + InputNumber.ToString();

        //Set the Bumper / Trigger
        Input.LeftTrigger = "LeftTrigger_" + InputNumber.ToString();
        Input.LeftBumper = "LeftBumper_" + InputNumber.ToString();
        Input.RightTrigger = "RightTrigger_" + InputNumber.ToString();
        Input.RightBumper = "RightBumper_" + InputNumber.ToString();

        //Set the special buttons
        Input.StartButton = "Start_" + InputNumber.ToString();
        Input.SelectButton = "Select_" + InputNumber.ToString();
    }


    public void DisableInputFor(float time)
    {
        StartCoroutine(DisableInput(time));
    }

    private IEnumerator DisableInput(float time)
    {
        AxisReturned = false;
        float timer = 0.0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        AxisReturned = true;
    }
}
