using UnityEngine;
using System.Collections;

public class BreakableObject : MonoBehaviour {
	public float breakSpeed	= 15;
	
	public ParticleSystem particle;
	
	public void OnCollisionEnter( Collision collision ){
		if( collision.relativeVelocity.magnitude > breakSpeed ){
			ParticleSystem p = (ParticleSystem)Instantiate(particle,transform.position,Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
