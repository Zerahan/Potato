using UnityEngine;
using System.Collections;

public class Equipment : MonoBehaviour {
	public		GUISkin		targetIcon		;
	protected	Vector3		targetPoint		;
	
	protected	string		name	= "Generic Equipment";
	public		string		Name	{ get{return name;} protected set{name	= value;} }
	
	private		Player player		;
	protected	Player User			{ get{return player;} }
	
	protected	bool isEnabled		= true;
	public		bool IsEnabled		{ get{return isEnabled;} set{isEnabled = value;} }
	
	protected	bool canAct			= true;
	public		bool CanAct			{ get{return canAct;} }
	
	protected	bool isActive		= false;
	public		bool IsActive		{ get{return isActive;} protected set{isActive = value;} }
	
	protected	bool lastActState	;
	protected	bool nextActState	;
	public		bool ActState		{ get{ return nextActState; } protected set{ nextActState = value; } }
	
	public enum Action{
		Enable,
		Disable,
		Single
	}
	
	public virtual void Start(){
		player	= transform.root.GetComponentInChildren<Player>();
	}
	
	public virtual void Update(){
		if( nextActState ){
			if(lastActState){
				OnActiveStay();
			}else{
				OnActiveStart();
			}
		}else if(lastActState){
			OnActiveEnd();
		}
		lastActState	= nextActState;
	}
	
	public void AddToPlayer(){
		player	= transform.root.GetComponentInChildren<Player>();
	}
	
	public virtual void DoAction(Action act){
		switch(act){
			case Action.Enable:
				if( isEnabled ){
					ActState	= true;
				}
				break;
			case Action.Disable:
				IsActive	= false;
				ActState	= false;
				break;
			case Action.Single:
				if( isEnabled ){
					SingleAction();
				}
				break;
			default:
				break;
		}
	}
	
	protected virtual void SingleAction(){
	}
	
	protected virtual void OnActiveStart(){
	}
	
	protected virtual void OnActiveStay(){
	}
	
	protected virtual void OnActiveEnd(){
	}
	
	protected virtual void Disable(){
	}
}
