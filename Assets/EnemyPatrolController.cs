using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolController : MonoBehaviour {

	public Transform[] targets;

	public float distanceThreshold = 0.5f;

	private Transform currentTarget;
	private int targetIndex;

	// Use this for initialization
	void Start () {
		if (targets.Length >= 2) {
			SetupPatrol (targets);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(currentTarget.position, gameObject.transform.position) <= distanceThreshold){
			currentTarget = getNewTarget ();
			gameObject.GetComponent<AIBugController> ().setTargetTransform (currentTarget);
		}
	}

	void SetupPatrol(Transform[] _targets){
		targets = _targets;
		gameObject.GetComponent<AIBugController> ().setTargetTransform (targets [0]);
		currentTarget = (targets [0]);
		targetIndex = 0;
	}

	Transform getNewTarget(){
		if (targetIndex == targets.Length) {
			targetIndex = 0;
		} else {
			targetIndex += 1;
		}
		return targets [targetIndex];
	}
}
