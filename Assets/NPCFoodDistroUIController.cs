using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCFoodDistroUIController : MonoBehaviour {
	//gets a list of npcs
	//creates a card for each one
		// contains name, health, energy level, and like mood?
	//get total amount of food for day
	public GameObject CardPrefab;

	public GameObject cardParent;

	private GameObject[] NPCs;

	private List<GameObject> NPCCards;

    public GameObject ExitButton;

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
        playerUICard.GetComponent<NPCFoodCardController>().parentFoodDistroObject = this;
		NPCCards.Add (playerUICard);
		playerUICard.GetComponent<NPCFoodCardController> ().updateFoodCardUI ();
        NPCCards[0].GetComponentInChildren<UnityEngine.UI.Button>().Select();

        SetUpControllerNavigation();
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
            ExitButton.GetComponent<Button>().Select();
		}
	}

    public void SetUpControllerNavigation()
    {
        Navigation exitButtonNav = ExitButton.GetComponent<Button>().navigation;
        exitButtonNav.selectOnDown = NPCCards[0].GetComponent<NPCFoodCardController>().healButton;
        exitButtonNav.selectOnUp = NPCCards[NPCCards.Count - 1].GetComponent<NPCFoodCardController>().healButton;
        ExitButton.GetComponent<Button>().navigation = exitButtonNav;

        for (int cardIndex = 0; cardIndex < NPCCards.Count; cardIndex++)
        {
            Navigation buttonNav = NPCCards[cardIndex].GetComponent<NPCFoodCardController>().healButton.navigation;
            buttonNav.selectOnUp = getSelectOnUp(NPCCards, cardIndex, ExitButton);
            buttonNav.selectOnDown = getSelectOnDown(NPCCards, cardIndex, ExitButton);
            NPCCards[cardIndex].GetComponent<NPCFoodCardController>().healButton.navigation = buttonNav;
        }
    }

    Button getSelectOnUp(List<GameObject> buttonStack, int currentIndex, GameObject DefaultButton)
    {
        //given the index, give the button above it in order, if it's the first, set it to the default button
        if (currentIndex == 0)
        {
            if (DefaultButton.GetComponent<Button>() != null)
            {
                return DefaultButton.GetComponent<Button>();
            }
        }
        else
        {
            return buttonStack[currentIndex - 1].GetComponentInChildren<Button>();
        }
        return null;
    }

    Button getSelectOnDown(List<GameObject> buttonStack, int currentIndex, GameObject DefaultButton)
    {
        //given index, give button below it in order, if it's the last, set it to default button;
        if (currentIndex == (buttonStack.Count - 1) && DefaultButton.GetComponent<Button>() != null)
        {
            return DefaultButton.GetComponent<Button>();
        }
        else
        {
            return buttonStack[currentIndex + 1].GetComponentInChildren<Button>();
        }
        return null;
    }

}
