using UnityEngine;
using System.Collections;

public class Grapple : Equipment {
	private		UserInput	userInput		;
	private		LineRenderer	lineRenderer;
	
	//private		string		button			= "Fire1";
	
	public		Material	cordMaterial	;
	public		float		minDistance		= 1f;
	public		float		maxDistance		= 5f;
	public		float		pullSpeed		= 4f;
	private		Vector3		pullForce		;
	
	private		bool		didRaycastHit	= false;
	private		RaycastHit	raycast			;
	
	private		Vector3		hitPoint		;
	private		GameObject	hitObject		;
	
	public override void Start(){
		base.Start();
		userInput		= transform.root.GetComponent<UserInput>();
		lineRenderer	= gameObject.AddComponent<LineRenderer>();
		if(cordMaterial){
			lineRenderer.material		= cordMaterial;
		}else{
			Debug.LogWarning("No grapple cord material selected!");
		}
		lineRenderer.SetColors(Color.yellow,Color.yellow);
		lineRenderer.SetWidth(0.2f,0.2f);
		lineRenderer.SetVertexCount(2);
		
		lineRenderer.renderer.enabled	= false;
	}
	
	public override void Update(){
		CastRay();
		if( isEnabled && !userInput.IsControlLocked() ){
			if( Input.GetButtonDown("Fire1") ){
				DoAction(Action.Enable);
			}else if(Input.GetButtonUp("Fire1")){
				DoAction(Action.Disable);
			}
		}
		base.Update();
	}
	
	void OnGUI(){
		if(targetIcon){
			GUI.skin			= targetIcon;
			Vector3 screenPoint	= Camera.main.WorldToScreenPoint(targetPoint);
			GUI.Box(new Rect(screenPoint.x-16,(Screen.height - screenPoint.y)-16,32,32),"");
		}
	}
	
	private void CastRay(){
		didRaycastHit	= Physics.Raycast(transform.position, userInput.GetMousePosition()-(transform.position), out raycast, maxDistance);
		if( didRaycastHit ){
			targetPoint	= raycast.point;
		}else if((userInput.GetMousePosition()-transform.position).magnitude > maxDistance){
			targetPoint	= transform.position + ((userInput.GetMousePosition()-transform.position).normalized * maxDistance);
		}else{
			targetPoint	= transform.position + (userInput.GetMousePosition()-transform.position);
		}
	}
}

