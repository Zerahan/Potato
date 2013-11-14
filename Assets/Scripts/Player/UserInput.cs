using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour {
	/* Handles all inputs from users
	 * 
	 * To Do:
	 *  Make the light and particles more generic, and autoloaded from the player's options instead of having to set through inspector.
	//*/
	
	public		ParticleSystem particle			;	// The particles that are spawned when THIS player collides with something
	
	private		Player		player				;
	private		TimedRace	gamemode			;
	private		bool		isJumping			= false;
	private		bool		onGround			= true;
	private		bool		isAttachedToWall	= false;
	private		float		moveSpeed			= 10f;
	private		float		lastParticleSpawn	;
	private		bool		controlsLocked		= false;
	private		float		horizontal			;
	private		float		vertical			;
	
	private		float		maxVelocity			= 50f;
	private		float		controlMultiplier	= 0.0f;
	//private		float		yVelocity			;
	
	// Engine
	
	public void Start () {
		player		= transform.root.GetComponent<Player>();
		gamemode	= transform.root.GetComponent<TimedRace>();
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
			onGround		= true;
			if( collision.gameObject.GetComponent<BreakableObject>() && collision.gameObject.GetComponent<BreakableObject>().IsDestroyed() ){
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
		horizontal	= Input.GetAxis("Horizontal");
		vertical	= Input.GetAxis("Vertical");
		bool jumpbutton		= Input.GetButton("Jump");
		bool isGrappled		= GetComponentInChildren<Grapple>().IsActive;
		
		Vector3 moveTarget	= new Vector3(0,0,0);
		
		if(onGround){
			moveTarget.x	= horizontal * moveSpeed;
		}else if(isGrappled){
			moveTarget.x	= horizontal * moveSpeed/2;
			moveTarget.y	= vertical * moveSpeed/2;			
		}else{
			moveTarget.x	= horizontal * moveSpeed/2;
			moveTarget.y	= vertical * moveSpeed/2;
		}
		
		if( onGround && !isJumping && (jumpbutton || Input.GetAxis("Vertical") > 0) ){
			isJumping = true;
			rigidbody.velocity	= rigidbody.velocity + new Vector3(0,11,0);
		}
		/*
		if(Input.GetAxis("Vertical") != 0){
			int mult = 0;
			if(Input.GetAxis("Vertical") < 0){
				mult = -1;
			}else{
				mult = 1;
			}
			if(isGrappled){
				moveTarget.y	= (mult * 2) + (Physics.gravity.y * mult * -1);
			}else if( controlMultiplier > 0 ){
				moveTarget.y	= (mult * 2) + (Physics.gravity.y * mult * -1);
				controlMultiplier -= Time.deltaTime/2f;
			}else{
				moveTarget.y	= 0;
			}
		}
		//*/
		//rigidbody.useGravity = useGravity;
		rigidbody.AddForce(moveTarget);
		//yVelocity	= moveTarget.y;
		
		if(rigidbody.velocity.magnitude > maxVelocity){
			rigidbody.velocity	= rigidbody.velocity.normalized * maxVelocity;
		}
		
		// If the player moves, start the timer
		if(!gamemode.HasStarted() && (isJumping || moveTarget.x != 0 || Input.GetButton("Grapple"))){
			gamemode.StartRace();
		}
	}
}
