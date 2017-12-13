using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour {

    public void Disable(float time)
    {
        StartCoroutine(DisableIn(time));
    }

    private IEnumerator DisableIn(float alarm)
    {
        float timer = 0.0f;

        while(timer < alarm)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        this.gameObject.SetActive(false);
    }
}
