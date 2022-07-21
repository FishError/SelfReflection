using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceWithMirror : MonoBehaviour
{
    public GameObject objectToHide;
    public GameObject mirrorToShow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other){
        if(other.tag=="Player"){
            objectToHide.SetActive(false);
            mirrorToShow.SetActive(true);
        }
    }
}
