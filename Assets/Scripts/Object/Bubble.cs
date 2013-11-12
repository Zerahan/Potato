using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {
	public float popMaxTime	= 10;
	public ParticleSystem particle;
	
	public AudioClip[] sounds;
	
	private float popTime;
	private float growTime	= 4;
	
	
	private float size;
	private float maxSize;
	
	private bool isDestroyed;
	
	// Use this for initialization
	void Start (){
		maxSize		= Random.Range(0.5f,2f);
		growTime	= 3*(size/2f) + 1;
		popTime		= growTime + Time.time + popMaxTime*Mathf.Pow(1-(size/2.5f)+Random.Range(0f,0.25f),2);
		transform.localScale	= Vector3.zero;
		rigidbody.mass			= (size/2.5f) + 1;
	}
	
	// Update is called once per frame
	void Update(){
		size	+= Time.deltaTime;
		transform.localScale( new Vector3(1,1,1) * Mathf.Log(size) );
		/*
		if(growTime >= 0){
			growTime -= Time.deltaTime;
			transform.localScale = new Vector3(size,size,size) * (1-(growTime/4));
			Light light = transform.GetComponent<Light>();
			if(light){
				light.range = (1.25f + 2 * (size/2f)) * (1-(growTime/4));
			}
		}
		if(rigidbody.velocity.y > 5 + 10 * (1-(size/2f))){
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,10,rigidbody.velocity.z);
		}
		if( popTime < Time.time ){
			PopBubble();
		}
		//*/
	}
	
	void FixedUpdate(){
		rigidbody.AddForce(Vector3.up * Physics.gravity.magnitude * (1f));
	}
	
	public bool IsDestroyed(){
		return isDestroyed;
	}
	
	public void OnCollisionEnter( Collision collision ){
		growTime	= 0.5f;
		if( collision.relativeVelocity.magnitude > 8 ){
			isDestroyed = true;
			PopBubble();
		}else{
			rigidbody.velocity += collision.contacts[0].normal * 1.5f;
		}
	}
	
	public void OnCollisionStay( Collision collision ){
		rigidbody.velocity += collision.contacts[0].normal * 1f;
	}
	
	public void SetLifeSpan(float span){
		popMaxTime = span;
	}
	
	private void PopBubble(){
		ParticleSystem p	= (ParticleSystem)Instantiate(particle,transform.position,transform.localRotation);
		if(sounds.Length > 0){
			AudioSource sound	= p.GetComponent<AudioSource>();
			if(sound){
				sound.clip		= sounds[Random.Range(0,sounds.Length-1)];
				sound.Play();
			}
		}
		isDestroyed = true;
		Destroy(gameObject);
	}
}
