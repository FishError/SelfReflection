using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSystem : MonoBehaviour
{
	public Vector3 FloorDistance = Vector3.up;
	public float Speed = 1.0f;
	public int Floor = 0;
	public int MinFloor = 0;
	public int MaxFloor = 1;
	public Transform moveTransform;
	public bool isOff;
	public bool CanGoUp = true;

	private float tTotal;
	private bool isMoving;
	private float moveDirection;


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
			MoveDown();
			if(Floor == MinFloor)
            {
				isMoving = false;
				isOff = false;
				CanGoUp = true;
            }
        }
		
	}

	void MoveElevator()
	{
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
			MoveUp();
		}
		else if(Floor > MinFloor)
		{
			CanGoUp = false;
			MoveDown();
		}
		else if(Floor == MinFloor)
        {
			CanGoUp = true;
			MoveUp();
        }
	}
}