var prefabGrapple:Transform;
var shootForceX=0;
var shootForceY=0;
var shootForceZ=0;



function Update ()
{



//on spacebar down...
if(Input.GetButtonDown("Fire1"))
{
//...spawn the grappling hook prefab
var InstanceGrapple = Instantiate(prefabGrapple, transform.position, Quaternion.identity);
//...shoot the spawned grappling hook with the forces set in the variables
InstanceGrapple.rigidbody.AddForce(shootForceX,shootForceY,shootForceZ);
}
}
