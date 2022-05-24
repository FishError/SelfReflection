using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveAnimator : MonoBehaviour
{
    [SerializeField]
    private bool waveX, waveY, waveZ;
    [SerializeField]
    private float intensity = 1.0f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 sinWave = new Vector3(0, 0, 0);
        if (waveX) sinWave.x = Mathf.Sin(Time.time);
        if (waveY) sinWave.y = Mathf.Sin(Time.time);
        if (waveZ) sinWave.z = Mathf.Sin(Time.time);
        sinWave = sinWave * intensity;
        transform.position = startPos + sinWave;
    }
}
