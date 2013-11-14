using UnityEngine;
using System.Collections;

public class TemporaryParticles : MonoBehaviour {
	private float timer;
	
	private ParticleSystem ps;
	private float endTime;
	
	void Start(){
		ps			= GetComponent<ParticleSystem>();
		endTime		= Time.time + (ps.startLifetime*2) + ps.duration;
		timer		= 1;
	}
	
	void Update(){
		if( ps ){
			if(endTime < Time.time && timer < 0){
				Destroy(gameObject);
			}
		}
		timer -= Time.deltaTime;
		GetComponent<Light>().intensity = 8 * timer;
		if(timer < 0){
			Destroy(gameObject);
		}
	}
}
