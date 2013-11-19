using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameSettings;

public class PlayerController : MonoBehaviour {
	// Miscellaneous
	public	bool	isHuman				= false;
	private	Vector3	spawnPoint			= Vector3.zero;
	public	float	minZRespawn			= -50;
	private	bool	isControlLocked		= false;
	public	bool	IsControlLocked		{ get{return isControlLocked;} set{isControlLocked = value;Debug.Log("Controls locked outside of PlayerController!");} }
	
	// Collision particles
	public	ParticleSystem	particle			;
	private	float			lastParticleSpawn	;
	private	bool			shouldSpawnParticles;
	
	// Collision Information
	private int		nextCollisionInfo	= 0;
	private int		lastCollisionInfo	= 0;
	private bool	collisionChanged	= true;
	private	Vector3	contactPoint		;
	private	Vector3	contactNormal		;
	
	private	Vector3	wallNormal			;
	private	Vector3	floorSlope			;
	
	public	enum	CollisionInfo		{
		None	= 0		,
		Above	= 1 << 0,	// 1
		Below	= 1 << 1,	// 2
		Slope	= 1 << 2,	// 4
		Wall	= 1 << 3,	// 8
	}
	
	// Movement
	private	Vector3		velocity		;
	private float		nextJump		;
	private	bool		canJump			;
	public	bool		CanJump			{ get{return canJump;} }
	private	float		moveSpeed		= 10f;
	
	private	bool		isJumping		;
	private	float		inputX			;
	private	float		inputY			;
	private	float		inputSmoothX	;
	private	float		inputSmoothY	;
	
	private	bool		controlsLocked	= false;
	public	bool		ControlsLocked	{ get{return controlsLocked;} set{controlsLocked = value;Debug.Log("Controls locked outside of UserInput!");} }
	
	private	Vector3		center;
	public	Vector3		Center			{ get{return center + transform.position;} }
	
	// Camera
	public	Camera		camera				;
	private	float		cameraZoom			= 0.45f;
	private	Vector3		cameraPosition		= Vector3.zero;
	private	Vector3		cameraPositionMin	= new Vector3(0,2,-5);
	private	Vector3		cameraPositionMax	= new Vector3(0,3,-20);
	
	// Animation
	private	Vector3		localEulerAngles	;
	public	Transform	body				;
	
	private	Animation	_animation			;
	
	public	AnimationClip	idleAnimation	;
	public	AnimationClip	walkAnimation	;
	public	AnimationClip	runAnimation	;
	public	AnimationClip	jumpPoseAnimation;
	
	private	float	walkMaxAnimationSpeed	= 0.75f;
	private	float	trotMaxAnimationSpeed	= 1.00f;
	private	float	runMaxAnimationSpeed	= 1.00f;
	private	float	jumpAnimationSpeed		= 1.15f;
	private	float	landAnimationSpeed		= 1.00f;
	
	// Equipment
	public	List<GameObject> equipment;
	
	void Awake(){
		//moveDirection = transform.TransformDirection(Vector3.forward);
		_animation	= GetComponentInChildren<Animation>();
	}
	
	void Start(){
		center		= ((CapsuleCollider)transform.root.GetComponent<CapsuleCollider>()).center;
		center.z	= 0;
		rigidbody.velocity = Vector3.zero;
		camera.transform.localPosition	= (cameraPositionMin * (1-cameraZoom)) + (cameraPositionMax * (cameraZoom));
	}
	
