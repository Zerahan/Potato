using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour {
	/* Handles all inputs from users
	 * 
	 * To Do:
	 *  Make the light and particles more generic, and autoloaded from the player's options instead of having to set through inspector.
	//*/
	
	public		ParticleSystem particle			;	// The particles that are spawned when THIS player collides with something
	public		float		moveForceFactor		= 10f;
	public		float		walkSpeed			= 10f;	
	private		float		frictionForce		= 200f;	
	
	private		Player		player				;
	private		TimedRace	gamemode			;
	private		bool		isJumping			= false;
	private		bool		onGround			= true;
	private		bool		isAttachedToWall	= false;
	private		float		lastParticleSpawn	;
	private		bool		controlsLocked		= false;
	private		float		inputX				;
	private		float		inputY				;
	private		float		inputSmoothX		;
	private		float		inputSmoothY		;
	private		Vector3		moveForce			;
	private		Vector3		velocity			;
	private		float		velocityMag			;
	private		float		frictionForceVal	;
	
	private		float		maxVelocity			= 50f;
	private		float		controlMultiplier	= 0.0f;
	//private		float		yVelocity		;
	
	private		Vector3		center;
	public		Vector3		Center				{ get{return center + transform.position;} }
	
	// Engine
	
	public void Start () {
		player		= transform.root.GetComponent<Player>();
		gamemode	= transform.root.GetComponent<TimedRace>();
		center		= ((CapsuleCollider)collider).center;
		center.z	= 0;
		rigidbody.velocity = Vector3.zero;
	}
	
	public void Update(){
	}
	
	public void FixedUpdate () {
		if(player.isHuman && !controlsLocked){
			DoMovePlayer();
		}
	}
	
	// Spawns particles and light at the point of collision, and gives the player the ability to jump again.
	public void OnCollisionEnter(Collision collision){
		if(collision.gameObject){
			//if (collision.gameObject.GetComponent<BreakableObject>()) {
			//	onGround = false;
			//	isJumping = false;
			//}else{
				onGround		= true;
				if( collision.gameObject.GetComponent<BreakableObject>() && collision.gameObject.GetComponent<BreakableObject>().IsDestroyed ){
					onGround	= false;
				}
				if(onGround){
					foreach(ContactPoint contact in collision.contacts){
						//float angle	=  Mathf.Tan(contact.normal.y/contact.normal.x);
						if( contact.normal.y > 0f || contact.normal.x > 0.5f || contact.normal.x < -0.5f ){
							isJumping = false;
							Debug.DrawRay(contact.point, contact.normal * 5, Color.red,1,true);
							if( particle && lastParticleSpawn < Time.time ){
								lastParticleSpawn = Time.time + 0.25f;
								ParticleSystem p = (ParticleSystem)Instantiate(particle,contact.point,Quaternion.identity);
								p.transform.eulerAngles = contact.normal;
								p.startSpeed = 4;
							}
						}
					}
				}
			//}
		}
	}
	
	public void OnCollisionStay(Collision collision){
		//onGround	= true;
		if( isAttachedToWall ){
			if( isJumping ){
				isAttachedToWall		= false;
			}else{
				foreach(ContactPoint contact in collision.contacts){
					float angle	=  Mathf.Tan(contact.normal.y/contact.normal.x);
					if( angle > Mathf.PI/6 || angle < 0-Mathf.PI/6 ){
						isAttachedToWall		= false;
					}
				}
			}
		}
	}
	
	public void OnCollisionExit(){
		onGround	= false;
	}	
	
	// Custom
	
	public float GetControlMultiplier(){ return controlMultiplier; }
	public void SetControlMultiplier(float mult){
		controlMultiplier	= mult;
	}
	
	public Vector3 GetMousePosition(){
		Vector3 mp	= Input.mousePosition;
		mp.z		= 0-Camera.main.transform.position.z;
		return Camera.main.ScreenToWorldPoint(mp);
	}
	
	public bool IsControlLocked(){ return controlsLocked; }
	public void SetControlLocked(bool shouldLock){
		controlsLocked = shouldLock;
	}
	
	public bool IsGrappled(){
		return GetComponent<Grapple>().IsActive;
	}
	public bool IsOnGround(){
		return onGround;
	}
	public bool IsJumping(){
		return isJumping;
	}
	
	public void DoMovePlayer(){
		inputX				= Input.GetAxis("Horizontal");
		inputY				= Input.GetAxis("Vertical");
		inputSmoothX		= Input.GetAxis("Horizontal");
		inputSmoothY		= Input.GetAxis("Vertical");
		bool jumpbutton		= Input.GetButton("Jump");
		bool isGrappled		= GetComponentInChildren<Grapple>().IsActive;
		
		moveForce = new Vector3(0,0,0);
		velocity = rigidbody.velocity;
		
		
		if(onGround && !isJumping){
			velocityMag = rigidbody.velocity.magnitude;
			if (velocityMag < 1 && inputX == 0){
				rigidbody.velocity *= 0f;
				moveForce *= 0f;
				return;
			}
			
			/*
			if (rigidbody.velocity.magnitude < walkSpeed) {
				if (inputX == 0){
					moveForce.x = frictionForce * rigidbody.velocity.x * -2 / walkSpeed;
				}else{
					//rigidbody.velocity = new Vector3(inputX * walkSpeed,0,0);
					moveForce.x = frictionForce * inputX;
				}
			}
			//*/
			
			moveForce.x	= inputSmoothX * moveForceFactor * 2;
		}else if(isGrappled){
			moveForce.x	= inputSmoothX * moveForceFactor;
			moveForce.y	= inputSmoothY * moveForceFactor;			
		}else{
			moveForce.x	= inputSmoothX * moveForceFactor/2;
			moveForce.y	= inputSmoothY * moveForceFactor/2;
		}
		
		if( onGround && !isJumping && (jumpbutton || inputSmoothY > 0) ){
			isJumping = true;
			rigidbody.velocity	= rigidbody.velocity + new Vector3(0,11,0);
		}
		
		// If the player moves, start the timer
		if(!gamemode.HasStarted() && (isJumping || moveForce.x != 0 || Input.GetButton("Action1"))){
			gamemode.StartRace();
		}
		
		/*
		if (onGround && !isJumping) {
			velocityMag = rigidbody.velocity.magnitude;
			
			if (rigidbody.velocity.x != inputSmoothX * walkVelocity) {
				moveForce.x = 
				rigidbody.velocity = new Vector3(inputSmoothX * walkVelocity * 1f, 0, 0);
			}
			
			if (inputX == 0) {
				frictionForceVal = -0.5f * frictionForce / rigidbody.velocity.x;
			}else if (inputX * rigidbody.velocity.x < 0) {
				// decelerate
				frictionForceVal = inputX * frictionForce;
			}else if (velocityMag < walkVelocity){
				// accelerate
				frictionForceVal = inputX * frictionForce / Mathf.Max(1, velocityMag);
			}else{
				frictionForceVal = 0;
			}
			moveForce.x = moveForce.x + frictionForceVal;
		}
		*/
		rigidbody.AddForce(moveForce);
		//yVelocity	= moveTarget.y;
		
		if(rigidbody.velocity.magnitude > maxVelocity){
			rigidbody.velocity	= rigidbody.velocity.normalized * maxVelocity;
		}
	}
}
