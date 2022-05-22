using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerReset : MonoBehaviour
{
    [SerializeField] private float time = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Void")
        {
            StartCoroutine(timeToDie());
        }
    }

    IEnumerator timeToDie()
    {
        yield return new WaitForSeconds(time);
        print("Player is Dead");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
