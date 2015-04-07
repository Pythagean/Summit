// BulletScript.cs
using UnityEngine;
using System.Collections;

public class arrowScript : MonoBehaviour {
	
	public float arrowSpeed = 5f;
	public float arrowLifetime = 4.0f;

	void Update () 
	{
		transform.Translate(Vector3.right * arrowSpeed * Time.deltaTime);
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
		Debug.Log ("Arrow created");

	}

	public void Initialize()
	{

	}

	public void setDirection()
	{
		
	}


}