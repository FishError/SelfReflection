using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MirrorManager : MonoBehaviour
{
    public Camera mainCamera;
    public List<GameObject> mirrors;

    // Update is called once per frame
    void Update()
    {
        setMirrorCameraPosition(mainCamera, mirrors, 1);
    }

    private void setMirrorCameraPosition(Camera cam, List<GameObject> mirrors, int depth)
    {
        if (depth < 5)
        {
            var closedMirrors = new List<GameObject>();
            var openMirrors = new List<GameObject>(mirrors);
            foreach (GameObject mirror in mirrors)
            {
                var m = mirror.GetComponentInChildren<GenerateTextureRender>().gameObject;
                Vector3 viewPos = cam.WorldToViewportPoint(m.transform.position);
                
                if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
                {
                    var dir = cam.transform.position - m.transform.position;
                    var x = Vector3.Dot(dir, m.transform.forward);
                    if (x > 0)
                    {
                        mirror.GetComponentInChildren<SetMirroredPosition>().mainCamera = cam;
                        closedMirrors.Add(mirror);
                        openMirrors.Remove(mirror);
                    }
                }
            }

            if (closedMirrors.Count > 0)
            {
                var minDistance = closedMirrors.Min(m => Vector3.Distance(cam.WorldToViewportPoint(m.GetComponentInChildren<GenerateTextureRender>().transform.position), new Vector3(0.5f, 0.5f, 0f)));
                var closestMirror = closedMirrors.First(m => Vector3.Distance(cam.WorldToViewportPoint(m.GetComponentInChildren<GenerateTextureRender>().transform.position), new Vector3(0.5f, 0.5f, 0f)) == minDistance);
                setMirrorCameraPosition(closestMirror.GetComponentInChildren<Camera>(), openMirrors, depth + 1);
            }
        }
    }
}
