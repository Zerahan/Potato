using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour {
	/* Handles all inputs from users
	 * 
	 * To Do:
	 *  Make the light and particles more generic, and autoloaded from the player's options instead of having to set through inspector.
	//*/
	public Light light;				// The light object that is spawned when THIS player collides with something
	public ParticleSystem particle;	// The particles that are spawned when THIS player collides with something
	
	private Player player;
	private TimedRace gamemode;
	private bool isJumping			= false;
	private bool onGround			= true;
	private bool isAttachedToWall	= false;
	private float moveSpeed			= 3.0f;
	private float lastLight;
	private bool controlsLocked		= false;
	private float horizontal;
	private float vertical;
	
	private float controlMultiplier	= 0.00f;
	private float yVelocity;
	
	public void Start () {
		player		= transform.root.GetComponent<Player>();
		gamemode	= transform.root.GetComponent<TimedRace>();
	}
	
	public void Update(){
	}
	
	public void FixedUpdate () {
		if(player.human && !controlsLocked){
			MovePlayer();
		}
	}
	
	public bool AreControlsLocked(){ return controlsLocked; }
	public void LockControls(bool shouldLock){
		controlsLocked = shouldLock;
	}
	
	public bool IsOnGround(){
		return onGround;
	}
	public bool IsGrappled(){
		return GetComponent<Grapple>().IsActive();
	}
	
	public void MovePlayer(){
		horizontal	= Input.GetAxis("Horizontal");
		vertical	= Input.GetAxis("Vertical");
		bool jumpbutton		= Input.GetButton("Jump");
		bool isGrappled		= GetComponent<Grapple>().IsActive();
		bool useGravity		= true;
		
		Vector3 moveTarget	= new Vector3(0,0,0);
		
		if(onGround){
			moveTarget.x	= Input.GetAxis("Horizontal") * 10;
		}else if(isGrappled){
			moveTarget.x	= Input.GetAxis("Horizontal") * 10;
			moveTarget.y	= Input.GetAxis("Vertical") * 10;			
		}else{
			moveTarget.x	= Input.GetAxis("Horizontal") * 5;
			moveTarget.y	= Input.GetAxis("Vertical") * 5;
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
		yVelocity	= moveTarget.y;
		
		// If the player moves, start the timer
		if(!gamemode.HasStarted() && (isJumping || moveTarget.x != 0 || Input.GetButton("Fire1"))){
			gamemode.StartRace();
		}
	}
	
	public float GetControlMultiplier(){ return controlMultiplier; }
	public void SetControlMultiplier(float mult){
		controlMultiplier	= mult;
	}
	
	// Spawns particles and light at the point of collision, and gives the player the ability to jump again.
	public void OnCollisionEnter(Collision collision){
		onGround	= true;
		foreach(ContactPoint contact in collision.contacts){
			float angle	=  Mathf.Tan(contact.normal.y/contact.normal.x);
			if( contact.normal.y > 0f || contact.normal.x > 0.5f || contact.normal.x < -0.5f ){
				isJumping = false;
				Debug.DrawRay(contact.point, contact.normal * 5, Color.red,1,true);
				if(light && particle && lastLight < Time.time){
					lastLight = Time.time + 0.25f;
					Instantiate(light,new Vector3(contact.point.x,contact.point.y,-2),Quaternion.identity);
					ParticleSystem p = (ParticleSystem)Instantiate(particle,contact.point,Quaternion.identity);
					p.transform.eulerAngles = contact.normal;
					p.startSpeed = 4;
				}
			}
		}
	}
	
	public void OnCollisionStay(Collision collision){
		//onGround	= true;
		if( isAttachedToWall ){
			if( isJumping ){
				isAttachedToWall		= false;
				rigidbody.useGravity	= true;
			}else{
				foreach(ContactPoint contact in collision.contacts){
					float angle	=  Mathf.Tan(contact.normal.y/contact.normal.x);
					if( angle > Mathf.PI/6 || angle < 0-Mathf.PI/6 ){
						isAttachedToWall		= false;
						rigidbody.useGravity	= true;
					}
				}
			}
		}
	}
	
	public void OnCollisionExit(){
		onGround	= false;
	}
}
