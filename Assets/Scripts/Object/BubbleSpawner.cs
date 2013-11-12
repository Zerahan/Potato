using UnityEngine;
using System.Collections;

public class BubbleSpawner : MonoBehaviour {
	
	public float spawnRate	= 0.5f;
	public int maxSpawns	= 10;
	public float lifeSpan	= 10;
	
	public GameObject bubble;
	
	private GameObject[] bubbles;
	
	private bool spawned;
	private float timer;
	private int index;
	
	void Start(){
		timer	= Random.Range(0.0f,1.0f);
		bubbles = new GameObject[maxSpawns];
	}
	
	void Update () {
		if( timer < Time.time ){
			timer	= Time.time + Random.Range(0.1f,spawnRate);
			index	= 0;
			spawned	= false;
			while(!spawned && index < maxSpawns){
				if( !bubbles[index] ){
					spawned = true;
					bubbles[index] = (GameObject)Instantiate(bubble,new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity);
					bubbles[index].GetComponent<Bubble>().SetLifeSpan(lifeSpan);
					bubbles[index].transform.localEulerAngles = new Vector3(0.0f,0.0f,Random.Range(0.0f,360.0f));
				}
				index++;
			}
		}
	}
	
	public void OnDrawGizmos(){
		Gizmos.color	= Color.blue;
		Gizmos.DrawWireSphere(transform.position,0.25f);
	}
}
