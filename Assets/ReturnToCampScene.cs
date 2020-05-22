 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToCampScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Yarn.Unity.YarnCommand("returnToCampScene")]
	public void loadCamp(){
		print ("returning to camp?");
		GameObject.Find ("CampEventController").GetComponent<CampEventController> ().StartDay ();
		SceneManager.LoadScene ("Camp");

	}
}
