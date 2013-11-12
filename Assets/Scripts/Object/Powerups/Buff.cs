using UnityEngine;
using System.Collections;

public class Buff : MonoBehaviour {
	public virtual float GetStrength() { return 1.0f; }
	public virtual void ApplyBuff(Collision collision){}
}
