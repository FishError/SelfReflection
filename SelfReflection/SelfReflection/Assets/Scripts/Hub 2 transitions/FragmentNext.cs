using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentNext : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextFragments;
    public GameObject prevFragments;
    public GameObject Cube3;
    public GameObject Cube4;
    public GameObject Cube5;
    public GameObject Cube6;

    public void ShowNextFragments(){
        prevFragments.SetActive(false);
        Cube3.SetActive(false);
        Cube4.SetActive(false);
        nextFragments.SetActive(true);
        Cube5.SetActive(true);
        Cube6.SetActive(true);
    }

}
