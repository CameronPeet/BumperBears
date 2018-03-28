using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpikeTrap : MonoBehaviour {

    [Header("Spike")]
    public GameObject SpikePad;
    public GameObject Spikes;

    [Header("Trigger Values")]
    public float distance = 3.0f;
    public Ease EasingType;
    public float TweenTime;

    bool Triggered = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Trigger()
    {
        if(!Triggered)
        {
            Triggered = true;
            Spikes.transform.DOMove(Spikes.transform.position + Spikes.transform.forward * distance, TweenTime).SetEase(EasingType);
            StartCoroutine("Reverse");
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "BumperBear")
        {
            Trigger();
        }
    }

    private IEnumerator Reverse()
    {
        float timer = 0.0f;
        while(timer <= TweenTime * 2.0f + 1.0f)
        {
            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        Spikes.transform.DOMove(Spikes.transform.position - Spikes.transform.forward * distance, TweenTime).SetEase(EasingType);

        timer = 0.0f;
        while (timer <= TweenTime)
        {
            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        Triggered = false;

    }
}
