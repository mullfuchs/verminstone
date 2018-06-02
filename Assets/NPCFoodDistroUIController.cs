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

	private List<GameObject> NPCCards;

	// Use this for initialization
	void Start () {
		NPCCards = new List<GameObject> ();
		NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateAndDisplayNPCcards(){
		cardParent.SetActive (true);
        NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        foreach (GameObject g in NPCs) {
			GameObject uiCard = Instantiate (CardPrefab, cardParent.transform, false);
			uiCard.SetActive (true);
			uiCard.GetComponent<NPCFoodCardController> ().assignNPCtoCard (g);
            
			NPCCards.Add (uiCard);
            uiCard.GetComponent<NPCFoodCardController>().updateFoodCardUI();
        }
	}

	public void cleanUpFoodUI(){
		foreach (GameObject x in NPCCards) {
			Destroy (x);
		}
		cardParent.SetActive (false);
	}
}
