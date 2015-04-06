
function OnCollisionEnter(collision : Collision) {

rigidbody.isKinematic = true;



bob = GameObject.Find("Shooter");


bob.transform.position = Vector3.Lerp(bob.transform.position, this.transform.position + Vector3(0,1,0), 1);

Destroy (gameObject);

}
