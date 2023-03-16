using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {
	public ParticleSystem	particle;
	public AudioClip[]		sounds;
	public float			maxMass = 1;
	public float			maxAge { get; set; }
	private float			age = 0;
	private float			scale = 0;
	//private float			sizeMag = 0;
	private float			maxSize = 1f;
	private float			maxLightRange = 10;
	private bool			isDestroyed;
 
	//private static Vector3 invalidPosition	= new Vector3(-99999, -99999, -99999 );
	//public  static Vector3 InvalidPosition	{ get{return invalidPosition;}}
	
	// Use this for initialization
	void Start (){
		transform.localScale	= Vector3.zero;
	}
	
	// Update is called once per frame
	void Update(){
		age = age + Time.deltaTime;
		
		if (age >= maxAge) PopBubble();
		
		scale = Mathf.Sqrt (age / maxAge) + 0.001f;
		
		
		transform.localScale = scale * maxSize * Vector3.one;		
		transform.GetComponent<Light>().range = scale * maxLightRange + 1.25f;
		GetComponent<Rigidbody>().drag = scale * 0.5f + 0.1f;
	}
	
	void FixedUpdate(){
		GetComponent<Rigidbody>().AddForce(Vector3.up * Physics.gravity.magnitude * (0.25f));
	}
	
	public bool IsDestroyed(){
		return isDestroyed;
	}
	
	public void OnCollisionEnter( Collision collision ){
		//growTime	= 0.5f;
		if( collision.relativeVelocity.magnitude > 8 ){
			isDestroyed = true;
			PopBubble();
		}else{
			GetComponent<Rigidbody>().velocity += collision.contacts[0].normal * 1.5f;
		}
	}
	
	public void OnCollisionStay( Collision collision ){
		GetComponent<Rigidbody>().velocity += collision.contacts[0].normal * 1f;
	}
	
	private void PopBubble(){
		ParticleSystem p	= (ParticleSystem)Instantiate(particle,transform.position,transform.localRotation);
		if(sounds.Length > 0){
			AudioSource sound	= p.GetComponent<AudioSource>();
			if(sound){
				sound.clip		= sounds[Random.Range(0,sounds.Length-1)];
				sound.volume	= 1f;
				sound.Play();
			}
		}
		isDestroyed = true;
		Destroy(gameObject);
	}
}
