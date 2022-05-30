using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearOverTime : MonoBehaviour
{
    public float time;
    public GameObject bookModel;
    public float spawnTime = 5f;

    private void Update()
    {
        if (!bookModel.activeInHierarchy)
        {
            StartCoroutine(Appear());
        }
    }

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
        this.transform.parent.GetComponent<BoxCollider>().isTrigger = true;
        bookModel.SetActive(false);
    }

    IEnumerator Appear()
    {
        yield return new WaitForSeconds(spawnTime);
        this.transform.parent.GetComponent<BoxCollider>().isTrigger = false;
        bookModel.SetActive(true);
    }

}
