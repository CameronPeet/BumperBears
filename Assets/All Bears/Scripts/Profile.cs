using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour
{
    public ControllerInput Input;
    public int InputNumber;
    public int PlayerNumber;

    public bool Ready = false;
    public bool AxisReturned = true;
    public bool PlayingAndEnabled = false;

    public GameObject PlayerKartPrefab;
    public GameObject PlayerKart;

    private int m_Lives;
    private int m_RoundsWon;


    //DEPRECATED
    int SkinIndex = 0;
    static Material[] BearSkins;
    static bool s_MaterialsLoaded = false;
    public SkinnedMeshRenderer Avatar;
    public Material Skin;

    void Start()
    {
        //DisableInputFor(0.1f);
    }

    public void InitInputIDs()
    {
        Input.LAnalogXAxis = "Horizontal_" + InputNumber.ToString();
        Input.LAnalogYAxis = "Vertical_" + InputNumber.ToString();
        Input.RAnalogXAxis = "LeftRight_" + InputNumber.ToString();
        Input.RAnalogYAxis = "UpDown_" + InputNumber.ToString();
        Input.AButton = "AButton_" + InputNumber.ToString();
        Input.BButton = "BButton_" + InputNumber.ToString();
        Input.XButton = "XButton_" + InputNumber.ToString();
        Input.YButton = "YButton_" + InputNumber.ToString();
        Input.DPadXAxis = "DPadX_" + InputNumber.ToString();
        Input.DPadYAxis = "DPadY_" + InputNumber.ToString();
        Input.LeftTrigger = "LeftTrigger_" + InputNumber.ToString();
        Input.LeftBumper = "LeftBumper_" + InputNumber.ToString();
        Input.RightTrigger = "RightTrigger_" + InputNumber.ToString();
        Input.RightBumper = "RightBumper_" + InputNumber.ToString();
        Input.StartButton = "Start_" + InputNumber.ToString();
        Input.SelectButton = "Select_" + InputNumber.ToString();
    }

    public void CreateKart()
    {
        if(PlayerKart)
        {
            Destroy(PlayerKart);
        }
        PlayerKart = Instantiate(PlayerKartPrefab);
    }

    public void ChangeSkin(int increment)
    {
        if (!s_MaterialsLoaded)
        {
            BearSkins = Resources.LoadAll<Material>("BearSkins");
            s_MaterialsLoaded = true;
        }

        SkinIndex += increment;
        if (SkinIndex < 0)
            SkinIndex = BearSkins.Length - 1;
        else if (SkinIndex >= BearSkins.Length)
            SkinIndex = 0;

        Skin = BearSkins[SkinIndex];
        Avatar.material = Skin;
    }

    public void EnablePlay()
    {
        transform.gameObject.SetActive(true);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<HoverCarControl>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<PlayerAnimator>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
        GetComponentInChildren<Camera>().enabled = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponentInChildren<Camera>().depth = 1;


        gameObject.layer = LayerMask.NameToLayer("Characters");
        foreach (Transform child in transform)
        {
            transform.gameObject.layer = LayerMask.NameToLayer("Characters");
        }
        PlayingAndEnabled = true;
    }

    public void DisablePlay()
    {
        PlayingAndEnabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void DisableFor(float time)
    {
        StartCoroutine("Disable", time);
    }

    public void DisableInputFor(float time)
    {
        StartCoroutine(DisableInput(time));
    }


    private IEnumerator Disable(float alarm)
    {
        DisablePlay();

        float timer = 0.0f;
        while(timer < alarm)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        EnablePlay();
    }

    private IEnumerator DisableInput(float time)
    {
        AxisReturned = false;
        float timer = 0.0f;

        while(timer < time)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        AxisReturned = true;
        print(AxisReturned);
    }
    public bool OnDeath()
    {
        m_Lives--;
        if(m_Lives <= 0)
        {
            return true;
        }
        return false;
    }

    public void SetLives(int lives)
    {
        m_Lives = lives;
    }

    public int GetLives()
    {
        return m_Lives;
    }

    public void AddRoundWon()
    {
        m_RoundsWon = 1;
    }

    public int GetRoundsWon()
    {
        return m_RoundsWon;
    }

}
