 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    [HideInInspector] public float limitYRotation = 0;
    private float minY;
    private float maxY;

    public float xRotation;
    public float yRotation;

    PlayerMovement playerMovement;

    public bool playerCamLocked;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement = orientation.transform.GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMovement.movementDisabled && !playerCamLocked)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            if (playerMovement.state == PlayerState.GrabbingLedge)
            {
                minY = limitYRotation - 90f;
                maxY = limitYRotation + 90f;
                if (yRotation < minY)
                    yRotation = minY;
                else if (yRotation > maxY)
                    yRotation = maxY;
            }

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            orientation.parent.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
