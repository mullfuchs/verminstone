using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAndNPCSpawner : MonoBehaviour {

	public Queue<GameObject> NpcQueue;

	Vector3 point;

	// Use this for initialization
	void Awake () {
		NpcQueue = new Queue<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setPoint(Vector3 _point){
		point = _point;
	}

	public void addNPC(GameObject _npc){
		Debug.Log (_npc.transform.position);
		NpcQueue.Enqueue (_npc);
		_npc.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = false;
		_npc.transform.position = gameObject.transform.position;
	}

	public void placeNextNPC(){
		if (NpcQueue.Count >= 1) {
			Debug.Log ("spawned an NPC");
			GameObject nextNPC = (GameObject)NpcQueue.Dequeue ();
			nextNPC.SetActive (true);

			nextNPC.transform.position = point;
			nextNPC.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = true;

		} 
	}

	void OnTriggerExit(Collider other){
//		if (other.tag == "Player" || other.tag == "Carrier" || other.tag == "Miner") {
//			Debug.Log ("Stepped off exit trigger");
//			Debug.Log (NpcQueue.Count);
//
//		} 
	}

}
