using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    public GameObject barrier;
    public GameObject key;

    private void OnTriggerEnter(Collider other)
    {

        if (other.name == key.name)
        {
            barrier.SetActive(false);
        }
    }
}
