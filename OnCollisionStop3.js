
function OnCollisionEnter(collision : Collision) {

rigidbody.isKinematic = true;



bob = GameObject.Find("Shooter");
//this makes him jump
bob.rigidbody.AddForce(0,250,250);



//Give time for addForce on the shooter to work it's magic
yield WaitForSeconds (1.5);
/*now we destroy this object, in this case the grappling hook as it has
been enough time for the shooter to move */
Destroy (gameObject);

}


function Update(){
bob = GameObject.Find("Shooter");
//If they are at the same Y height or less as the shooter, they get killed
if(this.transform.position.y<=bob.transform.position.y){
Destroy (gameObject);
}
//If they are greater than 10 units over the height of shooter, they get killed
if(this.transform.position.y>bob.transform.position.y+10){
Destroy (gameObject);
}
}
