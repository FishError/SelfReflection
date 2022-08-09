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
    

    [Tooltip("The margin of error for rotation")]
    [SerializeField] private float _targetRotationLeeway;
    [Tooltip("The value of the object's scale")]
    [SerializeField] private float _targetSize;
    [Tooltip("The margin of error for scale")]
    [SerializeField] private float _targetSizeLeeway;

    private BoxCollider _boxCollider;
    private InteractionController interactionController;
    public bool inPosition = false;


    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        interactionController = GameObject.Find("PlayerCam").GetComponent<InteractionController>();
    }

    private void Update()
    {
         
    }
    
    private void OnTriggerStay(Collider other)
    {
        //If other is the specified object
        if (_object == other.gameObject)
        {
            if(checkRotation(_boxCollider.gameObject, other.gameObject) && checkSize(other.gameObject)){
                
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

    bool checkRotation(GameObject zone, GameObject other)
    {
        return Mathf.Abs(zone.transform.rotation.eulerAngles.y - other.transform.rotation.eulerAngles.y) <=_targetRotationLeeway;
    }

    bool checkSize(GameObject other)
    {
        return other.transform.localScale.x <=_targetSize+_targetSizeLeeway && other.transform.localScale.x >= _targetSize - _targetSizeLeeway;
    }
}
