using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDelete : MonoBehaviour {
    public float LifeTime = 1.0f;
    public bool Activate;
    public float timer = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Activate)
        {
            timer += Time.deltaTime;
            if (timer > LifeTime)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
