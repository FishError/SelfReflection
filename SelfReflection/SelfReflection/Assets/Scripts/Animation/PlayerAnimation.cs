using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerMovement player;
    Animator animator;
    int isWalkingHash;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
    }

    private void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");
        bool backPressed = Input.GetKey("s");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool jumpPressed = Input.GetKey("space");

        if (!isWalking && forwardPressed || backPressed || leftPressed || rightPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (isWalking && !forwardPressed && !backPressed && !leftPressed && !rightPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if(jumpPressed || (forwardPressed && jumpPressed) || player.state.Equals(PlayerState.AirBorn))
        {
            animator.SetBool("isJumping", true);
        }

        if (!jumpPressed && player.state.Equals(PlayerState.Grounded))
        {
            animator.SetBool("isJumping", false);
        }

    }
}
