using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class SplashScreen : MonoBehaviour
{
    public UnityEvent myUnityEvent;
    public float TimeSpentOnScreen = 3.0f;
    Image image;

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine("Countdown");
        float length = 1.25f;
        image.rectTransform.DOPunchPosition(new Vector3(1, 1, 1), length, 3, 0.6f);
        image.rectTransform.DOPunchRotation(new Vector3(1, 1, 1), length, 3, 0.6f);
        image.rectTransform.DOPunchScale(new Vector3(1.15f, 1.15f, 1.15f), length, 3, 0.6f);
    }

    private void OnFinish()
    {
        myUnityEvent.Invoke();
    }
    private IEnumerator Countdown()
    {
        float timer = 0.0f;

        timer = 0.0f;
        while (timer < TimeSpentOnScreen)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        OnFinish();
    }
}