	void Update(){
		// Respawn if the player dropped too low.
		if(transform.position.y < minZRespawn){
			transform.position = spawnPoint;
			rigidbody.velocity = Vector3.zero;
		}
		
		// Camera
		if( !IsControlLocked ){
			if( cameraPosition	!= Vector3.zero ){
				cameraPosition	= Vector3.zero;
				camera.transform.localPosition	= (cameraPositionMin * (1-cameraZoom)) + (cameraPositionMax * (cameraZoom));
			}
			if( Input.GetAxis("Mouse ScrollWheel") != 0 ){
				cameraZoom	-= 0.5f * (Input.GetAxis("Mouse ScrollWheel"));
				if( cameraZoom < 0 ){
					cameraZoom	= 0;
				}else if( cameraZoom > 1 ){
					cameraZoom	= 1;
				}
				camera.transform.localPosition	= (cameraPositionMin * (1-cameraZoom)) + (cameraPositionMax * (cameraZoom));
			}
		}else{
			if( cameraPosition == Vector3.zero ){
				cameraPosition	= camera.transform.position;
			}
			camera.transform.position	= cameraPosition;
		}
		
		// Animation
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
		
		if(_animation) {
			if(rigidbody.velocity.magnitude < 0.1){
				rigidbody.velocity	= Vector3.zero;
				_animation.CrossFade(idleAnimation.name);
			}else{
				if( !IsOnGround() ){
					//debugFloat	= rigidbody.velocity.y;
//					if(rigidbody.velocity.y > 0) {
						_animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
						_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
						_animation.CrossFade(jumpPoseAnimation.name);
//					} else {
//						_animation[jumpPoseAnimation.name].speed = -landAnimationSpeed;
//						_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
//						_animation.CrossFade(jumpPoseAnimation.name);				
//					}
				}else{
					if(rigidbody.velocity.magnitude > 6){
						_animation[runAnimation.name].speed		= Mathf.Clamp(rigidbody.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
						_animation.CrossFade(runAnimation.name,0.5f);
					}else{
						_animation[walkAnimation.name].speed	= Mathf.Clamp(rigidbody.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
						_animation.CrossFade(walkAnimation.name,0.5f);
					}
				}
			}
		}
	}
	
	// Split wall jumping into separate function?
	void FixedUpdate(){
		if( lastCollisionInfo != nextCollisionInfo ){
			canJump	= (IsOnGround() || (nextCollisionInfo & (int)CollisionInfo.Slope) != 0 || nextCollisionInfo	== (int)CollisionInfo.Wall) && nextJump < Time.time;
			if( nextCollisionInfo == (int)CollisionInfo.Wall ){
				rigidbody.velocity	= Vector3.zero;
			}
		}
		if( !ControlsLocked ){
			if(!isJumping){
				isJumping	= Input.GetButtonDown("Jump");
			}
			inputX			= Input.GetAxisRaw("Horizontal");
			inputY			= Input.GetAxisRaw("Vertical");
			inputSmoothX	= Input.GetAxis("Horizontal");
			inputSmoothY	= Input.GetAxis("Vertical");
			if( !IsOnGround() ){
				rigidbody.AddForce(Physics.gravity,ForceMode.Acceleration);
				velocity.x		= inputSmoothX * moveSpeed;
			}else{
				velocity		= floorSlope * (inputSmoothX * moveSpeed);
			}
			
			if( (isJumping || (inputY == -1 && nextCollisionInfo == (int)CollisionInfo.Wall) || inputY == 1) && canJump ){
				canJump		= false;
				isJumping	= false;
				nextJump	= Time.time + 0.1f;
				if( nextCollisionInfo == (int)CollisionInfo.Wall ){
					rigidbody.AddForce( ((Vector3.up * 0.9f * inputSmoothY) + (wallNormal * (inputY == 0 ? 1f : 0.1f ))) * moveSpeed, ForceMode.VelocityChange );
				}else{
					rigidbody.AddForce( Vector3.up * moveSpeed, ForceMode.VelocityChange );
				}
			}
			rigidbody.AddForce(velocity,ForceMode.Acceleration);
		}
		
		if( particle && ((lastCollisionInfo != nextCollisionInfo && lastCollisionInfo == 0 && nextCollisionInfo != 0) || (lastCollisionInfo & (int)CollisionInfo.Below) != (nextCollisionInfo & (int)CollisionInfo.Below) )){
			if( lastParticleSpawn < Time.time ){
				lastParticleSpawn		= Time.time + 0.25f;
				ParticleSystem p		= (ParticleSystem)Instantiate(particle,contactPoint,Quaternion.identity);
				p.transform.eulerAngles	= contactNormal;
				p.startSpeed			= 4;
				contactPoint			= Vector3.zero;
			}
		}
		
		floorSlope			*= 0;
		lastCollisionInfo	= nextCollisionInfo;
		nextCollisionInfo	= 0;
	}
	
	void OnCollisionStay( Collision collision ){
		contactPoint	= collision.contacts[0].point;
		contactNormal	= collision.contacts[0].normal;
		foreach( ContactPoint contact in collision.contacts ){
			Debug.DrawRay(contact.point,contact.normal*1,Color.red);
			
			//Mathf.Cos(30 * Mathf.Deg2Rad)	= 0.8660254038f;
			//Mathf.Cos(80 * Mathf.Deg2Rad)	= 0.1736481777f;
			
			if( contact.normal.y >= 0.8660254038f ){
				if( (nextCollisionInfo & (int)CollisionInfo.Below) == 0 )	nextCollisionInfo	|= (int)CollisionInfo.Below;
				collisionChanged	= true;
				floorSlope			= Vector3.right;
				Vector3	normal		= contact.normal;
				Vector3.OrthoNormalize(ref normal, ref floorSlope);
				Debug.DrawRay(contact.point,contact.normal*2,Color.green);
				Debug.DrawRay(contact.point,floorSlope*2,Color.green);
			}else if( contact.normal.y >= 0.1736481777f ){
				if( (nextCollisionInfo & (int)CollisionInfo.Slope) == 0 )	nextCollisionInfo	|= (int)CollisionInfo.Slope;
				collisionChanged	= true;
			}else if( Mathf.Abs(contact.normal.y) < 0.1736481777f ){
				if( (nextCollisionInfo & (int)CollisionInfo.Wall)  == 0 )	nextCollisionInfo	|= (int)CollisionInfo.Wall;
				collisionChanged	= true;
				wallNormal	= contact.normal;
			}else{
				if( (nextCollisionInfo & (int)CollisionInfo.Above) == 0 )	nextCollisionInfo	|= (int)CollisionInfo.Above;
				collisionChanged	= true;
			}
		}
	}
	
	public bool IsOnGround(){
		return (nextCollisionInfo & (int)CollisionInfo.Below) != 0;
	}
	
	public bool RegisterEquipmentModel( GameObject obj, EquipmentSlot slot ){
		if( equipment.Count > (int)slot && !equipment[(int)slot] ){
			equipment[(int)slot] = obj;
			obj.GetComponentInChildren<Renderer>().enabled = false;
			return true;
		}
		Debug.LogWarning("Unable to register " + obj.name + " to slot " + slot + "!");
		return false;
	}
	
	public bool RegisterEquipment(EquipmentSlot[] slots){
		bool hasRegistered	= true;
		foreach( EquipmentSlot slot in slots ){
			if( (equipment[(int)slot]).GetComponentInChildren<Renderer>().enabled ){
				Debug.Log("Equipment failed to register in slot: " + slot);
				hasRegistered	= false;
			}
		}
		if( hasRegistered ){
			foreach( EquipmentSlot slot in slots ){
				equipment[(int)slot].GetComponentInChildren<Renderer>().enabled	= true;
			}
		}
		return hasRegistered;
	}
	
	public Vector3 GetSlotPosition( EquipmentSlot slot ){
		if( equipment[(int)slot].GetComponentInChildren<Renderer>().enabled ){
			return equipment[(int)slot].GetComponentInChildren<Transform>().position;
		}
		return Vector3.zero;
	}
	
	public Vector3 GetMousePosition(){
		Vector3 mp	= Input.mousePosition;
		mp.z		= 0-camera.transform.position.z;
		return camera.ScreenToWorldPoint(mp);
	}
}
