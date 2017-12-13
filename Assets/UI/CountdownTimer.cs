using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class CountdownTimer : MonoBehaviour {
    public UnityEvent myUnityEvent;
    public int Vibrato = 3;
    public float Elasticity = 0.8f;
    public float Duration = 1.0f;
    public Vector3 PunchScale = (Vector3.up + Vector3.right + Vector3.forward) * 1.25f;
    public Sprite[] sprites;
    private Image image;

    public void Restart()
    {
        StopCoroutine("Countdown");
        image = GetComponent<Image>();
        image.sprite = sprites[0];
        StartCoroutine("Countdown");
    }

    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
        StartCoroutine("Countdown");
	}

    private void OnFinish()
    {
        myUnityEvent.Invoke();
    }
    private IEnumerator Countdown()
    {
        float timer = 0.0f;
        int max = sprites.Length;
        int count = 1;
        image.transform.DOPunchScale(PunchScale, Duration, Vibrato, Elasticity);

        while (count < max)
        {
            timer += Time.deltaTime;
            if(timer >= 1.0f)
            {
                timer = 0.0f;
                image.transform.DOPunchScale(PunchScale, Duration, Vibrato, Elasticity);
                image.sprite = sprites[count];
                count++;
            }
            yield return null;
        }

        timer = 0.0f;
        while(timer < 1.0f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
        OnFinish();
    }
}
