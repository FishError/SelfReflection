using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanningController : MonoBehaviour
{
    public Camera panningCamera;
    public Camera playerCamera;
    public PlayerMovement playerMovement;

    public List<GameObject> cameraActions;
    public float cameraSpeed;
    public float rotationSpeed;

    private GameObject currentAction, mirrorManager, progressBar;
    private Vector3 direction;
    private Quaternion lookRotation;
    public bool isPanning = false;

    private void Start()
    {
        mirrorManager = GameObject.Find("MirrorManager");
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        progressBar = GameObject.Find("ProgressBar").transform.GetChild(0).gameObject;
        StartPanning();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAction)
        {
            direction = currentAction.transform.forward;
            lookRotation = Quaternion.LookRotation(direction);
            panningCamera.transform.rotation = Quaternion.Slerp(panningCamera.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            if (panningCamera.transform.position != currentAction.transform.position)
            {
                panningCamera.transform.position = Vector3.MoveTowards(panningCamera.transform.position, currentAction.transform.position, cameraSpeed * Time.deltaTime);
            }
            else
            {
                int nextIndex = cameraActions.IndexOf(currentAction) + 1;
                if (nextIndex < cameraActions.Count)
                {
                    currentAction = cameraActions[cameraActions.IndexOf(currentAction) + 1];
                }
                else
                {
                    currentAction = null;
                    panningCamera.enabled = false;
                    playerCamera.enabled = true;

                    if (mirrorManager != null)
                    {
                        mirrorManager.GetComponent<MirrorManager>().mainCamera = playerCamera;
                    }

                    playerMovement.EnableMovement();
                    isPanning = false;
                    if (progressBar.transform.parent.gameObject.activeInHierarchy)
                    {
                        progressBar.transform.parent.gameObject.SetActive(false);
                        progressBar.GetComponent<SkipProgressBarAnimation>().curProgress.transform.localScale = new Vector3(0, 1, 1);
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            progressBar.GetComponent<SkipProgressBarAnimation>().EnableBarAnimation();
            if (!progressBar.GetComponent<SkipProgressBarAnimation>().isPlaying)
            {
                StopPanning();
            }
        }
        else
        {
            progressBar.GetComponent<SkipProgressBarAnimation>().ResetBarAnimation();
        }
        
        if(mirrorManager.GetComponent<MirrorManager>().mainCamera.name == playerCamera.name)
        {
            StartCoroutine(timeToEnableMovement());
        }

    }

    public void StartPanning()
    {
        playerCamera.enabled = false;
        panningCamera.enabled = true;
        playerMovement.DisableMovement();
        currentAction = cameraActions[0];
        isPanning = true;
    }

    public void StopPanning()
    {
        playerCamera.enabled = true;
        panningCamera.enabled = false;
        currentAction = null;
        isPanning = false;
        if (mirrorManager != null)
        {
            mirrorManager.GetComponent<MirrorManager>().mainCamera = playerCamera;
        }
        progressBar.transform.parent.gameObject.SetActive(false);
        progressBar.GetComponent<SkipProgressBarAnimation>().curProgress.transform.localScale = new Vector3(0, 1, 1);
    }

    IEnumerator timeToEnableMovement()
    {
        yield return new WaitForSeconds(1f);
        playerMovement.EnableMovement();
    }
}
