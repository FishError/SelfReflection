using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTextureRender : MonoBehaviour
{

    public Camera cam;
    private RenderTexture rt;
    private Material material;
    private SetMirroredPosition setMirroredPosition;
    private Camera selfCamera;

    // Start is called before the first frame update
    void Start()
    {
        material = new Material(Shader.Find("Standard"));

        rt = new RenderTexture(new RenderTextureDescriptor(1024, 1024, RenderTextureFormat.ARGB2101010));
        cam.targetTexture = rt;
        material.SetTexture("_MainTex", rt);
        GetComponent<Renderer>().material = material;

        setMirroredPosition = transform.parent.GetComponentInChildren<SetMirroredPosition>();
        selfCamera = transform.parent.GetComponentInChildren<Camera>();
    }

    private void OnWillRenderObject()
    {
        if (Camera.current != null)
        {
            if (Camera.current.name != "SceneCamera" && Camera.current.name != "Preview Camera" && Camera.current != selfCamera)
            {
                //setMirroredPosition.mainCamera = Camera.current;
            }
        }
    }
}
