using UnityEngine;
using System.Collections;

public class WindTunnel : MonoBehaviour{
	public Vector3 windDirection	= new Vector3(0,0,0);
	public float windStrength		= 5f;
	
	private Vector3 wind;
	
	public void Start(){
		wind = windDirection.normalized * windStrength;
	}
	
	public void OnTriggerStay(Collider collider){
		collider.rigidbody.AddForce(wind);
	}
	
	public void OnDrawGizmos(){
		//Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		//Gizmos.matrix	= rotationMatrix;
		Gizmos.color	= new Color(1,1,1,0.25f);
		Gizmos.DrawCube(transform.position,transform.localScale);
		Gizmos.color	= Color.yellow;
		Gizmos.DrawWireCube(transform.position,transform.localScale);
		Gizmos.DrawRay(transform.position,(windDirection*windStrength));
	}
}
