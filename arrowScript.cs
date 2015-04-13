using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class arrowScript : MonoBehaviour {
	
	//public float arrowSpeed = 5f;
	//public float arrowLifetime = 4.0f;
	//public Vector3 arrowDirection = new Vector3(0,0);

	public float arrowCollision = 1/2;
	public LayerMask platformMask = 0;
	
	public arrowType = "normal";
	private LineRenderer line;

	//Rigidbody2D arrowRigidBody = getComponent<Rigidbody2D>();
	public Vector3 velocity;
	private RaycastHit2D _raycast;
	//public 

	void Update () 
	{
		if (arrowType == "grapple")
		{
			player = GameObject.Find("Player");
			//set starting point of line to this object, in this case the grappling hook prefab
			line.SetPosition(0, transform.position);
			//set the ending point of the line to the player
			line.SetPosition(1, player.transform.position);
		}
		
		//Detect Collisions
		velocity = GetComponent<Rigidbody2D>().velocity;
		_raycast = Physics2D.Raycast(transform.position,velocity, arrowCollision, platformMask);
		Debug.DrawRay (transform.position, velocity / 20, Color.red, 2);

		//If collision detected
		if (_raycast) 
		{
			//Stops arrow and sets gravity to 0
			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			GetComponent<Rigidbody2D>().gravityScale = 0;
		}
	}

	void Create()
	{

	}
	
	void OnBecameInvisible () {
		this.gameObject.SetActive(false);
	}

	void Awake ()
	{
		//Debug.Log ("Arrow created");
		line = this.gameObject.AddComponent(LineRenderer);
		line.SetWidth(startWidth, endWidth);
		line.SetVertexCount(2);
		line.material.color = Color.red;
		//we need to see the line... 
		line.GetComponent.<Renderer>().enabled = true;
		
		
		
	}

	public void Initialize()
	{

	}

	public void setDirection()
	{
		
	}


}