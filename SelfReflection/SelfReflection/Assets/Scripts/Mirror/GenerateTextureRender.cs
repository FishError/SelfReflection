using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GenerateTextureRender : MonoBehaviour
{

    public Camera cam;
    private RenderTexture rt;
    private Material material;

    // Start is called before the first frame update
    void Awake()
    {
        material = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        rt = new RenderTexture(new RenderTextureDescriptor(1024, 1024, RenderTextureFormat.ARGB2101010));
        rt.depth = 16;
        cam.targetTexture = rt;
        material.SetTexture("_BaseMap", rt);
        GetComponent<Renderer>().material = material;
    }
}
