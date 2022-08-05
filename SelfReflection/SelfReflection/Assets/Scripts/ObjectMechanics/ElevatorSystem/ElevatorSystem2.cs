using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSystem2 : ElevatorSystem
{
	public Vector3 FloorDistance = Vector3.up;
	public float Speed = 1.0f;
	public int Floor = 0;
	public int MinFloor = 0;
	public int MaxFloor = 1;
	public Transform moveTransform;
	public bool isOff;
	public bool CanGoUp = true;
	public bool isPlayerOn;

	private float tTotal;
	private bool isMoving;
	private float moveDirection;
	private float theTime = 0;
	private float randNum = 0;
	private float randNum1 = 0;


	// Use this for initialization
	void Start()
	{
		moveTransform = moveTransform ?? transform;
	}

	// Update is called once per frame
	void Update()
	{
		if (isMoving)
		{
			MoveElevator();
		}

		if (isOff)
		{
			theTime = 0;
			MoveDown();
			if (Floor == MinFloor)
			{
				isMoving = false;
				isOff = false;
				CanGoUp = true;
			}
		}
		/*
		if(moveWait){
			MoveUpWait();
		}
		*/

	}

	void MoveElevator()
	{
		if (randNum == 0 && randNum1 == 0)
			randNum = Random.Range(1f, 3f);
		randNum1 = Random.Range(6f, 12f);
		var v = moveDirection * FloorDistance.normalized * Speed;
		var t = Time.deltaTime;
		var tMax = FloorDistance.magnitude / Speed;
		t = Mathf.Min(t, tMax - tTotal);
		moveTransform.Translate(v * t);
		tTotal += t;

		if (tTotal >= tMax)
		{
			isMoving = false;
			tTotal = 0;
			Floor += (int)moveDirection;
		}
	}

	public void MoveUp()
	{
		if (isMoving)
			return;

		isMoving = true;
		moveDirection = 1;
	}
	/*
	public IEnumerator MoveUpWait(){
		 yield return new WaitForSeconds(3f);
		if (isMoving)
			yield break;

		isMoving = true;
		moveDirection = 1;
	}
	*/


	public void MoveDown()
	{
		if (isMoving)
			return;

		isMoving = true;
		moveDirection = -1;
	}

	public void CallElevator()
	{
		if (isMoving)
			return;

		if (Floor < MaxFloor && CanGoUp)
		{
			theTime = 0;
			MoveUp();
		}
		else if (Floor == MaxFloor)
		{
			Debug.Log(isPlayerOn);
			if (!isPlayerOn)
			{
				theTime += Time.deltaTime;
				if (theTime >= randNum1)
				{
					randNum1 = Random.Range(6f, 12f);
					theTime = 0;
					CanGoUp = false;
					MoveDown();
				}
			}
		}
		else if (Floor > MinFloor)
		{
			CanGoUp = false;
			MoveDown();
		}
		else if (Floor == MinFloor)
		{
			theTime += Time.deltaTime;
			if (theTime >= randNum)
			{
				randNum = Random.Range(1f, 3f);
				theTime = 0;
				CanGoUp = true;
				MoveUp();
			}
		}
	}
	/*
	private void OnTriggerStay(Collider other)
	{
		Debug.Log(other.gameObject.tag);
		if (other.gameObject.tag == "Player")
		{
			isPlayerOn = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			isPlayerOn = false;
			Debug.Log("OnTriggerExit: " + isPlayerOn);
		}
	}
	*/
}
