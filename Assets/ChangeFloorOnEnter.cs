using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFloorOnEnter : MonoBehaviour {

	public bool descending;

	GameObject caveManager;

	public bool CanSwapLevels = false;

	// Use this for initialization
	void Start () {
		caveManager = GameObject.Find ("CaveManager");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player" && CanSwapLevels) {
			if (descending) {
				caveManager.GetComponent<CaveManager> ().DescendToLowerFloor ();
				CanSwapLevels = false;
			} else {
				caveManager.GetComponent<CaveManager> ().AscendToUpperFloor ();
				CanSwapLevels = false;
			}
		}
	}
}
