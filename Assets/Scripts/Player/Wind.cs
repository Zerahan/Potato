﻿using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {
	public AudioClip audio;
	
	private AudioSource sound;
	private float velocity;

	// Use this for initialization
	void Start () {
		sound	= Camera.main.GetComponent<AudioSource>();
		sound.clip		= audio;
		sound.volume	= 0;
		sound.pitch		= 0.8f;
		sound.Play();
	}
	
	// Update is called once per frame
	void Update () {
		//*
		velocity = rigidbody.velocity.magnitude;
		if (velocity > 0.5) {
			sound.volume = rigidbody.velocity.magnitude / 20;
		}else{
			sound.volume = 0;
		}
		//*/
	}
}
