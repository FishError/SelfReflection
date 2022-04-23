using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    [SerializeField] private Material ethereal;
    [SerializeField] private Material real;

    private void Start()
    {
        if (IsEthereal())
            transform.GetComponent<MeshRenderer>().material = ethereal;
        else
            transform.GetComponent<MeshRenderer>().material = real;
    }

    public bool IsEthereal()
    {
        if (transform.tag == "Ethereal")
            return true;

        return false;
    }

    public void SetToEthereal()
    {
        transform.tag = "Ethereal";
        transform.GetComponent<MeshRenderer>().material = ethereal;
    }

    public void SetToReal()
    {
        transform.tag = "Real";
        transform.GetComponent<MeshRenderer>().material = real;
    }
}
