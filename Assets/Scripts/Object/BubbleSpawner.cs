using UnityEngine;
using System.Collections;

public class BubbleSpawner : MonoBehaviour {
	
	public float spawnRate	= 0.5f;
	public int maxSpawns	= 10;
	public float minAge	= 2f;
	public float maxAge	= 30f;
	
	
	public GameObject bubble;
	
	private GameObject[] bubbles;
	
	private bool spawned;
	private float timer;
	private int index;
	private float lifespan;
	private float ageRange;
	
	void Start(){
		timer		= Random.Range(spawnRate * 0.67f,spawnRate * 1.5f);
		bubbles 	= new GameObject[maxSpawns];
		ageRange	= maxAge - minAge;
	}
	
	void Update () {
		if( timer < Time.time ){
			timer	= Time.time + Random.Range(spawnRate * 0.67f,spawnRate * 1.5f);
			index	= 0;
			spawned	= false;
			while(!spawned && index < maxSpawns){
				if( !bubbles[index] ){
					spawned = true;
					
					lifespan = Random.Range(minAge,maxAge);
					lifespan = Mathf.Pow((lifespan - minAge) / ageRange, 2);
					lifespan = minAge + ageRange * lifespan;
					
					bubbles[index] = (GameObject)Instantiate(bubble,new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity);
					//bubbles[index].GetComponent<Bubble>().maxAge = lifespan;
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
