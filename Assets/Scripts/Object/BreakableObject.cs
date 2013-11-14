using UnityEngine;
using System.Collections;

public class BreakableObject : MonoBehaviour {
	public float breakSpeed	= 15;
	
	private bool isDestroyed;
	
	public ParticleSystem particle;
	public AudioClip[] sounds;
	
	public bool IsDestroyed(){
		return isDestroyed;
	}
	
	public void OnCollisionEnter( Collision collision ){
		if( collision.relativeVelocity.magnitude > breakSpeed ){
			ParticleSystem p		= (ParticleSystem)Instantiate(particle,transform.position,Quaternion.identity);
			if(sounds.Length > 0){
				AudioSource sound	= p.GetComponent<AudioSource>();
				if(sound){
					sound.clip		= sounds[Random.Range(0,sounds.Length-1)];
					sound.Play();
				}
			}
			isDestroyed = true;
			Destroy(gameObject);
		}
	}
}