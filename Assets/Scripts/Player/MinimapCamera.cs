using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour {
	public	UserInput	player		;
	public	GUISkin		playerIcon	;
	
	void OnGUI(){
		if( playerIcon ){
			GUI.skin			= playerIcon;
			Vector3 screenPoint	= GetComponent<Camera>().WorldToScreenPoint( player.Center );
			if(screenPoint.y > Screen.height * 0.3f){
				screenPoint.y	= Screen.height * 0.3f;
			}else if( screenPoint.y < 0 ){
				screenPoint.y	= 0;
			}
			if(screenPoint.x > Screen.width){
				screenPoint.x	= Screen.width;
			}else if(screenPoint.x < Screen.width*0.8f){
				screenPoint.x	= Screen.width * 0.8f;
			}
			GUI.Box(new Rect(screenPoint.x-16,(Screen.height - screenPoint.y)-16,32,32),"");
		}
	}
}
