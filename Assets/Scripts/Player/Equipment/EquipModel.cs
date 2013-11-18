using UnityEngine;
using System.Collections;
using GameSettings;

public class EquipModel : MonoBehaviour {
	public EquipmentSlot slot	= EquipmentSlot.None;
	
	void Awake(){
		if(slot != EquipmentSlot.None){
			transform.root.GetComponent<PlayerController>().RegisterEquipmentModel(gameObject, slot);
		}else{
			Debug.LogWarning("Equipment slot not set for " + gameObject.name + "!");
		}
	}
}
