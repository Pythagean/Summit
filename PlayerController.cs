using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public KeyCode Left = KeyCode.A;
	public KeyCode Right = KeyCode.D;
	public KeyCode Jump = KeyCode.W;
	public KeyCode Grapple = KeyCode.Mouse0;


	public float gravity = -15f;
	public float runSpeed = 8f;
	public float decaySpeed = 0.5f;
	public float jumpHeight = 1.8f;

	private CharacterController2D _controller;
	private Animator _animator;

	private RaycastHit2D _raycastHitGrapple;

	List<GameObject> arrowList;
	public GameObject ArrowPrefab;
	//public Rigidbody2D ArrowPrefab;


	void Awake()
	{
		_controller = GetComponent<CharacterController2D> ();
		_animator = GetComponent<Animator> ();
		arrowList = new List<GameObject>();
	}

	void Update()
	{
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

		//Debug.Log ("Angle between mouse and controller: " + Vector3.Angle (new Vector3(_controller.transform.position.x,_controller.transform.position.y), Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y))));

		if (Input.GetKeyDown (Grapple))
		{
			//camera = Camera.main;
			//Debug.Log("Mouse Click: x=" + Input.mousePosition.x.ToString() + ", y=" + Input.mousePosition.y.ToString());
			Vector3 mousePositionVector = new Vector3(Input.mousePosition.x,Input.mousePosition.y);
			var mousePositionWorldVector = Camera.main.ScreenToWorldPoint(mousePositionVector);
			Vector3 controllerPosition = new Vector3(_controller.transform.position.x,_controller.transform.position.y);
			Debug.DrawRay(controllerPosition,(mousePositionWorldVector-controllerPosition),Color.green,1);
			//Debug.DrawLine(new Vector3(_controller.transform.position.x,_controller.transform.position.y),mousePositionVector,Color.red,2);

			//Rigidbody2D arrowInstance = Instantiate(

			//Rigidbody2D arrowInstance = Instantiate(ArrowPrefab, controllerPosition, Quaternion.identity) as Rigidbody2D;
			//arrowInstance.velocity = transform.TransformDirection(new Vector3(100,100));
			GameObject arrowInstance = (GameObject)Instantiate(ArrowPrefab, controllerPosition, Quaternion.identity);
			Rigidbody2D arrowRigidBody = arrowInstance.GetComponent<Rigidbody2D>();
			arrowRigidBody.AddForce((mousePositionWorldVector-controllerPosition) * 75);

			Quaternion targetRotation = Quaternion.LookRotation(mousePositionWorldVector - arrowRigidBody.transform.position);
			
			//Attempt #1
			/* float targetRotationFloat = Vector3.Angle(controllerPosition,mousePositionWorldVector);
			if (mousePositionWorldVector.x < controllerPosition.x)
			{
				targetRotationFloat = 360 - targetRotationFloat;
			}
			Debug.Log(targetRotationFloat.ToString());
			arrowRigidBody.rotation = targetRotationFloat; */
			

			//Attempt #2
			//arrowRigidBody.transform.rotation.eulerAngles = Vector3.Angle(controllerPosition,mousePositionWorldVector);
			//arrowRigidBody.transform.rotation.eulerAngles = 
			float zRotation = Vector3.Angle(controllerPosition,mousePositionWorldVector);

			if (mousePositionWorldVector.x < controllerPosition.x)
			{
				Debug.Log("mouse: " + mousePositionWorldVector.x + " < controller: " + controllerPosition.x);
				zRotation = 360 - zRotation;
			}
			else
			{
				Debug.Log("mouse: " + mousePositionWorldVector.x + " > controller: " + controllerPosition.x);
			}

			//float zRotation = Vector3.Angle(controllerPosition,mousePositionWorldVector);
			arrowRigidBody.transform.eulerAngles = new Vector3(0,0,zRotation);

			Debug.Log(arrowRigidBody.transform.rotation.eulerAngles.z);
			//Debug.Log("controllerPosition: " + controllerPosition);
			//Debug.Log("mousePositionWorldVector: " + mousePositionWorldVector);
			//Attempt #3
			//arrowRigidBody.transform.LookAt(mousePositionWorldVector);
			

			var distanceBetweenMouseController = Vector3.Distance(mousePositionWorldVector,controllerPosition);
			_raycastHitGrapple = Physics2D.Raycast(controllerPosition,(mousePositionWorldVector-controllerPosition),distanceBetweenMouseController,_controller.platformMask);

			if (_raycastHitGrapple)
			{
				Debug.Log("Hit");
			}

			//var ray = new Vector2( _controller.transform.position.x, _controller.transform.position.y);
			//DrawRay( ray, rayDirection * rayDistance, Color.red );
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


