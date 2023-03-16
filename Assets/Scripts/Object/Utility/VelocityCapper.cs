using UnityEngine;
using System.Collections;

public class VelocityCapper : MonoBehaviour {
	private float velocityCap	= 50f;
	void FixedUpdate () {
		Vector3 velocity = GetComponent<Rigidbody>().velocity;
		if(GetComponent<Rigidbody>().velocity.x > velocityCap){
			velocity.x	= velocityCap;
		}
		if(GetComponent<Rigidbody>().velocity.y > velocityCap){
			velocity.y	= velocityCap;
		}
		if(GetComponent<Rigidbody>().velocity.z > velocityCap){
			velocity.z	= velocityCap;
		}
		GetComponent<Rigidbody>().velocity	= velocity;
	}
}
