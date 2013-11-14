using UnityEngine;
using System.Collections;
//tempchange <- remove this line

public class Boost : MonoBehaviour {
	private string	BUTTON				= "Boost";
	public float 	boostStrength		= 0.5f;
	public int 		startBoosts			= 1;
	public int 		maxBoosts			= 10;
	public float	boostRestoreDelay	= 10;
	public float 	moveStrength		= 1f;
	
	private UserInput userInput;
	private bool 	isDebug 		= false;
	
	private bool 	buttonState		;
	private bool 	lastButtonState	;
	private float 	energy			= 0f;
	private float 	maxEnergy		= 0f;
	private Vector3 moveDirection	;
	
	//private bool useGravity;
	
	private bool automatic;
	
	// Use this for initialization
	void Start(){
		userInput	= transform.root.GetComponent<UserInput>();
		boostStrength = boostStrength * Physics.gravity.magnitude;
		moveStrength = moveStrength * Physics.gravity.magnitude;
		maxEnergy	= boostStrength * maxBoosts;
		if (isDebug) {
			energy = 500f;
			//rigidbody.useGravity = false;
		}else{
			energy = boostStrength * startBoosts;
		}
	}
	
	void Update(){
		if(energy >= boostStrength){
			transform.root.GetComponentInChildren<Light>().intensity = (1 * (energy/maxEnergy))+1f;
		}else{
			transform.root.GetComponentInChildren<Light>().intensity = 0f;
		}
	}
	
	
	// Update is called once per frame
	void FixedUpdate () {
		moveDirection	= (userInput.GetMousePosition() - transform.position).normalized;
		//moveDirection.x = Input.GetAxis("Horizontal");
		//moveDirection.y = Input.GetAxis("Vertical");
		//useGravity		= true;
		//lastButtonState	= buttonState;
		//buttonState		= Input.GetButton(button);//moveDirection.magnitude > 0;
		
		if(energy > 0 && moveDirection.magnitude > 0){
			
			// first frame of button press - instant boost
			//if( !lastButtonState && buttonState && energy >= boostStrength ){
			if( Input.GetButtonDown(BUTTON) && energy >= boostStrength ){
				transform.root.rigidbody.velocity += moveDirection.normalized * boostStrength;
				if (!isDebug) {
					energy -= boostStrength;
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
		if (energy < maxEnergy) {
			energy = Mathf.Min(maxEnergy, energy + (Time.deltaTime * boostStrength) / boostRestoreDelay );
		}
		
		
		
		//rigidbody.useGravity = useGravity;
	}
	
	public void OnCollisionEnter(Collision collision){
		//Debug.Log("Adding Energy: " + (collision.relativeVelocity.magnitude));
		if (!isDebug) {
			energy = Mathf.Min(maxEnergy, energy + Mathf.Min(boostStrength,collision.relativeVelocity.magnitude));
		}
	}
	
	public void OnCollisionStay(Collision collision){
	}
	
	public void OnCollisionExit(Collision collision){
	}
}
