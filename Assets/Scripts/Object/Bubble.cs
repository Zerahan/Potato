using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {
	public float popMaxTime	= 10;
	public ParticleSystem particle;
	
	public AudioClip[] sounds;
	
	private float popTime;
	private float growTime	= 4;
	private float size;
	
	// Use this for initialization
	void Start () {
		size		= Random.Range(0.5f,2f);
		popTime		= Time.time + popMaxTime*Mathf.Pow(Random.Range(0f,1f),2);
		transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update(){
		if(growTime >= 0){
			growTime -= Time.deltaTime;
			transform.localScale = new Vector3(size,size,size) * (1-(growTime/4));
			Light light = transform.GetComponent<Light>();
			if(light){
				light.range = (1.25f + 2 * (size/2f)) * (1-(growTime/4));
			}
		}
		if(rigidbody.velocity.y > 5 + 10 * (1-(size/2f))){
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,5 + 10 * (1-(size/2f)),rigidbody.velocity.z);
		}
		if( popTime < Time.time ){
			PopBubble();
		}
	}
	
	void FixedUpdate(){
		rigidbody.AddForce(Vector3.up * Physics.gravity.magnitude * (0.5f + 0.5f*(1-(size/2f)) ) * rigidbody.mass);
	}
	
	public void OnCollisionEnter( Collision collision ){
		if( collision.relativeVelocity.magnitude > 8 ){
			PopBubble();
			Buff buff = GetComponent<Buff>();
			if(buff){
				buff.ApplyBuff(collision);
			}
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
		Destroy(gameObject);
	}
}
