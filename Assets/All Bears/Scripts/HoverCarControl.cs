using UnityEngine;
using System.Collections;

[System.Serializable]
public struct PlayerData
{

    public string verticalInput;
    public string horizontalInput;
    public string fireInput;
}



[RequireComponent(typeof(Rigidbody))]
public class HoverCarControl : MonoBehaviour
{

    private int MaxDashes = 4;
    private int NumDashes = 4;
    private float RestoreTimer = 3.0f;
    private float restoretime = 0.0f;
    //public PlayerData playerData;
    public Camera BearCamera;
    private Animator Animator;
    public HoverFollowCam prefab;
    public HoverFollowCam HoverFollowCamera;
    public Rect cameraRect;
    public GameObject CameraLookAt;

    Rigidbody m_body;
    float m_deadZone = 0.1f;

    public float m_constanceForce = 2500.0f;
    public float m_hoverForce = 9.0f;
    public float uprightTorque = 5000.0f;
    public float m_downForceMultiplier = 1.0f;
    public float m_hoverHeight = 2.0f;
    public GameObject[] m_hoverPoints;

    public float m_impulseForce = 1000.0f;
    public float m_forwardAcl = 100.0f;
    public float m_backwardAcl = 25.0f;
    float m_currThrust = 0.0f;

    public float m_turnStrength = 10f;
    float m_currTurn = 0.0f;

    public bool disableInput = false;

    public LayerMask HoverPointRayMask;
    private Player m_player;

    private bool falling = false;
    public bool DisableHovering = false;

    // public GameObject CameraPrefab;
    // public GameObject camera;

    public ParticleSystem Dash;

    void Start()
    {
        m_body = GetComponent<Rigidbody>();
        m_player = GetComponent<Player>();
        HoverFollowCamera = Instantiate(prefab);
        HoverFollowCamera.SetLookAt(CameraLookAt);
        BearCamera = HoverFollowCamera.GetComponent<Camera>();
        BearCamera.name = this.gameObject.name + " Camera";
        BearCamera.rect = cameraRect;
        Animator = GetComponent<Animator>();
        //camera = GameObject.Instantiate<GameObject>(CameraPrefab);
    }


    public void CreateCamera()
    {
        HoverFollowCamera = Instantiate(prefab);
        HoverFollowCamera.SetLookAt(CameraLookAt);
        BearCamera = HoverFollowCamera.GetComponent<Camera>();
        BearCamera.name = this.gameObject.name + " Camera";
        BearCamera.rect = cameraRect;
    }

    void Awake()
    {
        //camera.GetComponent<HoverFollowCam>().m_player = this.gameObject;
    }

    void OnDrawGizmos()
    {
        //  Hover Force
        RaycastHit hit;
        for (int i = 0; i < m_hoverPoints.Length; i++)
        {
            var hoverPoint = m_hoverPoints [i];
            if (Physics.Raycast(hoverPoint.transform.position, 
                                -Vector3.up, out hit,
                                m_hoverHeight,
                                HoverPointRayMask))
            {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
            Gizmos.DrawSphere(hit.point, 0.2f);
            } else
            {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(hoverPoint.transform.position, 
                            hoverPoint.transform.position - Vector3.up * m_hoverHeight);
            }
        }
    }

    void TickCharge()
    {
        if(NumDashes < MaxDashes)
        {
            restoretime += Time.deltaTime;
            if(restoretime >= RestoreTimer)
            {
                NumDashes++;
                if(NumDashes < MaxDashes)
                {
                    // minus the Restore Timer so the overflow continues into next frame. Minor accuracy improvement
                    restoretime -= RestoreTimer;
                }
                else
                {
                    // Set to zero. We don't want the overflow to continue.
                    restoretime = 0.0f;
                }
            }
        }
    }

