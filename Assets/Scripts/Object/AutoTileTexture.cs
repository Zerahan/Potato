#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class AutoTileTexture : MonoBehaviour {
	public float scale = 0.25f;
	
	// Use this for initialization
	void Start () {
	 
	}
	 
	// Update is called once per frame
	void Update () {
	 
	}
	void OnDrawGizmos() {
	 	this.gameObject.GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex",new Vector2(scale * this.gameObject.transform.lossyScale.x, scale * this.gameObject.transform.lossyScale.y));
	}
}
#endif