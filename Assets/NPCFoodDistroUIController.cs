using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFoodDistroUIController : MonoBehaviour {
	//gets a list of npcs
	//creates a card for each one
		// contains name, health, energy level, and like mood?
	//get total amount of food for day
	public GameObject CardPrefab;

	public GameObject cardParent;

	private GameObject[] NPCs;

	// Use this for initialization
	void Start () {
		NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateAndDisplayNPCcards(){
		cardParent.SetActive (true);
		foreach (GameObject g in NPCs) {
			GameObject uiCard = Instantiate (CardPrefab, cardParent.transform, false);
			uiCard.SetActive (true);
		}
	}
}
