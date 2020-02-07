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

	public int ExtraFoodAmount = 0;
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
		ExtraFoodAmount = GameObject.Find ("CampEventController").GetComponent<VStoneEconomyObject> ().getExtraPortionsBasedOnVstone ();
		cardParent.transform.Find ("ExtraFoodDisplay").GetComponent<UnityEngine.UI.Text> ().text = "Extra Portions: " + ExtraFoodAmount;
        NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        foreach (GameObject g in NPCs) {
			GameObject uiCard = Instantiate (CardPrefab, cardParent.transform, false);
			uiCard.SetActive (true);
			uiCard.GetComponent<NPCFoodCardController> ().assignNPCtoCard (g);
			uiCard.GetComponent<NPCFoodCardController> ().parentFoodDistroObject = this;
			NPCCards.Add (uiCard);
            uiCard.GetComponent<NPCFoodCardController>().updateFoodCardUI();
        }

		GameObject playerUICard = Instantiate (CardPrefab, cardParent.transform, false);
		playerUICard.SetActive (true);
		playerUICard.GetComponent<NPCFoodCardController> ().assignNPCtoCard (GameObject.Find ("Player"));
		NPCCards.Add (playerUICard);
		playerUICard.GetComponent<NPCFoodCardController> ().updateFoodCardUI ();
	}

	public void cleanUpFoodUI(){
		foreach (GameObject x in NPCCards) {
			Destroy (x);
		}
		cardParent.SetActive (false);
	}

	public void updateAllCards(){
		ExtraFoodAmount -= 1;
		cardParent.transform.Find ("ExtraFoodDisplay").GetComponent<UnityEngine.UI.Text> ().text = "Extra Portions: " + ExtraFoodAmount;
		if (ExtraFoodAmount <= 0) {
			foreach (GameObject x in NPCCards) {
				GameObject button = x.transform.Find ("Button").gameObject;
				button.GetComponent<UnityEngine.UI.Button> ().interactable = false;
			}
		}
	}
}
