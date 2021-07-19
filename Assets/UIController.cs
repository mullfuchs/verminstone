using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public GameObject HealthBarObject;
    public GameObject PowerBarObject;
    public GameObject XPBarObject;
	public GameObject XPText;

    public GameObject VStoneAmountText;
    public GameObject GameStatusText;

	public GameObject NPCHealthPrefab;
	public GameObject NPCPrefabContainer;

	private FillableBarController HealthBar;
    private FillableBarController PowerBar;
    private FillableBarController XPBar;


    // Use this for initialization
    void Start () {
		GameObject player = GameObject.Find ("Player");
		player.GetComponent<PowerObject> ().resetUIObject (gameObject);
        assignBarObject(HealthBar, HealthBarObject);
        assignBarObject(PowerBar, PowerBarObject);
        assignBarObject(XPBar, XPBarObject); 
		SetupNPCCards ();
	}

    void assignBarObject(FillableBarController bar, GameObject barObject)
    {
        if(barObject != null)
        {
            bar = barObject.GetComponent<FillableBarController>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateBar(GameObject barToUpdate, float amount)
    {
        if(barToUpdate != null)
        {
            barToUpdate.GetComponent<FillableBarController>().UpdateCurrentValue(amount);
        }
    }

    public void updateBarMaxValue(GameObject barToUpdate, float amount)
    {  
        if(barToUpdate != null)
        {
            barToUpdate.GetComponent<FillableBarController>().SetMaxValue(amount);
        }
    }

    public void updateText(GameObject textToUpdate, string value)
    {
        if(textToUpdate != null)
        {
            textToUpdate.GetComponent<Text>().text = value;
        }
    }

	public void SetupNPCCards(){
		//get all the npcs
		//make a card prefab for each one
		//add it to the card prefab holder
		//add a reference to the health which updates itself?
		//fuck the references aren't active on scene activation
		GameObject[] npcs = GameObject.Find("Player").GetComponent<NPCTeamHandler>().NPCMiners;

		for (int i = 0; i < npcs.Length; i++) {
			GameObject uiCard = Instantiate (NPCHealthPrefab, NPCPrefabContainer.transform);
			uiCard.GetComponentInChildren<FillableBarController> ().SetMaxValue( npcs [i].GetComponent<health> ().maxHealth );
			uiCard.GetComponentInChildren<FillableBarController> ().UpdateCurrentValue( npcs [i].GetComponent<health> ().healthPoints);
			uiCard.transform.Find("Portrait").GetComponent<Image>().sprite = npcs [i].GetComponent<NPCstats> ().DialogPortraits [0];
			SetUpInventoryIcons (npcs [i], uiCard);

			npcs [i].GetComponent<health> ().SetTrackingUIElement ( uiCard.GetComponentInChildren<FillableBarController> () );
			uiCard.SetActive (true);
		}
	}

    public void ResetNPCCards()
    {
        foreach(Transform child in NPCPrefabContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        //clear existing cards
        //make new ones via setup
        SetupNPCCards();
    }

	private void SetUpInventoryIcons(GameObject npc, GameObject card){
		NPCInventory inv = npc.GetComponent<NPCInventory> ();

		if (inv.ObjectHeldInHands != null) {
			card.transform.Find ("handIcon").GetComponent<Image> ().sprite = inv.ObjectHeldInHands.GetComponent<EquippableItem> ().icon;
		}
		if (inv.ObjectOnBack != null) {
			card.transform.Find ("backIcon").GetComponent<Image> ().sprite = inv.ObjectOnBack.GetComponent<EquippableItem> ().icon;
		}
		if (inv.ObjectOnHead != null) {
			card.transform.Find ("headIcon").GetComponent<Image> ().sprite = inv.ObjectOnHead.GetComponent<EquippableItem>().icon;
		}
	}



}
