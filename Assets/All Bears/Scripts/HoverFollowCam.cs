using UnityEngine;
using System.Collections;

public class HoverFollowCam : MonoBehaviour
{
    float m_camHeight;
    float m_camDist;
    public GameObject m_player;
    int m_layerMask;
    public float PosSlerpSpeed = 5.0f;

    Vector3 position = new Vector3(0, 4.88f, -5.63f);
    Vector3 Rotation = new Vector3(18.88f, 0, 0);
    Vector3 Bear = new Vector3(0, 0.724f, 2.6f);

    void Start()
    {
        Vector3 offsetCam = transform.position - m_player.transform.position;
        m_camHeight = offsetCam.y;
        m_camDist = Mathf.Sqrt(
            offsetCam.x * offsetCam.x + 
            offsetCam.z * offsetCam.z);

        m_layerMask = 1 << LayerMask.NameToLayer("Characters");
        m_layerMask = ~m_layerMask;
    }

    public void SetLookAt(GameObject player)
    {
        m_player = player;

        Vector3 offsetCam = transform.position - m_player.transform.position;
        m_camHeight = offsetCam.y;
        m_camDist = Mathf.Sqrt(
            offsetCam.x * offsetCam.x +
            offsetCam.z * offsetCam.z);

        m_layerMask = 1 << LayerMask.NameToLayer("Characters");
        m_layerMask = ~m_layerMask;
    }
	
    void FixedUpdate()
    {
        Vector3 camOffset = -m_player.transform.forward;
        camOffset = new Vector3(camOffset.x, 0.0f, camOffset.z) * m_camDist
            + Vector3.up * m_camHeight;

        Vector3 position = Vector3.Slerp(transform.position, m_player.transform.position + camOffset, Time.fixedDeltaTime * PosSlerpSpeed);

        transform.position = position;

        transform.LookAt(m_player.transform.position);
    }
}
