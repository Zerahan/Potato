﻿using UnityEngine;
using System.Collections;

public class BounceGoo : MonoBehaviour {
	public void OnCollisionStay(Collision collision){
		collision.rigidbody.velocity = collision.rigidbody.velocity + (transform.up * 8f);
	}
}
