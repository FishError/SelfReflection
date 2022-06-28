using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CallElevator : MonoBehaviour
{
    public GameObject elevatorObject;
    private ElevatorSystem elevator;
    private InteractablePlatformLimitedMovement platformSystem=null;
    public AudioManager audioManager;
    public GameObject audioSource;

    void Start(){
        elevator=elevatorObject.GetComponent<ElevatorSystem>();
        if(elevatorObject.GetComponent<InteractablePlatformLimitedMovement>()!=null){
            platformSystem = elevatorObject.GetComponent<InteractablePlatformLimitedMovement>();
        }
        else{
            Debug.Log("The Elevator does not have a platform system. This warning is intended.");
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Battery") && elevator != null)
        {
            elevator.isOff = false;
            elevator.CallElevator();
            if(platformSystem != null){
                Destroy(elevatorObject.GetComponent<InteractablePlatformLimitedMovement>());
            }
            audioManager.setCurAudioObject(audioSource);
            audioManager.setPlayback(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        elevator.isOff = true;
    }
}
