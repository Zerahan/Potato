using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	/* Handles most of the "player" stuff
	 * 
	 * Should work just by placing it onto an object that also has a Player script attached.
	 * 
	 * // Variables
	 * human				// Is the player a human or a computer?
	 * 
	 * // Functions
	 * IsControlLocked()	// Check if the player's controls are locked
	 * SetControlLocked(bool)	// Locks/unlocks controls
	 * 
	 * To Do:
	 *	The camera should be handled by a seperate script.
	 *	Record keeping should be handled by a seperate script on per-map basis.
	 *		Set best win time to player's best, or 10th place (if no player score on record). Something like "bestWinTime = Records.GetPlayerRecord();"
	 *		Award stars based on completion time. 1 for completing, 2 for under 20 seconds, 3 for under 10
	 *	Replace the "dropAmount" stuff with something that calculates the map's bounds, and resets the player if they manage to escape those bounds
	 *		Possible easter eggs for skilled players?
	 *	Automatically get the player's visible body
	 *	Make player into a prefab that does not collide with other players (potential multiplayer)
	//*/
	
	public bool human;						// No point in displaying graphical stuff to a computer
	public GameObject body;					// Set in inspector to the player's visible body, allows for the body to be made invisible
	
	private float timer;
	private int dropAmount		= -5;		// If the player gets outside the map, this is how far down they can drop before resetting
	private int maxCamDist		= 5;		// For camera position calculations
	private int startCamY		= 10;
	
	public void Start(){
		startCamY = (int)Camera.main.transform.position.y;
	}
	
	public void Update () {
		// Makes the main camera follow the player around the map
		// The player will be at the bottom of the camera's field of view when they
		//   are at the bottom of the map, and at the top when they are at the top
		if(Camera.main){
			float y = (maxCamDist * ((transform.position.y/80)*2 - 1));
			if(y < -maxCamDist){
				y = -maxCamDist;
			}
			if(y > maxCamDist){
				y = maxCamDist;
			}
			y = transform.position.y - y;
			if(y < startCamY){
				y = startCamY;
			}
			Camera.main.transform.position = new Vector3(transform.position.x,y,Camera.main.transform.position.z);
		}
		
		// Reset if the player dropped too low.
		if(transform.position.y < dropAmount){
			transform.position = new Vector3(0,0,0);
			rigidbody.velocity = new Vector3(0,0,0);
		}
	}
}
