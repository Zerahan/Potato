using UnityEngine;
using System.Collections;
//tempchange <- remove this line

public class Boost : MonoBehaviour {
	private string	BUTTON			= "Fire2";
	public float 	jumpStrength	= 0.5f;
	public float 	moveStrength	= 1f;
	public int 		maxJumps		= 10;
	
	private UserInput userInput;
	private bool 	isDebug 		= false;
	
	private bool 	buttonState		;
	private bool 	lastButtonState	;
	private float 	energy			= 0f;
	private float 	maxEnergy		= 0f;
	private Vector3 moveDirection	;
	
	private bool useGravity;
	
	private bool automatic;
	
	// Use this for initialization
	void Start(){
		userInput	= GetComponent<UserInput>();
		jumpStrength = jumpStrength * Physics.gravity.magnitude;
		moveStrength = moveStrength * Physics.gravity.magnitude;
		maxEnergy	= jumpStrength * maxJumps;
		if (isDebug) {
			energy = 500f;
			rigidbody.useGravity = false;
			
		}
	}
	
	void Update(){
		if(energy >= jumpStrength){
			GetComponent<Light>().intensity = (3 * (energy/maxEnergy))+1f;
		}else{
			GetComponent<Light>().intensity = 0f;
		}
	}
	
	
	// Update is called once per frame
	void FixedUpdate () {
		moveDirection	= (userInput.GetMousePosition() - transform.position).normalized;
		//moveDirection.x = Input.GetAxis("Horizontal");
		//moveDirection.y = Input.GetAxis("Vertical");
		useGravity		= true;
		//lastButtonState	= buttonState;
		//buttonState		= Input.GetButton(button);//moveDirection.magnitude > 0;
		
		if(energy > 0 && moveDirection.magnitude > 0){
			
			// first frame of button press - instant boost
			//if( !lastButtonState && buttonState && energy >= jumpStrength ){
			if( Input.GetButtonDown(BUTTON) && energy >= jumpStrength ){
				rigidbody.velocity += moveDirection.normalized * jumpStrength;
				if (!isDebug) {
					energy -= jumpStrength;
				}
			}
			
			/*
			// later frames of button press - gradual force
			if( lastButtonState && buttonState ){
				useGravity	= false;
				if(	energy - moveStrength > 0 ){
					energy -= moveStrength;
					rigidbody.AddForce(moveDirection.normalized * moveStrength);
				}else{
					Debug.Log("Expending all energy...");
					rigidbody.AddForce(moveDirection.normalized * energy);
					energy = 0;
				}
			}
			*/
		}
		
		//rigidbody.useGravity = useGravity;
	}
	
	public void OnCollisionEnter(Collision collision){
		//Debug.Log("Adding Energy: " + (collision.relativeVelocity.magnitude));
		if (!isDebug) {
			energy = Mathf.Min(maxEnergy, energy + Mathf.Min(jumpStrength,collision.relativeVelocity.magnitude));
		}
	}
	
	public void OnCollisionStay(Collision collision){
	}
	
	public void OnCollisionExit(Collision collision){
	}
}
