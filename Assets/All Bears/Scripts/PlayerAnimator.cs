using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    public Animator Animator;
    public Rigidbody Rigidbody;

    //Camera Animation
    public Camera Camera;
    private Rect OriginalRect;
    private bool Rotating = false;


    public float m_CurrentVelocity;
    private float m_LastVelocity;

    public bool SpinOut = false;
    public float SpinOutTime = 1.0f;
    public float SpinOutDegrees = 360.0f;

    public bool Blinking = false;
    public float BlinkingWaitTime = 0.5f;
    public int BlinkingTimes = 2;

    public bool ScreenAnimating = false;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        Camera = GetComponentInChildren<HoverCarControl>().BearCamera;
    }
	
	// Update is called once per frame
	void Update () {
        CheckAnimationParameters();
	}


    void CheckAnimationParameters()
    {
        Vector3 velocity = Rigidbody.velocity;
        float length = velocity.magnitude;

        Animator.SetFloat("Velocity", length);

        m_CurrentVelocity = length;
        //If you lose over half you velocity in a single frame -> Animate the Impact
        if (m_CurrentVelocity < m_LastVelocity / 2.0f && m_CurrentVelocity < m_LastVelocity - 5) 
        {
            Animator.SetBool("Hit", true);
            Animator.Play("Impact");
        }
        m_LastVelocity = m_CurrentVelocity;

    }

    public void StartSpinOut()
    {
        StartCoroutine("SpinOutControl");
    }

    public void StartBlinking()
    {
        StartCoroutine(BlinkingControl(BlinkingWaitTime, BlinkingTimes));
    }

    public void StartCameraToFullScreen()
    {
        OriginalRect = Camera.rect;
        StartCoroutine("CameraToFullScreen");
    }

    public void StartFullScreenToCamera()
    {
        StartCoroutine("FullScreenToCamera");
    }

    public void StartRotateCameraAroundPlayer()
    {
        StartCoroutine("RotateCameraAroundPlayer");
    }

    public void StopRotateCameraAroundPlayer() { Rotating = false; }


    private IEnumerator SpinOutControl()
    {
        GetComponent<HoverCarControl>().disableInput = true;
        SpinOut = true;
        var time = 0.0f;

        while(time < SpinOutTime)
        {
            time += Time.deltaTime;
            var turnIncrement = new Vector3(0, SpinOutDegrees / SpinOutTime * Time.deltaTime, 0);

            transform.eulerAngles += turnIncrement;
            yield return null;
        }

        SpinOut = false;
        GetComponent<HoverCarControl>().disableInput = false;
    }

    private IEnumerator BlinkingControl(float waitTime, int numberOfLoops)
    {
        int timesExecuted = 0;
        Blinking = true;
        Renderer renderer = GetComponentInChildren<Renderer>();
        while(timesExecuted < numberOfLoops)
        {
            timesExecuted++;
            if (renderer.enabled)
                renderer.enabled = false;
            else
                renderer.enabled = true;

            yield return new WaitForSeconds(waitTime);
        }

        renderer.enabled = true;
        Blinking = false;
    }

    private IEnumerator CameraToFullScreen()
    {
        ScreenAnimating = true;
        Vector2 StartPoint = new Vector2(OriginalRect.x, OriginalRect.y);
        Vector2 StartSize = new Vector2(OriginalRect.width, OriginalRect.height);
        Vector2 FinishPoint = new Vector2(0, 0);
        Vector2 FinishSize = new Vector2(1, 1);
        float timer = 0.0f;
        float alarm = 2.0f;
        while(timer < alarm)
        {
            timer += Time.deltaTime;
            float timeScale = timer / alarm;
            Vector2 StartLerp = Vector2.Lerp(StartPoint, FinishPoint, timeScale);
            Vector2 ScreenLerp = Vector2.Lerp(StartSize, FinishSize, timeScale);

            Camera.rect = new Rect(StartLerp, ScreenLerp);

            yield return null;
        }
        ScreenAnimating = false;
    }

    private IEnumerator FullScreenToCamera()
    {
        ScreenAnimating = true;

        Vector2 StartPoint = new Vector2(OriginalRect.x, OriginalRect.y);
        Vector2 StartSize = new Vector2(OriginalRect.width, OriginalRect.height);
        Vector2 FinishPoint = new Vector2(0, 0);
        Vector2 FinishSize = new Vector2(1, 1);
        float timer = 0.0f;
        float alarm = 2.0f;
        while (timer < alarm)
        {
            timer += Time.deltaTime;
            float timeScale = timer / alarm;
            Vector2 StartLerp = Vector2.Lerp(FinishPoint, StartPoint, timeScale);
            Vector2 ScreenLerp = Vector2.Lerp(FinishSize, StartSize, timeScale);

            Camera.rect = new Rect(StartLerp, ScreenLerp);

            yield return null;
        }
        ScreenAnimating = false;
    }

    private IEnumerator RotateCameraAroundPlayer()
    {
        Rotating = true;
        HoverFollowCam cam = GetComponentInChildren<HoverFollowCam>();
        cam.enabled = false;
        Vector3 LookAt = transform.position + Vector3.up * 1.5f;
        Vector3 OriginalPos = Camera.transform.position;
        Vector3 OriginalRot = Camera.transform.rotation.eulerAngles;
        Vector3 AnimatingPos = OriginalPos;
        AnimatingPos.y = OriginalPos.y - 1.5f;
        Camera.transform.position = AnimatingPos;
        while(Rotating)
        {
            Camera.transform.position = RotatePointAroundPivot(Camera.transform.position, transform.position, new Vector3(0.0f, Mathf.Rad2Deg * (Time.deltaTime) * 0.75f, 0.0f));
            Camera.transform.LookAt(LookAt);
            yield return null;
        }

        Camera.transform.position = OriginalPos;
        Camera.transform.rotation = Quaternion.Euler(OriginalRot);
        cam.enabled = true;
    }



    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }

}
