using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {
	public AudioClip audio;
	
	private AudioSource sound;
	private float velocity;

	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource>();
		sound.clip = audio;
		sound.volume = 1;
		//sound.Play();
	}
	
	// Update is called once per frame
	void Update () {
		/*
		velocity = rigidbody.velocity.magnitude;
		if (velocity > 0.5) {
			sound.volume = rigidbody.velocity.magnitude / 10;
		}else{
			sound.volume = 0;
		}
		*/
	}
}
