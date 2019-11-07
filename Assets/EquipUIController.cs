using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipUIController : MonoBehaviour {

    public GameObject ItemCardPrefab;

    public GameObject ItemCardParent;

    public GameObject NPCCardPrefab;

    public GameObject NPCCardParent;

    private GameObject[] NPCs;

    private GameObject[] Items;

    private List<GameObject> ItemCards;

	private List<GameObject> NPCCards;

    private GameObject currentItem;

    // Use this for initialization
    void Start()
    {
        NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        Items = GameObject.Find("CampEventController").GetComponent<CampInventoryController>().items;
        currentItem = null;
        ItemCards = new List<GameObject>();
		NPCCards = new List<GameObject> ();
		//Items = 
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void CreateAndDisplayNPCcards()
    {
        NPCCardParent.SetActive(true);
        NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        foreach (GameObject g in NPCs)
        {
            GameObject uiCard = Instantiate(NPCCardPrefab, NPCCardParent.transform, false);
            uiCard.SetActive(true);
            uiCard.GetComponent<NPCEquipCardController>().assignNPCtoCard(g);
			NPCCards.Add(uiCard);
        }
    }

    public void CreateAndDisplayItemCards()
    {
        Items = GameObject.Find("CampEventController").GetComponent<CampInventoryController>().items;
        ItemCardParent.SetActive(true);
        foreach (GameObject g in Items)
        {
            GameObject uiCard = Instantiate(ItemCardPrefab, ItemCardParent.transform, false);
            uiCard.SetActive(true);
            uiCard.GetComponent<ItemEquipCardController>().assignItemtoCard(g);
			ItemCards.Add (uiCard);
        }
    }

	public void cleanUpItemAndNPCCards(){
		//destroy cards

		foreach (GameObject x in ItemCards) {
            DestroyObject(x);
           // Destroy (x);
		}
		foreach (GameObject x in NPCCards) {
			DestroyObject (x);
		}
		ItemCards.Clear();
		NPCCards.Clear ();
		ItemCardParent.SetActive(false);
		NPCCardParent.SetActive(false);
	}

    public void SetCurrentItemAndEnableButtons(GameObject item)
    {
        //resetAllButtons();
        currentItem = item;
		foreach(GameObject g in NPCCards)
        {
            g.GetComponent<NPCEquipCardController>().EquipButton.GetComponent<Button>().interactable = true;

            if (item.GetComponent<EquippableItem> ().ForBack) {
				g.GetComponent<NPCEquipCardController> ().BackEquipButton.GetComponent<Button> ().interactable = true;
			} else if (item.GetComponent<EquippableItem> ().ForHand) {
				g.GetComponent<NPCEquipCardController> ().HandEquipButton.GetComponent<Button> ().interactable = true;
			} else {
				g.GetComponent<NPCEquipCardController> ().HeadEquipButton.GetComponent<Button> ().interactable = true;
			}
        }
    }

    private void resetAllButtons()
    {
		foreach (GameObject g in NPCCards)
        {
            g.GetComponent<NPCEquipCardController>().EquipButton.GetComponent<Button>().interactable = false; 

            if (g.GetComponent<NPCEquipCardController>() != null)
            {
                g.GetComponent<NPCEquipCardController>().BackEquipButton.GetComponent<Button>().interactable = false;

                g.GetComponent<NPCEquipCardController>().HandEquipButton.GetComponent<Button>().interactable = false;

				g.GetComponent<NPCEquipCardController>().HeadEquipButton.GetComponent<Button>().interactable = false;
            }

     
        }
    }
 
    //when the user clicks a 'select' button, enable the buttons for all players, hold that item here somewhere?
    //when the user clicks an enabled 'equip' button, add that item to the player inventory, update the UI

    public void equipHandItemToNPC(GameObject npc)
    {
        npc.GetComponent<NPCInventory>().EquipHandItem(currentItem);

        currentItem = null;
        resetAllButtons();
    }

    public void equipBackItemToNPC(GameObject npc)
    {
        npc.GetComponent<NPCInventory>().EquipBackItem(currentItem);
        currentItem = null;
        resetAllButtons();
    }

	public void equipHeadItemToNPC(GameObject npc)
	{
		npc.GetComponent<NPCInventory> ().EquipHeadItem (currentItem);
		currentItem = null;
		resetAllButtons ();
	}

    public void equipItemToNPC(GameObject npc)
    {
        if (currentItem.GetComponent<EquippableItem>().ForBack)
        {
            equipBackItemToNPC(npc);
        }
        else if (currentItem.GetComponent<EquippableItem>().ForHand)
        {
            equipHandItemToNPC(npc);
        }
        else
        {
            equipHeadItemToNPC(npc);
        }
    }

}
