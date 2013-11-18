using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {
	public AudioClip audio		;
	
	private AudioSource sound;
	private	float	startSpeed	= 8;

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
		if (rigidbody.velocity.magnitude > startSpeed) {
			sound.volume = Mathf.Min(1f, Mathf.Pow((rigidbody.velocity.magnitude-startSpeed)/50,2));
		}else{
			sound.volume = 0;
		}
		//*/
	}
}
