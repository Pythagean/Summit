// BulletScript.cs
using UnityEngine;
using System.Collections;

public class arrowScript : MonoBehaviour {
	
	public float arrowSpeed = 5f;
	public float arrowLifetime = 4.0f;
	public Vector3 arrowDirection = new Vector3(0,0);

	void Update () 
	{
		transform.Translate(arrowDirection * arrowSpeed * Time.deltaTime);
		//Debug.Log ("Test");
		//Destroy(this, arrowLifetime);
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