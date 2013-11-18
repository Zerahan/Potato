using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	private	UserInput	userInput	;
	
	// Information about what the last collisions were
	//private	Vector3		wallNormal			;
	private	int		lastCollisionInfo	;
	private	int		nextCollisionInfo	;
	private bool	collisionChanged	= true;
	
	private	Vector3	wallNormal;
	
	public	enum		CollisionInfo		{
		None	= 0,
		Above	= 1 << 0,	// 1
		Below	= 1 << 1,	// 2
		Slope	= 1 << 2,	// 4
		Wall	= 1 << 3,	// 8
	}
	
	// Movement stuff
	private	Vector3		velocity	;
	private float		nextJump	;
	private	bool		canJump		;
	public	bool		CanJump		{ get{return canJump;} }
	private	float		moveSpeed	= 10f;
	
	private	bool		isJumping		;
	private	float		inputHorizontal	;
	private	float		inputVertical	;
	
	void Start(){
		userInput	= GetComponent<UserInput>();
	}
	
	void Update(){
	}
	
	void FixedUpdate(){
		if( collisionChanged ){
			collisionChanged	= false;
			canJump	= (IsOnGround() || (nextCollisionInfo & (int)CollisionInfo.Slope) != 0 || nextCollisionInfo	== (int)CollisionInfo.Wall) && nextJump < Time.time;
		}
		if( !userInput.ControlsLocked ){
			isJumping		= Input.GetButton("Jump");
			inputHorizontal	= Input.GetAxis("Horizontal");
			inputVertical	= Input.GetAxisRaw("Vertical");
			if( nextCollisionInfo != (int)CollisionInfo.Wall ){
				rigidbody.AddForce(Physics.gravity,ForceMode.Acceleration);
			}else{
				if( canJump ){
					rigidbody.velocity	= rigidbody.velocity * 0;
				}
			}
			
			velocity.x	= inputHorizontal * moveSpeed;
			if( (isJumping || (inputVertical == -1 && nextCollisionInfo == (int)CollisionInfo.Wall) || inputVertical == 1) && canJump ){
				canJump		= false;
				nextJump	= Time.time + 0.5f;
				if( nextCollisionInfo == (int)CollisionInfo.Wall ){
					rigidbody.AddForce( ((Vector3.up * 0.9f * inputVertical) + (wallNormal * (inputVertical == 0 ? 1f : 0.1f ))) * moveSpeed, ForceMode.VelocityChange );
				}else{
					rigidbody.AddForce( Vector3.up * moveSpeed, ForceMode.VelocityChange );
				}
			}
			rigidbody.AddForce(velocity,ForceMode.Acceleration);
		}
		
		lastCollisionInfo	= nextCollisionInfo;
		nextCollisionInfo	= 0;
	}
	
	void OnCollisionStay( Collision collision ){
		foreach( ContactPoint contact in collision.contacts ){
			Debug.DrawRay(contact.point,contact.normal*2,Color.red);
			
			//Mathf.Cos(30 * Mathf.Deg2Rad)	= 0.8660254038f;
			//Mathf.Cos(80 * Mathf.Deg2Rad)	= 0.1736481777f;
			
			if( contact.normal.y >= 0.8660254038f ){
				if( (nextCollisionInfo & (int)CollisionInfo.Below) == 0 )	nextCollisionInfo	|= (int)CollisionInfo.Below;
				collisionChanged	= true;
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
}
