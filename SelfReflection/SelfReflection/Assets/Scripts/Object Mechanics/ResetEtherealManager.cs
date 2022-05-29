using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEtherealManager : MonoBehaviour
{
    private ResetEtherealManager etherealManager;

    private void Start()
    {
        etherealManager = GameObject.Find("ObjectManager").GetComponent<ResetEtherealManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ethereal")
        {

        }
    }
}
