using UnityEngine;
using System.Collections;

public class TimedRace : MonoBehaviour {
	
	private Player player;
	
	private bool hasStarted		= false;
	private float recordTime	= 30.0f;
	private float startTime;
	private float endTime;
	
	private float endTimer;
	
	void Start () {
		player = transform.root.GetComponent<Player>();
	}
	
	void Update () {
		if(endTimer != 0){
			if( endTimer < Time.time ){
				Restart();
			}
		}
	}
	
	public bool HasStarted(){
		return hasStarted;
	}
	
	public float GetDisplayTime(){
		if(hasStarted){
			return (Time.time - startTime);
		}else{
			if(endTimer != 0){
				return endTime;
			}else{
				return 0.0f;
			}
		}
	}
	
	public void Restart(){
		endTime		= 0;
		endTimer	= 0;
		player.GetComponent<UserInput>().LockControls(false);
		player.transform.position = new Vector3(0,0,0);
		player.rigidbody.velocity = new Vector3(0,0,0);
		//player.renderer.enabled = true;
	}
	
	public void StartRace(){
		hasStarted	= true;
		startTime	= Time.time;
	}
	
	public void EndRace(bool hasWon){
		hasStarted = false;
		float time	= Time.time;
		endTimer	= time + 2;
		endTime = (time - startTime);
		player.GetComponent<UserInput>().LockControls(true);
		//player.renderer.enabled = false;
		Grapple hook = transform.root.GetComponent<Grapple>();
		hook.Disable();
		if(hasWon){
			player.rigidbody.velocity = new Vector3(player.rigidbody.velocity.x*0.25f,player.rigidbody.velocity.y,player.rigidbody.velocity.z*0.25f);
			if( endTime <= recordTime){
				recordTime = endTime;
			}
		}else{
			player.rigidbody.velocity = new Vector3(0,0,0);
		}
	}
	
	public float GetRecordTime(){
		return recordTime;
	}
}
