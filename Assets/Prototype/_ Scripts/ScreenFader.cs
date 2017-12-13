using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour {

    public bool AutoStart = false;
    public bool Running = false;
    public bool FadingIn = false;
    Image image;    
    
    void Start()
    {
        image = GetComponent<Image>();
        if(AutoStart)
        {
            FadePanelIn();
        }
    }

    public void FadeScreen()
    {
        if (!Running)
            StartCoroutine("ScreenFade");
    }
    public void FadePanelIn()
    {
        if (!Running)
            StartCoroutine("FadeIn");
    }
    public void FadePanelOut()
    {
        if (!Running)
            StartCoroutine("FadeOut");
    }
    public void LoadScene(string SceneNameToLoad)
    {
        if(!Running)
            StartCoroutine("FadeToScene", SceneNameToLoad);
    }

    private IEnumerator ScreenFade()
    {
        Running = true;
        float timer = 0.0f;
        float alarm = 1.5f;
        while(timer < alarm)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0.0f, 1.0f, timer);

            Vector4 color = image.color;
            color.w = alpha;
            image.color = color;
            yield return null;
        }

        StartCoroutine("FadeOut");
    }
    private IEnumerator FadeToScene(string SceneName)
    {
        Running = true;
        FadingIn = true;
        float timer = 0.0f;
        float alarm = 1.5f;
        while (timer < alarm)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, timer);
            timer += Time.deltaTime;
            Vector4 color = image.color;
            color.w = alpha;
            image.color = color;

            yield return null;
        }
        Running = false;
        FadingIn = false;
        SceneManager.LoadScene(SceneName);
    }

    private IEnumerator FadeOut()
    {
        Running = true;
        FadingIn = true;
        float timer = 0.0f;
        float alarm = 1.5f;
        while (timer < alarm)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, timer);
            timer += Time.deltaTime;
            Vector4 color = image.color;
            color.w = alpha;
            image.color = color;

            yield return null;
        }
        Running = false;
        FadingIn = false;
    }

    private IEnumerator FadeIn()
    {
        Running = true;
        FadingIn = true;
        float timer = 0.0f;
        float alarm = 1.5f;
        while (timer < alarm)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0.0f, 1.0f, timer);

            Vector4 color = image.color;
            color.w = alpha;
            image.color = color;
            yield return null;
        }

        Running = false;
        FadingIn = false;

    }
}
