#pragma strict

var path:Vector3[]	= new Vector3[1];
var sumDist:float[]	= new float[1];

var totalDistance:float;
var curDistance:float;
var curTarget:int;

function Start () {
	totalDistance = 0;
	sumDist	= new float[path.Length];
	for(var i = 0; i < path.Length; i++){
		if(i != 0){
			totalDistance += (path[i]-path[i-1]).magnitude;
			sumDist[i]	= (path[i]-path[i-1]).magnitude + sumDist[i-1];
		}
	}
}

function Update () {
	
	if(curDistance >= sumDist[curTarget]){
		curTarget++;
		if(curTarget >= path.Length){
			curTarget	= 0;
			curDistance	= 0;
		}
	}
}

function OnDrawGizmos(){
	for(var i = 0; i < path.Length; i++){
		Gizmos.color	= Color.yellow;
		Gizmos.DrawWireCube(path[i],Vector3(0.5,0.5,0.5));
		if(i != 0){
			Gizmos.DrawLine(path[i-1],path[i]);
		}else{
			Gizmos.DrawLine(path[path.Length-1],path[i]);
		}
	}
}