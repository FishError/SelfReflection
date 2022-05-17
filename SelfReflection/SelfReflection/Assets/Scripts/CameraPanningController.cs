using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraAction
{
    public Vector3 MoveTo;
    public float MoveSpeed;
    public Vector3 LookTowards;
    public float RotationSpeed;
}

public class CameraPanningController : MonoBehaviour
{
    public Camera panningCamera;
    public Camera playerCamera;

    public List<CameraAction> cameraActions;
    public List<GameObject> cameraActions2;
    public float cameraSpeed;
    public float rotationSpeed;

    private GameObject currentAction;
    private Vector3 direction;
    private Quaternion lookRotation;

    private void Start()
    {
        StartPanning();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAction)
        {
            direction = (currentAction.transform.GetChild(0).transform.position - currentAction.transform.position);
            lookRotation = Quaternion.LookRotation(direction);
            panningCamera.transform.rotation = Quaternion.Slerp(panningCamera.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            if (panningCamera.transform.position != currentAction.transform.position)
            {
                panningCamera.transform.position = Vector3.MoveTowards(panningCamera.transform.position, currentAction.transform.position, cameraSpeed * Time.deltaTime);
            }
            else
            {
                int nextIndex = cameraActions2.IndexOf(currentAction) + 1;
                if (nextIndex < cameraActions2.Count)
                {
                    currentAction = cameraActions2[cameraActions2.IndexOf(currentAction) + 1];
                }
                else
                {
                    currentAction = null;
                    panningCamera.enabled = false;
                    playerCamera.enabled = true;
                }
            }
        }
    }

    public void StartPanning()
    {
        playerCamera.enabled = false;
        panningCamera.enabled = true;
        currentAction = cameraActions2[0];
    }
}
