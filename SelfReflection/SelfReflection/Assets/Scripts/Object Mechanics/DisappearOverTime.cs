using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearOverTime : MonoBehaviour
{
    public float time;
    public bool isStandingOnPlatform;


    private void Update()
    {
        if (isStandingOnPlatform)
        {
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
        isStandingOnPlatform = false;
    }
}
