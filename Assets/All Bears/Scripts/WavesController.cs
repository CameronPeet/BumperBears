using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class WavesController : MonoBehaviour {

    public float height;
    public float time;

	// Use this for initialization
	void Start () {
        // iTween.MoveBy(this.gameObject, iTween.Hash("y", height, "time", time, "looptype,", "pingpong", "easytype", iTween.EaseType.easeInOutSine));

		this.transform.DOMoveY(transform.position.y + height, time).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
