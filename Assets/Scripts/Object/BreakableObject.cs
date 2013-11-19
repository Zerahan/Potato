using UnityEngine;
using System.Collections;

public class BreakableObject : MonoBehaviour {
	public	float	breakVelocity	= 16;
	private	bool	isDestroyed		;
	public	bool	IsDestroyed		{ get{return isDestroyed;} }
	
	public	ParticleSystem	particle	;
	public	AudioClip[]		breakSounds	;
	
	// get relative angle, use that to determine force?
	// normalize
	public void OnCollisionEnter( Collision collision ){
		if( (collision.relativeVelocity.magnitude * (1-Vector3.Dot(collision.contacts[0].normal,collision.rigidbody.velocity.normalized))) >= breakVelocity ){
			//Debug.Log( collision.relativeVelocity.magnitude * (1-Vector3.Dot(collision.contacts[0].normal,collision.rigidbody.velocity.normalized)) );
			Kill();
		}
	}
	
	public void Kill(){
		isDestroyed = true;
		ParticleSystem p		= (ParticleSystem)Instantiate(particle,transform.position,Quaternion.identity);
		if(breakSounds.Length > 0){
			AudioSource sound	= p.GetComponent<AudioSource>();
			if(sound){
				sound.clip		= breakSounds[Random.Range(0,breakSounds.Length-1)];
				sound.Play();
				sound.volume = 1f;
			}
		}
		Destroy(gameObject);
	}
}