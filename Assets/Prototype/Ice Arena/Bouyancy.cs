using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouyancy : MonoBehaviour {

    public float WaterLevel;
    public Vector3 BuoyancyCentreOffset;
    public float BounceDamping = 0.75f;
    public float FloatForce = 10000.0f;
    private Vector3[] BuoyancyPoints;

    bool running = false;

    void Awake()
    {
        BuoyancyPoints = new Vector3[12];
        running = true;
    }

    void OnDrawGizmos()
    {
        if(running)
        {
            for (int i = 0; i < BuoyancyPoints.Length; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(BuoyancyPoints[i], 0.2f);
            }
        }
    }

    void CalculateBuoyancyPoints()
    {
        for (int i = 0; i < BuoyancyPoints.Length; i++)
        {
            Vector3 Edge = (transform.position + transform.right * 20.0f);
            BuoyancyPoints[i] = Quaternion.AngleAxis(i * 30.0f, transform.up) * Edge;

            Vector3 ActionPoint = BuoyancyPoints[i];
            if (ActionPoint.y < WaterLevel + 0.1f)
            {
                float distance = ActionPoint.y / WaterLevel;
                Vector3 UpLift = -Physics.gravity + transform.up * FloatForce * distance;
                GetComponent<Rigidbody>().AddForceAtPosition(UpLift, ActionPoint);
            }
        }
    }
    void FixedUpdate()
    {
        CalculateBuoyancyPoints();
    }
}
