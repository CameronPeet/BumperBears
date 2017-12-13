using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDragOnTrigger : MonoBehaviour {

    private List<KeyValuePair<Rigidbody, float>> m_Bodies;
	// Use this for initialization
	void Start () {
        m_Bodies = new List<KeyValuePair<Rigidbody, float>>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnTriggerEnter(Collider collider)
    {
        print("trigger");
        if(collider.gameObject.layer == LayerMask.NameToLayer("Characters"))
        {
            Rigidbody key = collider.gameObject.GetComponent<Rigidbody>();
            float drag = key.drag;
            KeyValuePair<Rigidbody, float> newPair = new KeyValuePair<Rigidbody, float>(key, drag);

            m_Bodies.Add(newPair);
            print("added body " + collider.name  + " with drag of " + collider.gameObject.GetComponent<Rigidbody>().drag);

            key.drag = 35;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        print("trigger");
        if (collider.gameObject.layer == LayerMask.NameToLayer("Characters"))
        {
            foreach(KeyValuePair<Rigidbody, float> body in m_Bodies)
            {
                if(body.Key.gameObject == collider.gameObject)
                {
                    body.Key.drag = body.Value;
                    print("body drag reset to " + body.Key.gameObject.name + " drag = " + body.Value);
                    return;
                }
            }
        }
    }
}
