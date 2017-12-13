using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour {

    public GameObject ExplosionPrefab;
    public float BounceForce = 1000.0f;
    public float ExplodeAtMagnitude = 5.0f;
    public float ExplosionForce = 3000.0f;
    public float ExplosionRadius = 2.0f;
    Rigidbody Rigidbody;

    public bool Triggered = false;
    public bool Cooked = false;
    public float Timer = 0.0f;
    private float m_wait = 0.1f;
	// Use this for initialization
	void Start () {
        Rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Triggered)
        {
            Timer += Time.deltaTime;
            if(Timer >= m_wait)
            {
                Cooked = true;
            }
        }
	}


    void OnCollisionEnter(Collision col)
    {
        if(Cooked && Rigidbody.velocity.magnitude >= ExplodeAtMagnitude)
        {
            foreach (Rigidbody child in FindObjectsOfType<Rigidbody>())
            {
                if((child.GetComponent<Transform>().position - col.transform.position).magnitude <= ExplosionRadius)
                {
                    child.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
                }
            }
            DestroyObject(this.gameObject);
            var explosion = Instantiate<GameObject>(ExplosionPrefab);
            explosion.transform.position = transform.position;
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("BumperBear"))
        {
            Triggered = true;
            GetComponent<Rigidbody>().AddForce(Vector3.up * BounceForce * col.relativeVelocity.magnitude);
        }
    }


}
