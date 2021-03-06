﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAndNPCSpawner : MonoBehaviour {

	public Queue<GameObject> NpcQueue;

	private GameObject[] NPCs;
	private GameObject Player;

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

	public void addPlayerAndNPCs(GameObject player, GameObject[] npcs){
		Player = player;
		NPCs = npcs;
	}

	public void addNPC(GameObject _npc){
		Debug.Log (_npc.transform.position);
		NpcQueue.Enqueue (_npc);
		_npc.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = false;
		_npc.transform.position = gameObject.transform.position;
        _npc.SetActive(false);
	}

	public void placeNextNPC(){
		Debug.Log ("attempting to spawn npc");
		if (NpcQueue.Count >= 1) {
			Debug.Log ("spawned an NPC");
			GameObject nextNPC = (GameObject)NpcQueue.Dequeue ();

			nextNPC.SetActive (true);
			nextNPC.transform.position = point + new Vector3(0, 1.5f, 0);
			nextNPC.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = true;

		} 
	}

	public void placeAllNPCs(GameObject[] exitPlacementObjects){
        foreach(GameObject placementObject in exitPlacementObjects)
        {
            if(NpcQueue.Count > 0)
            {
                GameObject nextNPC = (GameObject)NpcQueue.Dequeue(); // removed
                nextNPC.SetActive(true);
                nextNPC.transform.position = placementObject.transform.position;
                nextNPC.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                nextNPC.GetComponent<AIStateMachine>().SetNPCTarget(placementObject);
            }
            else
            {
                return;
            }
        }

        /*
        int index = 0;
		while(NpcQueue.Count > 0)
		{
			GameObject nextNPC = (GameObject)NpcQueue.Dequeue(); // removed
            nextNPC.SetActive (true);
            nextNPC.transform.position = exitPlacementObjects[index].transform.position;
			nextNPC.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = true;
            if (index + 1 != exitPlacementObjects.Length)
                index++;
            else
                index = 0;
		}
        */
	}

	public void placePlayerAndNPCs(){
		float offsetX = 0.0f;
		GameObject.Find ("MultipurposeCameraRig").transform.position = point;
		if (Player != null) {
			Player.transform.position = point;
		}
		for (int i = 0; i < NPCs.Length; i++) {

			//NPCs [i].SetActive (false);
			Vector3 npcWarpPosition = point + new Vector3(0 + offsetX, 0.05f, -1.0f);
			NPCs [i].GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().agent.Warp ( npcWarpPosition );
			//NPCs [i].transform.position = point;
			offsetX += 1.5f;
			//NPCs [i].SetActive (true);
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
