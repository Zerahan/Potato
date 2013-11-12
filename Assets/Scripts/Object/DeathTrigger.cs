using UnityEngine;
using System.Collections;

public class DeathTrigger : MonoBehaviour{
	/* Attach to "death trap" to allow player to "lose"
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
		if(obj.transform.GetComponent<Player>()){
			timer = 2;
			obj.GetComponent<TimedRace>().EndRace(false);
		}
	}
	
	public void Update(){
		if(timer > 0){
			timer -= Time.deltaTime;
		}
		
		foreach(Light light in lights){
			light.intensity = timer * 2;
			if(light.intensity < 4){
				light.intensity = 4;
			}
		}
	}
}
