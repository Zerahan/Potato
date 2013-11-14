using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	/* Attach to player object to display a heads-up-display for said player
	 * 
	 * Should work just by placing it onto an object that also has a Player script attached.
	//*/
	private Player player;
	private TimedRace gamemode;
	private float recordTime;
	private float currentTime;	// To keep the current time constant during formatting.
	
	// Set these from the inspector, so the code doesn't have to change when the hud's graphics are updated.
	public GUISkin left,right,up;	// Image for the left, right, and up keyboard buttons
	
	// Just changing the transparency of the keyboard buttons.
	// These will be used alot, so it's better to go ahead and store them ahead of time.
	private Color activeColor	= new Color(1,1,1,1);
	private Color inactiveColor	= new Color(1,1,1,0.25f);
	
	void Start () {
		player		= GetComponent<Player>();
		gamemode	= GetComponent<TimedRace>();
	}
	
	void OnGUI(){
		if(player.isHuman){
			// Everything in the group will remain in the same arrangement, even if the group's position is moved
			// Example:
			//     GUI.Box(new Rect(64,0,64,64),""); this sets the x position to the group's position, +64
			GUI.BeginGroup( new Rect(0,10,150,120) );
				recordTime = gamemode.GetRecordTime();
				GUI.Label(new Rect(0,0,150,30),"Record: " + (int)recordTime + ":" + (int)((recordTime - (int)recordTime)*100));
				currentTime = gamemode.GetDisplayTime();
				GUI.Label(new Rect(0,15,150,30),"Current: " + (int)currentTime + ":" + (int)((currentTime - (int)currentTime)*100));
			GUI.EndGroup();
			
			// Temporary thing to show the game's controls until something better can be worked out.
			GUI.BeginGroup(new Rect(0,Screen.height-128,192,128));
				if(Input.GetKey(KeyCode.W)){
					GUI.color = activeColor;
				}else{
					GUI.color = inactiveColor;	// Make button's image transparent if not being pressed
				}
				GUI.skin = up;
				GUI.Box(new Rect(64,0,64,64),"");
				
				GUI.color = ((Input.GetKey(KeyCode.A))?(activeColor):(inactiveColor));
				GUI.skin = left;
				GUI.Box(new Rect(0,64,64,64),"");
				
				GUI.color = ((Input.GetKey(KeyCode.D))?(activeColor):(inactiveColor));
				GUI.skin = right;
				GUI.Box(new Rect(128,64,64,64),"");
				GUI.color = new Color(1,1,1,1);
			GUI.EndGroup();
		}
	}
}
