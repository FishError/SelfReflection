using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class FragmentManager : MonoBehaviour
{
    [Tooltip("The list of GameObject references to keep track of.")]
    [SerializeField] private List<GameObject> _fragments;
    [Tooltip("Callbacks for when all fragments are in the zone.")]
    [SerializeField] private UnityEvent _onFragmentsCompleted;
    [Tooltip("The magnitude of explosive force to apply on the fragments.")]
    [SerializeField] private float _explosionForce;
    [Tooltip("The delay, in seconds, before the explosion after the last fragment enters the zone."), Min(0f)]
    [SerializeField] private float _explodeDelay;
    [Tooltip("The delay, in seconds, after the explosion that the fragments will be destroyed."), Min(0f)]
    [SerializeField] private float _destroyDelay;

    private IEnumerable<Rigidbody> _rbs;
    private HashSet<GameObject> _inZoneObjects;
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        // Cache
        _rbs = _fragments.Select(fr => fr.GetComponent<Rigidbody>());
        _inZoneObjects = new HashSet<GameObject>();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If other is one of the specified fragments and is not currently in zone
        if (_fragments.Contains(other.gameObject) && !_inZoneObjects.Contains(other.gameObject))
        {
            _inZoneObjects.Add(other.gameObject);
        }

        // If all the specified fragments are in zone, trigger event
        if (_inZoneObjects.Count == _fragments.Count)
        {
            StartCoroutine(CompleteFragments());
        }

        //If the first half of fragments collected, active remaining fragments
        if (_inZoneObjects.Count >= _fragments.Count/2)
        {
            foreach (var fragment in _fragments)
            {
                fragment.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_fragments.Contains(other.gameObject) && _inZoneObjects.Contains(other.gameObject))
        {
            _inZoneObjects.Remove(other.gameObject);
        }
    }

    private IEnumerator CompleteFragments()
    {
        yield return new WaitForSeconds(_explodeDelay);
        _onFragmentsCompleted?.Invoke();
        foreach (var rb in _rbs)
        {
            var tr = transform;
            rb.AddExplosionForce(_explosionForce, tr.position, tr.localScale.x * _sphereCollider.radius);
        }

        yield return new WaitForSeconds(_destroyDelay);
        foreach (var fragment in _fragments)
        {
            Destroy(fragment);
        }
        
        _fragments.Clear();
        _inZoneObjects.Clear();
    }
}