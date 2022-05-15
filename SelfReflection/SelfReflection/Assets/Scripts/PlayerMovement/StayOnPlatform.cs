using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnPlatform : MonoBehaviour
{
    private RaycastHit downCastHit;
    public List<DisappearOverTime> platformList = new List<DisappearOverTime>();
    public PlayerMovement player;
    private int count = 0;

    private void Update()
    {
        CheckIfOnPlatform();
    }

    public void CheckIfOnPlatform()
    {
        var down = Physics.Raycast(transform.position, Vector3.down, out downCastHit);
        if(player.state.Equals(PlayerState.Grounded) && downCastHit.transform.gameObject.tag == "Platform")
        {
            print("On Platform " + count);
            platformList[count].isStandingOnPlatform = true;
            

        }
        
    }
}
