using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPlayerAndNPCsToZoneOnEnter : MonoBehaviour {

	public GameObject TargetZone;
	public Transform point;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player" && TargetZone != null) {
			StartCoroutine ("warpToZOne");
		}
	}

	IEnumerator warpToZOne(){
		//get objects
		GameObject player = GameObject.Find("Player");
		GameObject[] npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");

		//fade camera out
		GameObject.Find("MultipurposeCameraRig").GetComponent<CameraFade>().StartFade(Color.black, 2.0f);
		//move em
		yield return new WaitForSeconds(2.0f);
		PlayerAndNPCSpawner destinationObject = TargetZone.GetComponent<PlayerAndNPCSpawner>();
		destinationObject.setPoint (TargetZone.GetComponent<WarpPlayerAndNPCsToZoneOnEnter>().point.position);
		destinationObject.addPlayerAndNPCs (player, npcs);
		destinationObject.placePlayerAndNPCs ();

		GameObject.Find("MultipurposeCameraRig").GetComponent<CameraFade>().StartFade(Color.clear, 2.0f);
		//fade camera in

	}
}
