﻿using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {
	public float popMaxTime	= 10;
	public ParticleSystem particle;
	
	public AudioClip[] sounds;
	
	private float growTime;
	private float popTime;
	private float size;
	
	private bool isDestroyed;
	
	// Use this for initialization
	void Start (){
		size		= Random.Range(0.5f,2.0f);
		growTime	= 3*(size/2f) + 1;
		popTime	= growTime + Time.time + popMaxTime*Mathf.Pow(1-(size/2.5f)+Random.Range(0f,0.25f),2);
		transform.localScale	= Vector3.zero;
		rigidbody.mass			= 2;
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
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,10,rigidbody.velocity.z);
		}
		if( popTime < Time.time ){
			PopBubble();
		}
	}
	
	void FixedUpdate(){
		rigidbody.AddForce(Vector3.up * Physics.gravity.magnitude * (1f));
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
