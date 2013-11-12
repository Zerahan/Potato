using UnityEngine;
using System.Collections;

public class ContactLight : MonoBehaviour {
	// Attach to a light so that dims and is deleted when no longer visible.
	private float timer;
	
	void Start () {
		timer = 1;
	}
	
	void Update () {
		timer -= Time.deltaTime;
		GetComponent<Light>().intensity = timer;
		if(timer < 0){
			Destroy(gameObject);
		}
	}
}
