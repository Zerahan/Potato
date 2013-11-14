using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameSettings;

public class Player : MonoBehaviour {
	/* Handles most of the "player" stuff
	 * 
	 * Should work just by placing it onto an object that also has a Player script attached.
	 * 
	 * // Variables
	 * isHuman				// Is the player a isHuman or a computer?
	 * 
	 * // Functions
	 * IsControlLocked()	// Check if the player's controls are locked
	 * SetControlLocked(bool)	// Locks/unlocks controls
	 * 
	 * To Do:
	 *	The camera should be handled by a seperate script.
	 *	Record keeping should be handled by a seperate script on per-map basis.
	 *		Set best win time to player's best, or 10th place (if no player score on record). Something like "bestWinTime = Records.GetPlayerRecord();"
	 *		Award stars based on completion time. 1 for completing, 2 for under 20 seconds, 3 for under 10
	 *	Replace the "dropAmount" stuff with something that calculates the map's bounds, and resets the player if they manage to escape those bounds
	 *		Possible easter eggs for skilled players?
	 *	Automatically get the player's visible body
	 *	Make player into a prefab that does not collide with other players (potential multiplayer)
	//*/
	
	public bool isHuman;					// No point in displaying graphical stuff to a computer
	public GameObject body;					// Set in inspector to the player's visible body
	
	private UserInput userInput;
	
	private float timer;
	private int dropAmount		= -5;		// If the player gets outside the map, this is how far down they can drop before resetting
	
	//private float debugFloat;
	
	private Vector3 localEulerAngles;
	private	float	cameraZoom;
	private	Vector3	cameraPosition		= Vector3.zero;
	private	Vector3 cameraPositionMin	= new Vector3(0,2,-5);
	private	Vector3 cameraPositionMax	= new Vector3(0,3,-20);
	
	public	List<Equipment> equipment	= new List<Equipment>();
	
	public AnimationClip idleAnimation;
	public AnimationClip walkAnimation;
	public AnimationClip runAnimation;
	public AnimationClip jumpPoseAnimation;
	
	public float walkMaxAnimationSpeed	= 0.75f;
	public float trotMaxAnimationSpeed	= 1.0f;
	public float runMaxAnimationSpeed	= 1.0f;
	public float jumpAnimationSpeed		= 1.15f;
	public float landAnimationSpeed		= 1.0f;
	
	private Animation _animation;
		
	public void Start(){
		userInput		= GetComponent<UserInput>();
		cameraZoom		= 0.25f;
		Camera.main.transform.localPosition	= (cameraPositionMin * (1-cameraZoom)) + (cameraPositionMax * (cameraZoom));
	}

	void Awake ()
	{
		//moveDirection = transform.TransformDirection(Vector3.forward);
		
		_animation = body.GetComponent<Animation>();
				
	}
	
	public void Update () {
		float vel	= 0f;
		if(rigidbody.velocity.x > 0.1 || rigidbody.velocity.x < -0.1){
			vel	= rigidbody.velocity.x;
			if(vel > 2){
				vel = 2;
			}
			if(vel < -2){
				vel = -2;
			}
		}
		localEulerAngles.y	= ((vel/2f)*-90)+180;
		body.transform.localEulerAngles = localEulerAngles;
		
		if( !userInput.IsControlLocked() ){
			if( cameraPosition	!= Vector3.zero ){
				cameraPosition	= Vector3.zero;
				Camera.main.transform.localPosition	= (cameraPositionMin * (1-cameraZoom)) + (cameraPositionMax * (cameraZoom));
			}
			if( Input.GetAxis("Mouse ScrollWheel") != 0 ){
				cameraZoom	-= 0.5f * (Input.GetAxis("Mouse ScrollWheel"));
				if( cameraZoom < 0 ){
					cameraZoom	= 0;
				}else if( cameraZoom > 1 ){
					cameraZoom	= 1;
				}
				Camera.main.transform.localPosition	= (cameraPositionMin * (1-cameraZoom)) + (cameraPositionMax * (cameraZoom));
			}
		}else{
			if( cameraPosition == Vector3.zero ){
				cameraPosition	= Camera.main.transform.position;
			}
			Camera.main.transform.position	= cameraPosition;
		}
		
		// Reset if the player dropped too low.
		if(transform.position.y < dropAmount){
			transform.position = new Vector3(0,0,0);
			rigidbody.velocity = new Vector3(0,0,0);
		}
		
		if(_animation) {
			if(rigidbody.velocity.magnitude < 0.1){
				rigidbody.velocity	= Vector3.zero;
				_animation.CrossFade(idleAnimation.name);
			}else{
				if( !userInput.IsOnGround() ){
					//debugFloat	= rigidbody.velocity.y;
					if(rigidbody.velocity.y > 0) {
						_animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
						_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
						_animation.CrossFade(jumpPoseAnimation.name);
					} else {
						_animation[jumpPoseAnimation.name].speed = -landAnimationSpeed;
						_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
						_animation.CrossFade(jumpPoseAnimation.name);				
					}
				}else{
					if(rigidbody.velocity.magnitude > 4){
						_animation[runAnimation.name].speed		= Mathf.Clamp(rigidbody.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
						_animation.CrossFade(runAnimation.name);
					}else{
						_animation[walkAnimation.name].speed	= Mathf.Clamp(rigidbody.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
						_animation.CrossFade(walkAnimation.name);
					}
				}
			}
		}
	}
}
