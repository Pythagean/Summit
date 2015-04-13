using UnityEngine;
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

	private CharacterController2D _controller;
	private Animator _animator;

	private RaycastHit2D _raycastHitGrapple;

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

			Vector3 mousePositionVector = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.nearClipPlane);
			var mousePositionWorldVector = Camera.main.ScreenToWorldPoint(mousePositionVector);
			Vector3 controllerPosition = new Vector3(_controller.transform.position.x,_controller.transform.position.y);
			Debug.DrawRay(controllerPosition,(mousePositionWorldVector-controllerPosition),Color.green,1);

			//Create Arrow Object
			GameObject arrowInstance = (GameObject)Instantiate(ArrowPrefab, controllerPosition, Quaternion.identity);
			
			if (Input.GetKeyDown (ShootGrappleHook))
			{
				//arrowInstance.GetComponent<arr>() = "grapple";
				arrowScript arrowScript = arrowInstance.GetComponent<arrowScript>();
				arrowScript.arrowType = "grapple";
			}
			
			Rigidbody2D arrowRigidBody = arrowInstance.GetComponent<Rigidbody2D>();
			arrowRigidBody.AddForce((mousePositionWorldVector-controllerPosition) * 75,ForceMode2D.Force);

			//Rotate arrow to face position of mouse click
			Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - arrowRigidBody.transform.position;
			diff.Normalize();
			float zRotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			arrowRigidBody.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
			
			//Calculating if arrow hits an obstacle
			/* var distanceBetweenMouseController = Vector3.Distance(mousePositionWorldVector,controllerPosition);
			_raycastHitGrapple = Physics2D.Raycast(controllerPosition,(mousePositionWorldVector-controllerPosition),distanceBetweenMouseController,_controller.platformMask);
			if (_raycastHitGrapple)
			{
				arrowRigidBody.velocity = Vector3.zero;
			} */

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


