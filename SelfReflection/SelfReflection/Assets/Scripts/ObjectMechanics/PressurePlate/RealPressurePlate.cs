using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealPressurePlate : MonoBehaviour
{
    public Rigidbody wall;
    public float speed = 1.0f;
    public float doorHeight = 10.0f;

    private float sinkAmount = 0.1f;
    private Vector3 startPos;
    private Vector3 endPos;

    private void Start()
    {
        startPos = wall.transform.position;
        endPos = new Vector3(startPos.x, startPos.y - doorHeight, startPos.z);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Real")
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - sinkAmount, gameObject.transform.position.z);
            MoveDoor(endPos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Real")
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + sinkAmount, gameObject.transform.position.z);
        }
    }

    private void MoveDoor(Vector3 goalPos)
    {
        float dist = Vector3.Distance(wall.transform.position, goalPos);

        while (dist > .1f)
        {
            wall.transform.position = Vector3.Lerp(wall.transform.position, goalPos, speed * Time.deltaTime);
            dist = Vector3.Distance(wall.transform.position, goalPos);
        }
    }
}
