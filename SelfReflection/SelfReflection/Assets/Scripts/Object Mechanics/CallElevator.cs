using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallElevator : MonoBehaviour
{
	public ElevatorSystem elevator;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Battery") && elevator != null)
        {
            elevator.isOff = false;
            elevator.CallElevator();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        elevator.isOff = true;
    }
}
