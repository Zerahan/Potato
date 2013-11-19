using UnityEngine;
using System.Collections;
using GameSettings;

public class Grapple : Equipment {
	private		LineRenderer	lineRenderer;
	
	private		string		button			= "Action1";
	
	protected	string		Name			= "Magnetic Grapple";
	
	public		Material	cordMaterial	;
	public		float		minDistance		= 2f;
	public		float		maxDistance		= 8f;
	public		float		pullForce		= 4f;
	private		Vector3		pullDirection	;
	
	private		bool		didRaycastHit	= false;
	private		RaycastHit	raycast			;
	
	private		Vector3		hitPoint		;
	private		Transform	hitObject		;
	
	public override void Start(){
		base.Start();
		lineRenderer	= gameObject.AddComponent<LineRenderer>();
		if(cordMaterial){
			lineRenderer.material		= cordMaterial;
			lineRenderer.SetColors(Color.white, Color.cyan);
		}else{
			Debug.LogWarning("No grapple cord material selected!");
		}
		lineRenderer.SetColors(Color.white,Color.cyan);
		lineRenderer.SetWidth(0.1f,0.2f);
		lineRenderer.SetVertexCount(2);
		
		lineRenderer.renderer.enabled	= false;
	}
	
	public override void Update(){
		CastRay();
		if( isEnabled && !player.IsControlLocked ){
			if( Input.GetButtonDown(button) ){
				DoAction(Action.Enable);
			}else if(Input.GetButtonUp(button)){
				DoAction(Action.Disable);
			}
		}
		base.Update();
	}
	
	void FixedUpdate(){
		if(IsActive){
			//transform.root.rigidbody.AddForce(pullDirection.normalized*pullForce);
			transform.root.rigidbody.AddForce( pullDirection.normalized * Physics.gravity.magnitude * pullForce, ForceMode.Acceleration );
		}
	}
	
	void OnGUI(){
		if(targetIcon){
			GUI.skin			= targetIcon;
			Vector3 screenPoint	= player.camera.WorldToScreenPoint(targetPoint);
			GUI.Box(new Rect(screenPoint.x-16,(Screen.height - screenPoint.y)-16,32,32),"");
		}
	}
	
	private void CastRay(){
		didRaycastHit	= Physics.Raycast(player.Center, player.GetMousePosition()-player.Center, out raycast, maxDistance);
		if( didRaycastHit ){
			targetPoint	= raycast.point;
		}else if((player.GetMousePosition()-player.Center).magnitude > maxDistance){
			targetPoint	= player.Center + ((player.GetMousePosition()-player.Center).normalized * maxDistance);
		}else{
			targetPoint	= player.Center + (player.GetMousePosition()-player.Center);
		}
	}
	
	protected override void OnActiveStay(){
		if( IsActive ){
			if(!hitObject || (hitObject.GetComponent<BreakableObject>() && hitObject.GetComponent<BreakableObject>().IsDestroyed)){
				lineRenderer.renderer.enabled	= false;
				DoAction(Action.Disable);
			}else{
				lineRenderer.SetPosition(0,player.GetSlotPosition(slots[0]));
				lineRenderer.SetPosition(1,hitObject.position + hitPoint);
				pullDirection		= (hitObject.position + hitPoint)-player.Center;
				if(pullDirection.magnitude < minDistance){
					pullDirection		= Vector3.zero;
				}else if(pullDirection.magnitude > maxDistance){
					//OnActiveEnd();	// disconnect if too far
				}
			}
		}else{
			if( didRaycastHit && raycast.transform ){
				IsActive	= true;
				hitObject	= raycast.transform;
				hitPoint	= raycast.point - hitObject.position;
				lineRenderer.SetPosition(0,player.GetSlotPosition(slots[0]));
				lineRenderer.SetPosition(1,hitObject.position + hitPoint);
				lineRenderer.renderer.enabled = true;
			}
		}
	}
	
	protected override void OnActiveEnd(){
		IsActive	= false;
		hitPoint	= Vector3.zero;
		hitObject	= null;
		lineRenderer.renderer.enabled = false;
	}
	
	public void OnDrawGizmos(){
		if(hitObject && hitPoint != Vector3.zero){
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(hitObject.position + hitPoint,1f);
		}
		
		if (!player) {
			player = transform.root.GetComponent<PlayerController>();
		}
		
		Gizmos.DrawWireSphere( player.GetMousePosition(),1f );
		
		Gizmos.DrawLine( player.Center, player.GetMousePosition() );
		
		Gizmos.color	= Color.red;
		Gizmos.DrawLine( player.Center, raycast.point );
	}
}