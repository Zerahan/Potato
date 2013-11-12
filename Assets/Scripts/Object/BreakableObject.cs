using UnityEngine;
using System.Collections;

public class BreakableObject : MonoBehaviour {
	public ParticleSystem particle;
	
	public void OnCollisionEnter( Collision collision ){
		if( collision.relativeVelocity.magnitude > 15 ){
			ParticleSystem p = (ParticleSystem)Instantiate(particle,transform.position,Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
