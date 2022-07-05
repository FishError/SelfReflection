using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovementZone : MonoBehaviour
{
    public GameObject mirror;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !mirror.GetComponent<MovingMirrors>().isMoving)
        {
            mirror.GetComponent<MovingMirrors>().isMoving = true;
        }
    }
}
