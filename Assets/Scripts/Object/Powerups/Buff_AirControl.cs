using UnityEngine;
using System.Collections;

public class Buff_AirControl : Buff {
	public virtual float GetStrength() { return 1.0f; }
	public override void ApplyBuff(Collision collision){
		if( collision.transform.GetComponent<UserInput>() ){
			collision.transform.GetComponent<UserInput>().SetControlMultiplier(GetStrength());
		}
	}
}