    void Update()
    {
        if (!m_player.PlayingAndEnabled)
            return;


        float thrust = 0.0f;
        if(Input.GetButton(m_player.Input.AButton))
        {
            thrust = 1.0f;
        }
        if(Input.GetButton(m_player.Input.BButton))
        {
            thrust = -1.0f;
        }
        float turn = Input.GetAxis(m_player.Input.LAnalogXAxis);

        if (Input.GetButtonDown(m_player.Input.LeftBumper) 
        || Input.GetButtonDown(m_player.Input.RightBumper
        ))
        {
                Charge();
        }


        if (Input.GetButtonDown(m_player.Input.XButton))
        {

        }

        Accelerate(thrust);
        Turn(turn);

        if(m_body.velocity.y < -5.0f && !falling)
        {
            falling = true;
            GetComponent<Animator>().Play("Falling");
        }
        else if(falling && m_body.velocity.y > -5.0f)
        {
            falling = false;
            GetComponent<Animator>().Play("Idle");
        }

        TickCharge();
    }
    public void Accelerate(float axis)
    {
        m_currThrust = m_constanceForce;
        if (axis > m_deadZone)
            m_currThrust = axis * m_forwardAcl;
        else if (axis < -m_deadZone)
            m_currThrust = axis * m_backwardAcl;
    }

    public void Turn(float axis)
    {
        m_currTurn = 0.0f;
        if (Mathf.Abs(axis) > m_deadZone)
            m_currTurn = axis;
    }

    public void Charge()
    {
        //Return if no charges are available. Naming convention is off using Dashes = Charges I know..
        if (NumDashes <= 0)
            return;


        GetComponent<Rigidbody>().AddForce(transform.forward * m_impulseForce, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * m_impulseForce / 10.0f, transform.position + transform.forward, ForceMode.Impulse);
        Instantiate(Dash, gameObject.transform);
        //GetComponentInChildren<ParticleSystem>().Play();
        Animator.Play("Fast");

        NumDashes--;
    }

    void FixedUpdate()
    {
        int Airborne = 0;
        if (!DisableHovering)
        {
            RaycastHit hit;
      
            for (int i = 0; i < m_hoverPoints.Length; i++)
            {
                var hoverPoint = m_hoverPoints[i];
                if (Physics.Raycast(hoverPoint.transform.position,
                                    -hoverPoint.transform.up, out hit,
                                    m_hoverHeight,
                                    HoverPointRayMask))
                {
                    if(hit.distance <= m_hoverHeight)
                    {
                        m_body.AddForceAtPosition(hoverPoint.transform.up
                        * m_hoverForce
                        * (1.0f - (hit.distance / m_hoverHeight)),
                                                hoverPoint.transform.position);
                    }

                    else
                    {
                        //m_body.AddForceAtPosition(                                             // AddForce to Bear Body
                        //    hoverPoint.transform.up * (-m_hoverForce * 0.5f), // Force amount
                        //    hoverPoint.transform.position);
                        //Airborne++;
                    }
                }
                    
                else
                {
                    Airborne++;
                    //m_body.AddForceAtPosition(                                             // AddForce to Bear Body
                    //    hoverPoint.transform.up * (-m_hoverForce * m_downForceMultiplier), // Force amount
                    //    hoverPoint.transform.position);
                }
            }
        }

        var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        m_body.AddTorque(new Vector3(rot.x, rot.y, rot.z) * uprightTorque);

        if (Airborne >= 4)
        {
            m_currThrust /= 5.0f;
            m_body.AddForce(-Vector3.up * m_hoverForce * m_downForceMultiplier);
        }
            //return;

        // Forward
        if (Mathf.Abs(m_currThrust) > 0)
        {
            m_body.AddForce(transform.forward * m_currThrust * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        // Turn
        if (m_currTurn > 0)
        {
            //m_body.AddTorque(Vector3.up * m_currTurn * m_turnStrength);
            transform.Rotate(Vector3.up, m_currTurn * m_turnStrength * Time.deltaTime);
        }
        else if (m_currTurn < 0)
        {
            //m_body.AddTorque(Vector3.up * m_currTurn * m_turnStrength);
            transform.Rotate(Vector3.up, m_currTurn * m_turnStrength * Time.deltaTime);
        }

    }

}
