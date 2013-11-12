using UnityEngine;
using System.Collections;

public class Grapple : MonoBehaviour {
	private Player player;
	public Material material;
	
	private string useButton	= "Fire1";
	private bool canActivate	= true;
	private bool isActive;
	private float lastUse;
	private float useDelay		= 0.01f;
	private LineRenderer lineRenderer;
	
	private Transform hitObject;
	private Vector3 hitPoint;
	private float minDistance	= 2.00f;
	private float maxDistance	= 10.00f;
	private Vector3 offset		= new Vector3(0,0.5f,0);
	private float hookPullSpeed	= 4;
	
	void Start () {
		player					= transform.root.GetComponent<Player>();
		lineRenderer			= gameObject.AddComponent<LineRenderer>();
		lineRenderer.material	= material;
		lineRenderer.SetColors(Color.yellow,Color.yellow);
		lineRenderer.SetWidth(0.2f,0.2f);
		lineRenderer.SetVertexCount(2);
		
		renderer.enabled = false;
	}
	
	void Update () {
		if( !player.GetComponent<UserInput>().AreControlsLocked() && Input.GetButtonDown(useButton) && lastUse < Time.time ){
			Action();
		}
		if( isActive ){
			if(hitObject){
				lineRenderer.SetPosition(0,transform.position + offset);
				lineRenderer.SetPosition(1,hitObject.position + hitPoint);
				PullToHook();
			}else{
				Disable();
			}
		}
	}
	
	public bool IsActive(){
		return isActive;
	}
	
	private bool Action(){
		canActivate = true;
		if( !isActive ){
			lastUse = Time.time + useDelay;
			RaycastHit hit;
			if( Physics.Raycast(transform.position + offset, GetMousePosition()-(transform.position + offset), out hit) ){
				if( (hit.point-transform.position).magnitude <= maxDistance ){
					isActive	= true;
					hitObject	= hit.transform;
					hitPoint	= hit.point - hitObject.position;
					//rigidbody.useGravity	= false;
					renderer.enabled = true;
					return true;
				}
			}
		}
		Disable();
		return false;
	}
	
	private void PullToHook(){
		Vector3 direction	= (hitObject.position + hitPoint)-transform.position;
		Vector3 velocity	= ((Time.deltaTime * hookPullSpeed) * direction);
		
		if(direction.magnitude >= minDistance){
			rigidbody.velocity = (rigidbody.velocity) + (velocity);
		}else{
			//rigidbody.velocity = Vector3(rigidbody.velocity.x*0.9, rigidbody.velocity.y + , rigidbody.velocity.z*0.9);
		}
	}
	
	public void Disable(){
		lastUse		= Time.time + useDelay;
		canActivate	= true;
		isActive	= false;
		hitPoint	= Vector3.zero;
		hitObject	= null;
		
		renderer.enabled = false;
	}

	private Vector3 GetMousePosition(){
		Vector3 mp	= Input.mousePosition;
		mp.z		= 0-Camera.main.transform.position.z;
		return Camera.main.ScreenToWorldPoint(mp);
	}
	
	public void OnDrawGizmos(){
		if(hitPoint != Vector3.zero){
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(hitObject.position + hitPoint,1f);
		}
		Gizmos.DrawWireSphere(GetMousePosition(),1f);
		
		Gizmos.DrawLine( transform.position, GetMousePosition() );
	}
}
