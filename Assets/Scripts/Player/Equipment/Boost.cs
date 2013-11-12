using UnityEngine;
using System.Collections;
//tempchange <- remove this line

public class Boost : MonoBehaviour {
	public string BUTTON	= "Jump";
	
	private UserInput userInput;
	
	private bool buttonState;
	private bool lastButtonState;
	private float energy		= 0f;
	private float maxEnergy		= 200f;
	private float jumpStrength	= Physics.gravity.magnitude * 1.25f;
	private float moveStrength	= Physics.gravity.magnitude * 1; //2f;
	private Vector3 moveDirection;
	
	private bool useGravity;
	
	private bool automatic;
	
	// Use this for initialization
	void Start(){
		userInput = GetComponent<UserInput>();
		maxEnergy = jumpStrength * 10;
		//rigidbody.useGravity = false;
	}
	
	void Update(){
		if(energy > 0){
			GetComponent<Light>().intensity = (3 * (energy/maxEnergy))+1f;
		}else{
			GetComponent<Light>().intensity = 0f;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		moveDirection.x = Input.GetAxis("Horizontal");
		moveDirection.y = Input.GetAxis("Vertical");
		useGravity		= true;
		//lastButtonState	= buttonState;
		//buttonState		= Input.GetButton(button);//moveDirection.magnitude > 0;
		
		if(!userInput.IsOnGround() && !userInput.IsGrappled() && energy > 0 && moveDirection.magnitude > 0){
			
			// first frame of button press - instant boost
			//if( !lastButtonState && buttonState && energy >= jumpStrength ){
			if( Input.GetButtonDown(BUTTON) && energy >= jumpStrength ){
				rigidbody.velocity += moveDirection.normalized * jumpStrength;
				energy -= jumpStrength;
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
		energy = Mathf.Min(maxEnergy, energy + collision.relativeVelocity.magnitude);
	}
	
	public void OnCollisionStay(Collision collision){
	}
	
	public void OnCollisionExit(Collision collision){
	}
}
