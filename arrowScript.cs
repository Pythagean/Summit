using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class arrowScript : MonoBehaviour {
	
	//public float arrowSpeed = 5f;
	//public float arrowLifetime = 4.0f;
	//public Vector3 arrowDirection = new Vector3(0,0);

	public float arrowCollision = 1/2;



	public LayerMask platformMask = 0;

	//Rigidbody2D arrowRigidBody = getComponent<Rigidbody2D>();
	public Vector3 velocity;
	private RaycastHit2D _raycast;
	//public 

	void Update () 
	{
		velocity = GetComponent<Rigidbody2D>().velocity;
		//transform.Translate(arrowDirection * arrowSpeed * Time.deltaTime);
		//Debug.Log ("TEST");
	
		//Debug.Log ("velocity: " + velocity);

		//Debug.Log ("position: " + transform.position + ", velocity: " + velocity);

		_raycast = Physics2D.Raycast(transform.position,velocity, arrowCollision, platformMask);
		//Debug.DrawRay (transform.position, transform.forward 10, Color.red,10);
		//Debug.DrawLine (transform.position, transform.forward, Color.red, 2, false);
		//Debug.Log ("transform.position: " + transform.position + "transform.forward: " + transform.forward);
		Debug.DrawRay (transform.position, velocity / 20, Color.red, 2);


		if (_raycast) 
		{
			//Debug.Log("HIT");
			velocity = Vector3.zero;
			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			GetComponent<Rigidbody2D>().gravityScale = 0;
		}
		//if (_raycastHitGrapple)
		//{
		//	Debug.Log("Hit");
		//	arrowRigidBody.velocity = Vector3.zero;
		//}
		//Debug.Log ("Test");
		//Destroy(this, arrowLifetime);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		//all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
		//if (col.gameObject.tag == "Enemy") {
			//Destroy (col.gameObject);
			//add an explosion or something
			//destroy the projectile that just caused the trigger collision
			//Destroy (gameObject);
		Debug.Log ("HIT2");
		//}
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

	}

	public void Initialize()
	{

	}

	public void setDirection()
	{
		
	}


}