using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float waitOnPickup = 0.2f;
    public float breakForce = 35f;
    [HideInInspector] public bool pickedUp = false;
    [HideInInspector] public PickupThroughMirrorController pickupThroughMirror;


    private void OnCollisionEnter(Collision collision)
    {
        if (pickedUp)
        {
            if(collision.relativeVelocity.magnitude > breakForce)
            {
                pickupThroughMirror.BreakConnection();
            }
        }
    }

    public IEnumerator PickUp()
    {
        yield return new WaitForSecondsRealtime(waitOnPickup);
        pickedUp = true;
    }
}
