using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNPCEscapeFlags : MonoBehaviour {

	public string[] npcsThatCanEscape;

	// Use this for initialization
	void Start () {
		
	}

	void Awake(){
		foreach (string name in npcsThatCanEscape) {
			GameObject n = GameObject.Find (name);
			n.GetComponent<NPCOverworldController> ().isEscaping = true;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
