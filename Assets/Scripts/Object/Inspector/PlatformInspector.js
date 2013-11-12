#pragma strict

/*
@CustomEditor(Platform_v2)

class PlatformInspector extends Editor{
	function OnSceneGUI(){
		var tar : = target
		for(var i = 0; i < target.path.Length; i++){
			target.path[i]	= Handles.PositionHandle(target.path[i],Quaternion.identity);
		}
		if(GUI.changed){
			EditorUtility.SetDirty(target);
		}
	}
}
//*/