using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIntroAnimation : MonoBehaviour {

    Vector3[] Points;
    Vector3 LookAt;
	// Use this for initialization
	void Start () {

        LookAt = new Vector3(0, 1, 0);
        
        Vector3 P1 = new Vector3(0, 15, 25);
        Vector3 P2 = new Vector3(25, 15, 0);
        Vector3 P3 = new Vector3(0, 15, -25);
        Vector3 P4 = new Vector3(-25, 15, 0);
        Points = new Vector3[4];
        Points[0] = P1;
        Points[1] = P2;
        Points[2] = P3;
        Points[3] = P4;

        StartCoroutine("CameraAnimation", 0);
	}
	
    private IEnumerator CameraAnimation(int pointIndex)
    {

        float timer = 0.0f;
        float alarm = 1.5f;
        Vector3 Position = transform.position;
        while(timer < alarm)
        {
            timer += Time.deltaTime;
            float timeScale = timer / alarm;
            Vector3 Slerp = Vector3.Slerp(Position, Points[pointIndex], timeScale);
            transform.position = Slerp;
            transform.LookAt(LookAt);
            yield return null;
        }

        pointIndex++;
        if(pointIndex < Points.Length)
        {
            StartCoroutine("CameraAnimation", pointIndex);
        }


    }
}
