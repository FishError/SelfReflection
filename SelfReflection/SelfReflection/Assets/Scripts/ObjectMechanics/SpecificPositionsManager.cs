using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpecificPositionsManager : MonoBehaviour
{
    [Tooltip("The GameObject reference to keep track of.")]
    [SerializeField] private GameObject _object;
    [Tooltip("Callbacks for when object are in the zone and in correct position, rotation, and size.")]
    [SerializeField] private UnityEvent _onObjectInPosition;

    [Tooltip("The target rotation on the x-axis for rotation")]
    [SerializeField] private float _targetRotationX;
    [Tooltip("The target rotation on the y-axis for rotation")]
    [SerializeField] private float _targetRotationY;
    [Tooltip("The target rotation on the z-axis for rotation")]
    [SerializeField] private float _targetRotationZ;
    [Tooltip("The margin of error for x-axis rotation")]
    [SerializeField] private float _xRotationLeeway;
    [Tooltip("The margin of error for y-axis rotation")]
    [SerializeField] private float _yRotationLeeway;
    [Tooltip("The margin of error for z-axis rotation")]
    [SerializeField] private float _zRotationLeeway;
    [Tooltip("The target value of the object's scale")]
    [SerializeField] private float _targetSize;
    [Tooltip("The margin of error for scale")]
    [SerializeField] private float _sizeLeeway;

    private InteractionController interactionController;
    public bool inPosition = false;


    private void Awake()
    {
        interactionController = GameObject.Find("PlayerCam").GetComponent<InteractionController>();
    }

    
    private void OnTriggerStay(Collider other)
    {
        //If other is the specified object
        if (_object == other.gameObject)
        {
            if(checkRotation(other.gameObject) && checkSize(other.gameObject)){
                ObjectInPosition();
            }
        }
    }

    private void ObjectInPosition()
    {
        _onObjectInPosition?.Invoke();
        _object.GetComponent<InteractableObject>().gameObject.layer = 0;
        interactionController.DropObject();
        this.gameObject.SetActive(false);

    }

    bool checkRotation(GameObject other)
    {

        return Mathf.Abs(other.transform.rotation.eulerAngles.x - _targetRotationX) <= _xRotationLeeway &&
            Mathf.Abs(other.transform.rotation.eulerAngles.y - _targetRotationY) <= _yRotationLeeway &&
            Mathf.Abs(other.transform.rotation.eulerAngles.z - _targetRotationZ) <= _zRotationLeeway;

    }

    bool checkSize(GameObject other)
    {
        return other.transform.localScale.x <=_targetSize+_sizeLeeway && other.transform.localScale.x >= _targetSize - _sizeLeeway;
    }
}
