using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour {

	public string NameOfQuest;

	// Use this for initialization
	void Start () {
		
	}

	void Awake(){
		print ("Quest object for " + NameOfQuest + " active");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
