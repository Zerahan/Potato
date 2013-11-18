using UnityEngine;
using System.Collections;

public class Boost : MonoBehaviour {
	private string	BUTTON				= "Action2";
	public float 	boostStrength		= 0.5f;
	public int 		startBoosts			= 1;
	public int 		maxBoosts			= 10;
	public float	boostRestoreDelay	= 10;
	public float 	moveStrength		= 1f;
	public GameObject body				;
	public float	maxLightIntensity	= 4;
	
	private PlayerController player;
	private bool 	isDebug 		= false;
	private Light	light				;
	
	private bool 	buttonState		;
	private bool 	lastButtonState	;
	private float 	energy			= 0f;
	private float 	maxEnergy		= 0f;
	private Vector3 moveDirection	;
	
	private bool useGravity;
	
	private bool automatic;
	
	// Use this for initialization
	void Start(){
		player			= transform.root.GetComponent<PlayerController>();
		boostStrength	= boostStrength * Physics.gravity.magnitude;
		moveStrength	= moveStrength * Physics.gravity.magnitude;
		light			= transform.root.GetComponentInChildren<Light>();
		maxEnergy		= boostStrength * maxBoosts;
		if (isDebug) {
			energy = 500f;
			transform.root.rigidbody.useGravity = false;
		}else{
			energy = boostStrength * startBoosts;
		}
	}
	
	
	void Update(){
		light.intensity = Mathf.Sqrt(Mathf.Floor(energy/boostStrength)/maxBoosts) * maxLightIntensity;
	}
	
	
	// Update is called once per frame
	void FixedUpdate () {
		moveDirection	= (player.GetMousePosition() - transform.position).normalized;
		useGravity		= true;
		
		if(energy > 0 && moveDirection.magnitude > 0){
			
			if( Input.GetButtonDown(BUTTON) && energy >= boostStrength ){
				transform.root.rigidbody.velocity += moveDirection.normalized * boostStrength;
				if (!isDebug) {
					energy -= boostStrength;
				}
			}
		}
		if (energy < maxEnergy) {
			energy = Mathf.Min(maxEnergy, energy + (Time.deltaTime * boostStrength) / boostRestoreDelay );
		}
	}
	
	public void OnCollisionEnter(Collision collision){
		if (!isDebug) {
			energy = Mathf.Min(maxEnergy, energy + Mathf.Min(boostStrength,collision.relativeVelocity.magnitude));
		}
	}
	
	public void OnCollisionStay(Collision collision){
	}
	
	public void OnCollisionExit(Collision collision){
	}
}
