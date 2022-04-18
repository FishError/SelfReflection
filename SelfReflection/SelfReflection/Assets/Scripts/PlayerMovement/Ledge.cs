using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ledge forward direction needs to face in the opposite direction of the ledge

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
                    //player.GrabLedge(GrabPosition.position, transform.position, transform.forward);
                }
            }
        }
    }
}
