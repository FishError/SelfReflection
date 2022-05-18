using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearOverTime : MonoBehaviour
{
    public float time;

   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(time);
        this.gameObject.transform.parent.gameObject.SetActive(false);

    }
}
