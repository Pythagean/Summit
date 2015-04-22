using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public KeyCode Left = KeyCode.A;
	public KeyCode Right = KeyCode.D;
	public KeyCode Jump = KeyCode.W;
	public KeyCode Down = KeyCode.S;
	public KeyCode ShootArrow = KeyCode.Mouse0;
	public KeyCode ShootGrappleHook = KeyCode.Mouse1;
	public KeyCode GrapplePull = KeyCode.Q;
	

	public float gravity = -15f;
	public float runSpeed = 8f;
	public float decaySpeed = 0.5f;
	public float jumpHeight = 1.8f;
	
	public int numberOfGrapples = 5;
	public int numberOfArrows = 20;

	public int grapplePullSpeed = 15;

	private CharacterController2D _controller;
	private Animator _animator;

	private RaycastHit2D _raycastHitGrapple;
	
	private bool grappling = false;
	private bool grapplingHold = false;
	private bool holdingLedge = false;
	
	public LayerMask platformMask = 0;

	private RaycastHit2D _raycastTop;
	private RaycastHit2D _raycastBottom;
	private Vector2 _topRight;
	private Vector2 _topLeft;
	private Vector2 _bottomRight;
	private Vector2 _bottomLeft;

	List<GameObject> arrowList;
	public GameObject ArrowPrefab;

	private float height = 0.5f;
	private float width = 0.5f;

	void Awake()
	{
		_controller = GetComponent<CharacterController2D> ();
		_animator = GetComponent<Animator> ();
		arrowList = new List<GameObject>();
	}

	void Update()
	{
		_topRight = new Vector2(transform.position.x+width, transform.position.y+height);
		_topLeft = new Vector2(transform.position.x-width, transform.position.y+height);
		_bottomRight = new Vector2(transform.position.x+width, transform.position.y-height);
		_bottomLeft = new Vector2(transform.position.x-width, transform.position.y-height);
		
		
		//Debug.Log(name.ToString());
		//Grabs current velocity of player
		var velocity = _controller.velocity;

		//Debug.Log ("X Velocity: " + _controller.velocity.x);
		//Debug.Log ("Y Velocity: " + _controller.velocity.y);

		if (_controller.isGrounded)
				velocity.y = 0;

		//Horizontal Input
		if (Input.GetKey (Right) && holdingLedge == false) 
		{
			velocity.x = runSpeed;
			//goRight();
		}
		else if( Input.GetKey(Left) && holdingLedge == false)
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
		
		//Pulls player towards the grapple
		if (Input.GetKey (GrapplePull)) 
		{
			var objects = GameObject.FindGameObjectsWithTag("Grapple");
			var objectCount = objects.Length;
			foreach (var obj in objects) {
				transform.position = Vector3.MoveTowards(transform.position,obj.transform.position,(grapplePullSpeed * Time.deltaTime));
			}
		}
	
		if (Input.GetKeyDown (ShootArrow) || Input.GetKeyDown(ShootGrappleHook))
		{
			//If grapple button is clicked while already grappling
			if (Input.GetKeyDown (ShootGrappleHook) && grappling == true)
			{
				//Remove current grapple
				var objects = GameObject.FindGameObjectsWithTag("Grapple");
				var objectCount = objects.Length;
				foreach (var obj in objects) {
					Destroy (obj);
				}
				grappling = false;
			}
			else if (grappling == false && numberOfArrows > 0)
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
					arrowScriptInstance.tag = "Grapple";
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
			

		}
		
		if (holdingLedge)
		{
			_controller.isGrounded = true;
			velocity = Vector3.zero;
			if (Input.GetKey (Jump))
			{
				//Debug.Log("Jumping up from Ledge");
				var targetJumpHeight = jumpHeight;
				velocity.y = Mathf.Sqrt(2f * targetJumpHeight * -gravity);
				
				holdingLedge = false;
			}
			else if (Input.GetKey (Down))
			{
				//Debug.Log("Dropping down from Ledge");
				holdingLedge = false;
			}
		}
		
		//Check for holding ledge
		if (velocity.x > 0 && holdingLedge == false)
		{
			_raycastTop = Physics2D.Raycast(_topRight, new Vector2(velocity.x,0), 0.1f, platformMask);
			//_raycastTop = Physics2D.Raycast(_controller.topRight, velocity, 0.5f, platformMask);
			Debug.DrawRay (_topRight, new Vector2(velocity.x,0), Color.blue, 0.1f);
			
			_raycastBottom = Physics2D.Raycast(_bottomRight, new Vector2(velocity.x,0), 0.1f, platformMask);
			Debug.DrawRay (_bottomRight, new Vector2(velocity.x,0), Color.blue, 0.1f);
			
			//Debug.Log(velocity);
			if (!_raycastTop && _raycastBottom)
			{
				holdingLedge = true;
			}

		}
		else if (velocity.x < 0 && holdingLedge == false)
		{
			_raycastTop = Physics2D.Raycast(_topLeft, new Vector2(velocity.x,0), 1f, platformMask);
			Debug.DrawRay (_topLeft, new Vector2(velocity.x,0), Color.blue, 0.1f);
			
			_raycastBottom = Physics2D.Raycast(_bottomLeft, new Vector2(velocity.x,0), 1f, platformMask);
			Debug.DrawRay (_bottomLeft, new Vector2(velocity.x,0), Color.blue, 0.1f);

			if (!_raycastTop && _raycastBottom)
			{
				holdingLedge = true;
			}
		}

		//Debug.Log (arrowList.Count);
	
		//Debug.Log (_controller.isGrounded.ToString ());
		if (Input.GetKeyDown (Jump) && _controller.isGrounded) 
		{
			//Debug.Log("Jump");
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


