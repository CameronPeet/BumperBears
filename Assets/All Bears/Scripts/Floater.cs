using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {

    public Transform seaPlane;
    public Cloth planeCloth;
    private int closestVertexIndex = -1;

    public float YOffset = 1.5f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if(planeCloth != null)
            GetClosestVertex();
	}

    void GetClosestVertex()
    {
        for(int i = 0; i < planeCloth.vertices.Length; i++)
        {
            if(closestVertexIndex == -1)
            {
                closestVertexIndex = i;
            }

            float distance = Vector3.Distance(planeCloth.vertices[i], transform.position);
            float closestDistance = Vector3.Distance(planeCloth.vertices[closestVertexIndex], transform.position);

            if(distance < closestDistance)
            {
                closestVertexIndex = i;
            }

            transform.position = new Vector3(transform.position.x, planeCloth.vertices[closestVertexIndex].y + YOffset, transform.position.z);
        }
    }
}
