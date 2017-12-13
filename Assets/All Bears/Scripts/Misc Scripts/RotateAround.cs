using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

    public bool animate = false;
    public Transform m_lookAt;
    public Transform m_rotateAround;
    public Vector3 m_origin;

    public float m_animationLength = 20.0f;
    public float m_angle = 360.0f;

    Camera m_camera;
    float m_timer = 0.0f;
	// Use this for initialization
	void Start () {
        m_camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if(animate)
        {
            m_timer += Time.deltaTime;
            float timeScale = m_timer / m_animationLength;
            float angle = m_angle * timeScale;
            Vector3 position = RotatePointAroundPivot(m_origin, m_rotateAround.position, new Vector3(0.0f, angle, 0.0f));
            transform.position = position;
            m_camera.transform.LookAt(m_lookAt);
        }
	}

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }


    public void StartAnimating()
    {
        animate = true;
        m_origin = transform.position;

    }

    public void StopAnimating()
    {
        animate = false;
    }
}
