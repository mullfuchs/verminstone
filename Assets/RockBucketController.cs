using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBucketController : MonoBehaviour {

	private int numberOfCarriersVisited = 0;
	private int numberOfStones = 0;
	private float totalWeightOfStones = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "WorkerNPC") {
			totalWeightOfStones += other.GetComponent<AIStateMachine> ().GetVerminStoneAmount ();
			numberOfStones++;
			numberOfCarriersVisited++;
		}
	}

	public int getNumberOfVisitedCarriers(){
		return numberOfCarriersVisited;
	}

	public float getTotalWeightOfStones(){
		return totalWeightOfStones;
	}
}

