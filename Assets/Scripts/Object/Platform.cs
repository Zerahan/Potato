using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	/* Attach to an object to make a moving platform.
	 * 
	 * Currently requires a rendered object to be attached to an immobile empty object to function properly
	 * 
	 * To Do:
	 *  Make this follow a set path instead
	 *  Auto-aquire mover
	 *  Lock a player's movement to this when in contact with it
	 * 
	 * Running into EVERYTHING* destroys it - run into bosses to destroy bosses
	//*/
	
	public float moveDistance = 0.0f;	// how far from the origin should it move along the x axis
	public float startPercent = 0.0f;	// what percentage of the way is it along the path
	public int speed = 8;				// how fast it moves, currently not based on seconds
	public GameObject mover;			// MUST be set in inspector to make the platform mobile
	
	private bool isTouching = false;
	private bool isAscending;
	
	public void Start(){
	}
	
	public void OnCollisionEnter(){
		isTouching = true;
	}
	public void OnCollisionExit(){
		isTouching = false;
	}
	
	public void Update(){
		if( isAscending ){
			if(startPercent < 1){
				startPercent += Time.deltaTime * speed/64f;
			}else{
				isAscending = false;
			}
		}else{
			if(startPercent > 0){
				startPercent -= Time.deltaTime * speed/64f;
			}else{
				isAscending = true;
			}
		}
		if(mover){
			mover.transform.position = transform.position + new Vector3(moveDistance * startPercent,0,0);
		}
	}
	
	// This renders stuff in the editor only - does not transfer to compiled version
	public void OnDrawGizmos(){
		if(moveDistance != 0){
			if( startPercent > 1 ){
				startPercent = 1;
			}
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(transform.position,new Vector3(2,1,2));
			Gizmos.DrawWireCube(transform.position + new Vector3(moveDistance,0,0),new Vector3(2,1,2));
			Gizmos.DrawLine(transform.position,transform.position + new Vector3(moveDistance,0,0));
			Gizmos.DrawLine(transform.position+new Vector3(0f,-1.5f,0f),transform.position + new Vector3(0f,1.5f,0f));
			if(mover){
				mover.transform.position = transform.position + new Vector3(moveDistance * startPercent,0,0);
			}
		}
	}
}
