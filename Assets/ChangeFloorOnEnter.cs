using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFloorOnEnter : MonoBehaviour {

	public bool descending;

	GameObject caveManager;

	public bool CanSwapLevels = false;

	private int floorlevel = 1;
	private GameObject EnemyTeamHandler;
	private CameraFade cameraFade;

	// Use this for initialization
	void Start () {
		caveManager = GameObject.Find ("CaveManager");
		cameraFade = GameObject.Find ("MultipurposeCameraRig").GetComponent<CameraFade> ();
		floorlevel = 1;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay(Collider other){
		
		if (other.gameObject.tag == "Player" && CanSwapLevels && Input.GetKeyDown(KeyCode.Space)) {
			cameraFade.StartFade (Color.black, 2.0f);
			if (descending) {
				caveManager.GetComponent<CaveManager> ().DescendToLowerFloor ();
				floorlevel += 1;
				EnemyTeamHandler = GameObject.Find ("EnemyNPCHandler");
				EnemyTeamHandler.GetComponent<EnemyTeamHandler> ().floorLevel = floorlevel;
				print ("floor level: " + floorlevel);
				CanSwapLevels = false;
			} else {
				caveManager.GetComponent<CaveManager> ().AscendToUpperFloor ();
				floorlevel -= 1;
				EnemyTeamHandler = GameObject.Find ("EnemyNPCHandler");
				EnemyTeamHandler.GetComponent<EnemyTeamHandler> ().floorLevel = floorlevel;
				print ("floor level: " + floorlevel);
				CanSwapLevels = false;
			}
			cameraFade.StartFade (Color.clear, 2.0f);
		}

	}

}
