using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    public GameObject selectedObject;
    [Range(0, 255)] public int red;
    [Range(0, 255)] public int green;
    [Range(0, 255)] public int blue;
    [Range(0, 255)] public int emissionRed;
    [Range(0, 255)] public int emissionGreen;
    [Range(0, 255)] public int emissionBlue;

    public int maxReflections;
    public float maxReflectionDistance;
    public float maxGrabDistance = 5f;

    public int interactableLayer;
    private RaycastHit hit;
    private Ray ray;

    private MoveObjectController moveObjectController;
    private Color? originalColor;

    // Start is called before the first frame update
    void Start()
    {
        moveObjectController = GetComponent<MoveObjectController>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        for (int reflections = 0; reflections < maxReflections; reflections++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, maxReflectionDistance))
            {
                if (hit.collider.tag != "Mirror")
                {
                    if (hit.collider.transform.gameObject.layer == interactableLayer)
                    {
                        if (reflections > 0 || hit.distance < maxGrabDistance)
                        {
                            selectedObject = hit.transform.GetComponent<Interactable>().gameObject;
                        }
                    }
                    else
                    {
                        if (moveObjectController.interactable == null)
                        {
                            if (selectedObject)
                            {
                                var c = (Color)originalColor;
                                //Highlight(selectedObject, (int)(c.r * 255), (int)(c.g * 255), (int)(c.b * 255), false);
                                Highlight(selectedObject, 255, 255, 255, false);
                                originalColor = null;
                            }
                            selectedObject = null;
                        }
                    }
                    break;
                }
            }
            
            ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
        }
        
        if (moveObjectController.interactable != null || selectedObject != null)
        {
            Highlight(selectedObject, red, green, blue, true);
        }
    }

    private void Highlight(GameObject obj, int r, int g, int b, bool emission)
    {
        Renderer parentRenderer = obj.GetComponent<Renderer>();
        if (parentRenderer != null)
        {
            if (originalColor == null)
            {
                originalColor = parentRenderer.material.color;
            }
            HighlightMaterial(parentRenderer.material, r, g, b, emission);
        }

        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            HighlightMaterial(renderer.material, r, g, b, emission);
        }
    }

    private void HighlightMaterial(Material mat, int r, int g, int b, bool emission)
    {
        mat.color = new Color32((byte)r, (byte)g, (byte)b, 255);
        if (emission)
        {
            mat.SetColor("_EmissionColor", new Color32((byte)emissionRed, (byte)emissionGreen, (byte)emissionBlue, 255));
        }
        else
        {
            mat.SetColor("_EmissionColor", new Color32((byte)0, (byte)0, (byte)0, 255));
        }
    }

    private Transform GetParentTransform(Transform transform)
    {
        if (transform.GetComponent<Interactable>() != null)
        {
            return transform;
        }
        else
        {
            return GetParentTransform(transform.parent);
        }
    }
}
