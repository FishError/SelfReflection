using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTextureRender : MonoBehaviour
{

    public Camera cam;
    private RenderTexture rt;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = new Material(Shader.Find("Standard"));

        rt = new RenderTexture(new RenderTextureDescriptor(1024, 1024, RenderTextureFormat.ARGB32));
        cam.targetTexture = rt;
        material.SetTexture("_MainTex", rt);
        GetComponent<Renderer>().material = material;
    }
}
