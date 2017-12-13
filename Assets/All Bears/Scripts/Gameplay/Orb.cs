using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Orb : MonoBehaviour {

    float cooldown = 1.0f;
    float timer = 0.0f;
    bool justDropped = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
        if(justDropped)
        {
            timer += Time.deltaTime;
            if(timer >= cooldown)
            {
                timer = 0.0f;
                justDropped = false;
                GetComponentInChildren<SphereCollider>().enabled = true;

            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (justDropped)
            return;

        if (other.gameObject.tag == "BumperBear")
        {
            transform.position = other.transform.position;
            transform.parent = other.transform;
            GetComponentInChildren<SphereCollider>().enabled = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    public void Drop()
    {
        //Vector3 Position = transform.position;

        //print("Orb pos = " + transform.position);
        //print("World pos = " + Position);
        //print(GetComponentInParent<Player>().gameObject.transform.position);

        transform.parent = null;

        Vector3 jump = Random.insideUnitSphere * 2.5f;
        jump.y = 1.0f;
        jump += transform.position;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.DOJump(jump, 2.5f, 1, 1.5f);
        GetComponentInChildren<SphereCollider>().enabled = true;
        GetComponentInChildren<MeshRenderer>().enabled = true;
        justDropped = true;
    }
}
