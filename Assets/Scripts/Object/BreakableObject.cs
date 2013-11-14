using UnityEngine;
using System.Collections;

public class BreakableObject : MonoBehaviour {
	public	float	breakVelocity	= 15;
	private	bool	isDestroyed		;
	public	bool	IsDestroyed		{ get{return isDestroyed;} }
	
	public	ParticleSystem	particle	;
	public	AudioClip[]		breakSounds	;
	
	public void OnCollisionEnter( Collision collision ){
		if( collision.relativeVelocity.magnitude >= breakVelocity ){
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
			}
		}
		Destroy(gameObject);
	}
}