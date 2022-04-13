using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    public Transform GrabPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LedgeGrab")
        {
            PlayerMovement player = other.transform.GetComponentInParent<PlayerMovement>();
            if (player != null)
            {
                if (GrabPosition != null)
                {
                    player.GrabLedge(GrabPosition.position, transform.position);
                }
            }
        }
    }
}
