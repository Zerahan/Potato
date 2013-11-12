using UnityEngine;
using System.Collections;

public class VelocityCapper : MonoBehaviour {
	private float velocityCap	= 50f;
	void FixedUpdate () {
		Vector3 velocity = rigidbody.velocity;
		if(rigidbody.velocity.x > velocityCap){
			velocity.x	= velocityCap;
		}
		if(rigidbody.velocity.y > velocityCap){
			velocity.y	= velocityCap;
		}
		if(rigidbody.velocity.z > velocityCap){
			velocity.z	= velocityCap;
		}
		rigidbody.velocity	= velocity;
	}
}
