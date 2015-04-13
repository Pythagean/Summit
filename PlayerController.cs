﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public KeyCode Left = KeyCode.A;
	public KeyCode Right = KeyCode.D;
	public KeyCode Jump = KeyCode.W;
	public KeyCode ShootArrow = KeyCode.Mouse0;
	public KeyCode ShootGrappleHook = KeyCode.Mouse1;
	

	public float gravity = -15f;
	public float runSpeed = 8f;
	public float decaySpeed = 0.5f;
	public float jumpHeight = 1.8f;
	
	public int numberOfGrapples = 5;
	public int numberOfArrows = 20;

	private CharacterController2D _controller;
	private Animator _animator;

	private RaycastHit2D _raycastHitGrapple;
	
	private bool grappling = false;

	List<GameObject> arrowList;
	public GameObject ArrowPrefab;

	void Awake()
	{
		_controller = GetComponent<CharacterController2D> ();
		_animator = GetComponent<Animator> ();
		arrowList = new List<GameObject>();
	}

	void Update()
	{
		//Debug.Log(name.ToString());
		//Grabs current velocity of player
		var velocity = _controller.velocity;

		//Debug.Log ("X Velocity: " + _controller.velocity.x);
		//Debug.Log ("Y Velocity: " + _controller.velocity.y);

		if (_controller.isGrounded)
				velocity.y = 0;

		//Horizontal Input
		if (Input.GetKey (Right)) 
		{
			velocity.x = runSpeed;
			//goRight();
		}
		else if( Input.GetKey(Left))
		{
			velocity.x = -runSpeed;
			//goLeft();
		}
		if(_controller.isGrounded)
		{
			if(velocity.x < 0.001 && velocity.x > -0.001)
			{
				velocity.x = 0;
			}
			else
			{
				velocity.x *= decaySpeed;
			}
		}
		else
		{
			if(velocity.x < 0.001 && velocity.x > -0.001)
			{
				velocity.x = 0;
			}
			else
			{
				velocity.x *= decaySpeed;
			}
		}
	
		if (Input.GetKeyDown (ShootArrow) || Input.GetKeyDown(ShootGrappleHook))
		{
			//GetComponent<Camera>() = Camera.main;
			if ((Input.GetKeyDown (ShootArrow) || grappling == false) && numberOfArrows > 0)
			{
				//Get positions of mouse/player and draw ray
				Vector3 mousePositionVector = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.nearClipPlane);
				var mousePositionWorldVector = Camera.main.ScreenToWorldPoint(mousePositionVector);
				Vector3 controllerPosition = new Vector3(_controller.transform.position.x,_controller.transform.position.y);
				Debug.DrawRay(controllerPosition,(mousePositionWorldVector-controllerPosition),Color.green,1);

				//Create Arrow Object
				GameObject arrowInstance = (GameObject)Instantiate(ArrowPrefab, controllerPosition, Quaternion.identity);
				numberOfArrows -= 1;
				
				//Shoot the grapple
				if (Input.GetKeyDown (ShootGrappleHook) && numberOfGrapples > 0)
				{
					arrowScript arrowScriptInstance = arrowInstance.GetComponent<arrowScript>();
					arrowScriptInstance.arrowType = "grapple";
					grappling = true;
					numberOfGrapples -= 1;
				}
				
				//Add force to arrow object
				Rigidbody2D arrowRigidBody = arrowInstance.GetComponent<Rigidbody2D>();
				arrowRigidBody.AddForce((mousePositionWorldVector-controllerPosition) * 75,ForceMode2D.Force);

				//Rotate arrow to face position of mouse click
				Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - arrowRigidBody.transform.position;
				diff.Normalize();
				float zRotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
				arrowRigidBody.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
			}
			
			if (numberOfArrows == 0)
			{
				Debug.Log("Out of Arrows!");
			}
			if (numberOfGrapples == 0)
			{
				Debug.Log("Out of Rope!");
			}
			
			//If grapple button is clicked while already grappling
			if (Input.GetKeyDown (ShootGrappleHook) || grappling == true)
			{
				//Remove current grapple
				var objects = GameObject.FindGameObjectsWithTag("Grapple"); //Will need to add tag to arrows
				var objectCount = objects.Length;
				foreach (var obj in objects) {
					// whatever
					Destroy (obj);
				}
			}
		}

		//Debug.Log (arrowList.Count);
	

		if (Input.GetKeyDown (Jump) && _controller.isGrounded) 
		{
			var targetJumpHeight = jumpHeight;
			velocity.y = Mathf.Sqrt(2f * targetJumpHeight * -gravity);
			 

		}

		velocity.y += gravity * Time.deltaTime;

		_controller.move(velocity * Time.deltaTime);
	}

	private void DrawRay( Vector3 start, Vector3 dir, Color color )
	{
		Debug.DrawRay( start, dir, color );
	}

	private void goLeft()
	{
		if (transform.localScale.x > 0f)
			transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	private void goRight()
	{
		if (transform.localScale.x < 0f)
			transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
}


