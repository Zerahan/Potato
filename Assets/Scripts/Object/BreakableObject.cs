using UnityEngine;
using System.Collections;

public class BreakableObject : MonoBehaviour {
	public float breakSpeed	= 15;
	
	private bool isDestroyed;
	
	public ParticleSystem particle;
	
	public bool IsDestroyed(){
		return isDestroyed;
	}
	
	public void OnCollisionEnter( Collision collision ){
		if( collision.relativeVelocity.magnitude > breakSpeed ){
			ParticleSystem p = (ParticleSystem)Instantiate(particle,transform.position,Quaternion.identity);
			isDestroyed = true;
			Destroy(gameObject);
		}
	}
}
