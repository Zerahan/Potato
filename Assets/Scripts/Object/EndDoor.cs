using UnityEngine;
using System.Collections;

public class EndDoor : MonoBehaviour {
	/* Attach to "end door" to allow player to "win"
	 * Also shows some graphical stuff in the form of a bright light that fades out
	 * 
	 * Should work just by placing it onto an object that has a collider with "is trigger" checked
	//*/
	
	private float timer;
	private Light[] lights;
	
	public void Start(){
		lights = GetComponentsInChildren<Light>();
	}
	
	public void OnTriggerEnter(Collider obj){
		timer = 1;
		obj.GetComponent<TimedRace>().EndRace(true);
	}
	
	public void Update(){
		if(timer > 0){
			timer -= Time.deltaTime*0.25f;
		}
		
		foreach(Light light in lights){
			light.intensity = timer * 4;
			if(light.intensity < 0.1){
				light.intensity = 0.1f;
			}
		}
	}
}
