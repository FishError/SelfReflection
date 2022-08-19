using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairSeatPlayerCheck : MonoBehaviour
{
    public PlayerMovement player;
    public bool isPlayerGrabbing=false;
    private void OnTriggerStay(Collider other){

	    if(other.gameObject.tag=="Player" && (player.state.Equals(PlayerState.GrabbingLedge) || player.state.Equals(PlayerState.ClimbingLedge))){
		    isPlayerGrabbing=true;
	    }
	    else if(other.gameObject.tag=="Player" && player.state.Equals(PlayerState.Grounded)){
		    isPlayerGrabbing=false;
	    }

	}
}
