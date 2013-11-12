using UnityEngine;
using System.Collections;

public class ContactParticles : MonoBehaviour {
	// Attach to a particle system so that it is deleted when it finishes emitting.
	private ParticleSystem ps;
	
	void Start () {
		ps = GetComponent<ParticleSystem>();
	}
	
	void Update () {
		if(ps){
			if(!ps.IsAlive()){
				Destroy(gameObject);
			}
		}
	}
}
