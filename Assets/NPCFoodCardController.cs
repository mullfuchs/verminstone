using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCFoodCardController : MonoBehaviour {

	public NPCFoodDistroUIController parentFoodDistroObject;

	private GameObject associatedNPC;
	private health npcHealth;

	public GameObject nameText;
	public GameObject moodText;
	public GameObject healthBar;
	public GameObject staminaBar;

	// Use this for initialization
	void Start () {
		//Get the ui elements attached to this thing
//		nameText = gameObject.transform.Find("NPCName").gameObject;
//		moodText = gameObject.transform.Find ("NPCMoodStatus").gameObject;
//		healthBar = gameObject.transform.Find ("Healthbar").gameObject;
//		staminaBar = gameObject.transform.Find ("StaminaBar").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateFoodCardUI(){
		NPCstats stats = associatedNPC.GetComponent<NPCstats> ();
		print ("stats name" + stats.name);
		healthBar.GetComponent<FillableBarController> ().SetMaxValue (stats.maxHealth);
		healthBar.GetComponent<FillableBarController> ().UpdateCurrentValue (stats.health);

		staminaBar.GetComponent<FillableBarController> ().SetMaxValue (stats.maxStamina);
		staminaBar.GetComponent<FillableBarController> ().UpdateCurrentValue (stats.stamina);


        healthBar.GetComponent<Image>().fillAmount = (npcHealth.healthPoints / npcHealth.maxHealth);
        staminaBar.GetComponent<Image>().fillAmount = (stats.stamina / stats.maxStamina);

		nameText.GetComponent<Text> ().text = stats.name;
		moodText.GetComponent<Text> ().text = stats.mood;
	}

	public void assignNPCtoCard(GameObject npc){
		associatedNPC = npc;
		npcHealth = npc.GetComponent<health> ();
		updateFoodCardUI ();
	}

	public void givePortionToNPC(){
		//check remaining amount
		if (parentFoodDistroObject.ExtraFoodAmount > 0) {
			NPCstats stats = associatedNPC.GetComponent<NPCstats> ();
			stats.health = 100;
			npcHealth.healthPoints = npcHealth.maxHealth;
			updateFoodCardUI ();
			parentFoodDistroObject.updateAllCards ();
		}


	}
}