/*
public class Grapple : MonoBehaviour {
	private Player player;
	public Material material;
	public GUISkin	targetIcon;
	private UserInput userInput;
	
	private RaycastHit	lastRaycastHit;
	private bool		isLastRaycastHit;
	private Vector3		estimatedHitTarget;
	
	private string		useButton		= "Grapple";
	private bool		canActivate		= true;
	private bool		isActive		;
	private float		lastUse			;
	private float		useDelay		= 0.01f;
	private LineRenderer lineRenderer	;
	
	private Transform	hitObject		;
	private Vector3		hitPoint		;
	private float		minDistance		= 2.00f;
	private float		maxDistance		= 10.00f;
	private Vector3		offset			= new Vector3(0,1.5f,0);
	private float		hookPullSpeed	= 4;
	
	public enum Action{
		Grapple,
		UnGrapple
	}
	
	void Start () {
		player					= transform.root.GetComponent<Player>();
		userInput				= GetComponent<UserInput>();
		lineRenderer			= gameObject.AddComponent<LineRenderer>();
		lineRenderer.material	= material;
		lineRenderer.SetColors(Color.yellow,Color.yellow);
		lineRenderer.SetWidth(0.2f,0.2f);
		lineRenderer.SetVertexCount(2);
		
		renderer.enabled = false;
	}
	
	void Update(){
		if( !player.GetComponent<UserInput>().IsControlLocked() && lastUse < Time.time ){
			if( Input.GetButtonDown(useButton) ){
				DoAction(Action.Grapple);
			}else if(Input.GetButtonUp(useButton)){
				DoAction(Action.UnGrapple);
			}
		}
		if( isActive ){
			if(hitObject){
				lineRenderer.SetPosition(0,transform.position + offset);
				lineRenderer.SetPosition(1,hitObject.position + hitPoint);
				PullToHook();
			}else{
				DoAction(Action.UnGrapple);
			}
		}
		//isLastRaycastHit	= Physics.Raycast(transform.position + offset, userInput.GetMousePosition()-(transform.position + offset), out lastRaycastHit, maxDistance);
		if( Physics.Raycast(transform.position + offset, userInput.GetMousePosition()-(transform.position + offset), out lastRaycastHit, maxDistance) ){
			estimatedHitTarget		= lastRaycastHit.point;
		}else{
			if((userInput.GetMousePosition()-(transform.position + offset)).magnitude > maxDistance){
				estimatedHitTarget	= transform.position + offset + ((userInput.GetMousePosition()-(transform.position + offset)).normalized * maxDistance);
			}else{
				estimatedHitTarget	= transform.position + offset + (userInput.GetMousePosition()-(transform.position + offset));
			}
		}
	}
	
	void OnGUI(){
		//GUI.Box(new Rect(screenPoint.x,(Screen.height - screenPoint.y),32,32),"");
		GUI.skin			= targetIcon;
		Vector3 screenPoint	= Camera.main.WorldToScreenPoint(estimatedHitTarget);
		GUI.Box(new Rect(screenPoint.x-16,(Screen.height - screenPoint.y)-24,32,32),"");
	}
	
	public bool IsActive(){
		return isActive;
	}
	
	public bool DoAction( Action action ) {
		switch (action) {
			case Action.Grapple:
				//Debug.LogError("Only the grappling hook can grapple!");
				DoGrapple();
				return false;
				break;
			
			case Action.UnGrapple:
				DoUnGrapple();
				return true;
				break;
			
			default:
				Debug.LogError("Invalid action " + action);
				return false;
				break;				
		}
		return false;
	}
	
	private bool DoGrapple(){
		canActivate = true;
		//if( !isActive ){
			lastUse = Time.time + useDelay;
			RaycastHit hit;
			if( Physics.Raycast(transform.position + offset, userInput.GetMousePosition()-(transform.position + offset), out hit) ){
				if( (hit.point-transform.position).magnitude <= maxDistance ){
					isActive	= true;
					hitObject	= hit.transform;
					hitPoint	= hit.point - hitObject.position;
					//rigidbody.useGravity	= false;
					renderer.enabled = true;
					return true;
				}
			}
		//}
		//DoAction(Action.UnGrapple);
		return false;
	}
	
	private void PullToHook(){
		if(isActive){
			Vector3 direction	= (hitObject.position + hitPoint)-transform.position;
			Vector3 velocity	= ((Time.deltaTime * hookPullSpeed) * direction);
			
			if(direction.magnitude >= minDistance){
				rigidbody.velocity = (rigidbody.velocity) + (velocity);
			}else{
				//rigidbody.velocity = Vector3(rigidbody.velocity.x*0.9, rigidbody.velocity.y + , rigidbody.velocity.z*0.9);
			}
		}else if( lastRaycastHit.transform ){
			hitObject	= lastRaycastHit.transform;
			hitPoint	= lastRaycastHit.point - hitObject.position;
		}
	}
	
	public void DoUnGrapple(){
		lastUse		= Time.time + useDelay;
		canActivate	= true;
		isActive	= false;
		hitPoint	= Vector3.zero;
		hitObject	= null;
		
		renderer.enabled = false;
	}
	
	public void OnDrawGizmos(){
		if(hitPoint != Vector3.zero){
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(hitObject.position + hitPoint,1f);
		}
		
		if (!userInput) {
			userInput = GetComponent<UserInput>();
		}
		
		Gizmos.DrawWireSphere(userInput.GetMousePosition(),1f);
		
		Gizmos.DrawLine( transform.position, userInput.GetMousePosition() );
		
		Gizmos.color	= Color.red;
		Gizmos.DrawLine( transform.position, estimatedHitTarget );
	}
}
//*/
