using UnityEngine;
using System.Collections;

public class BounceGoo : MonoBehaviour {
	public void OnCollisionStay(Collision collision){
<<<<<<< HEAD
		collision.rigidbody.velocity = collision.rigidbody.velocity + (transform.up * 4f);
=======
		collision.rigidbody.velocity = collision.rigidbody.velocity + (transform.up * 8f);
>>>>>>> origin/Thal
	}
}
