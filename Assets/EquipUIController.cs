using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipUIController : MonoBehaviour {

    public GameObject ItemCardPrefab;

    public GameObject ItemCardParent;

    public GameObject ItemGroupParent;

    public GameObject NPCCardPrefab;

    public GameObject NPCCardParent;

    public GameObject VStoneQuotaUIObject;

    public GameObject LaunchButton;

    private int CurrentQuota;
    private int CurrentCapacity;

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
        CurrentCapacity = 0;
		//Items = 
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void CreateAndDisplayNPCcards()
    {
        CurrentQuota = (int)GameObject.Find("CampEventController").GetComponent<VStoneEconomyObject>().getDailyQuota();

        NPCCardParent.SetActive(true);
        NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        foreach (GameObject g in NPCs)
        {
            GameObject uiCard = Instantiate(NPCCardPrefab, NPCCardParent.transform, false);
            uiCard.SetActive(true);
            uiCard.GetComponent<NPCEquipCardController>().assignNPCtoCard(g);
			NPCCards.Add(uiCard);
        }

        UpdateCarryCapacityUI();
    }

    public void CreateAndDisplayItemCards()
    {
        Items = GameObject.Find("CampEventController").GetComponent<CampInventoryController>().getInventoryList();
        ItemCardParent.SetActive(true);
        foreach (GameObject g in Items)
        {
            GameObject uiCard = Instantiate(ItemCardPrefab, ItemCardParent.transform, false);
            uiCard.SetActive(true);
            uiCard.GetComponent<ItemEquipCardController>().assignItemtoCard(g);
			ItemCards.Add (uiCard);
        }
        ItemCards[0].GetComponentInChildren<UnityEngine.UI.Button>().Select();
    }
    
    public void CreateAndDisplayItemCategories()
    {
        CampInventoryController inventoryInstance = GameObject.Find("CampEventController").GetComponent<CampInventoryController>();
        ItemCardParent.SetActive(true);
        //create a new item category object, parent it to the Item card parent
        //put cards in there for the items in each catagory

    }
    

    public void UpdateCarryCapacityUI()
    {
        NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        int npcCapacity = 0;
        foreach (GameObject g in NPCs)
        {
           GameObject backObject = g.GetComponent<NPCInventory>().getBackObject();
           if ( backObject != null && backObject.tag == "BagTool")
            {
                GameObject bag = g.GetComponent<NPCInventory>().ObjectOnBack;
                npcCapacity += bag.GetComponent<Vstonebag>().vStoneCapacity;
            }

        }
        VStoneQuotaUIObject.GetComponent<Text>().text = "Today's Quota: " + CurrentQuota + "\n" + "Current Capacity: " + npcCapacity;
    }

	public void cleanUpItemAndNPCCards(){
		//destroy cards

		foreach (GameObject x in ItemCards) {
            Destroy(x);
		}
		foreach (GameObject x in NPCCards) {
            Destroy(x);
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

            /*
            if (item.GetComponent<EquippableItem> ().ForBack) {
				g.GetComponent<NPCEquipCardController> ().BackEquipButton.GetComponent<Button> ().interactable = true;
			} else if (item.GetComponent<EquippableItem> ().ForHand) {
				g.GetComponent<NPCEquipCardController> ().HandEquipButton.GetComponent<Button> ().interactable = true;
			} else {
				g.GetComponent<NPCEquipCardController> ().HeadEquipButton.GetComponent<Button> ().interactable = true;
			}
            */
        }
    }

    private void resetAllButtons()
    {
		foreach (GameObject g in NPCCards)
        {
            g.GetComponent<NPCEquipCardController>().EquipButton.GetComponent<Button>().interactable = false; 

            /*
            if (g.GetComponent<NPCEquipCardController>() != null)
            {
                g.GetComponent<NPCEquipCardController>().BackEquipButton.GetComponent<Button>().interactable = false;

                g.GetComponent<NPCEquipCardController>().HandEquipButton.GetComponent<Button>().interactable = false;

				g.GetComponent<NPCEquipCardController>().HeadEquipButton.GetComponent<Button>().interactable = false;
            }
            */
     
        }
    }
 
    //when the user clicks a 'select' button, enable the buttons for all players, hold that item here somewhere?
    //when the user clicks an enabled 'equip' button, add that item to the player inventory, update the UI

    public void equipHandItemToNPC(GameObject npc)
    {
        npc.GetComponent<NPCInventory>().EquipHandItem(currentItem);

       // currentItem = null;
       // resetAllButtons();
        checkForValidLoadout();
    }

    public void equipBackItemToNPC(GameObject npc)
    {
        npc.GetComponent<NPCInventory>().EquipBackItem(currentItem);
        UpdateCarryCapacityUI();
       // currentItem = null;
       // resetAllButtons();
        checkForValidLoadout();
    }

	public void equipHeadItemToNPC(GameObject npc)
	{
		npc.GetComponent<NPCInventory> ().EquipHeadItem (currentItem);
		//currentItem = null;
		//resetAllButtons ();
        checkForValidLoadout();
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

    public GameObject getCurrentlySelectedItem()
    {
        return currentItem;
    }

    void checkForValidLoadout()
    {
        //if there's at least one equipped pickaxe and one equipped bag, enable the launch button
        //otherwise set it to false;
        bool hasPickaxe = false;
        bool hasBag = false;

        NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        foreach (GameObject g in NPCs)
        {
            NPCInventory Inventory =  g.GetComponent<NPCInventory>();
            GameObject backobject = Inventory.getBackObject();
            if (backobject != null && backobject.tag == "BagTool")
            {
                hasBag = true;
            }
            GameObject handobject = Inventory.getHandObject();
            if(handobject != null && handobject.tag == "MineTool")
            {
                hasPickaxe = true;
            }
        }

        if(hasBag && hasPickaxe)
        {
            enableLaunchButton(true);
        }
        else
        {
            enableLaunchButton(false);
        }

    }

    void enableLaunchButton(bool canInteract)
    {
        LaunchButton.GetComponent<Button>().interactable = canInteract;
    }

    public void SetUpControllerNavigation()
    {
        //UNITY IS BEING A LITTLE SCRIMBLO BIMBLO
        //I guess I can get the equip buttons and see if there's a corrosponding select button, if not set it to the last one
        //for the first 

        for (int cardIndex = 0; cardIndex < NPCCards.Count; cardIndex++)
        {
            if (cardIndex < ItemCards.Count)
            {
                //OMGF
                Navigation buttonNav = NPCCards[cardIndex].GetComponent<NPCEquipCardController>().EquipButton.GetComponent<Button>().navigation;
                buttonNav.selectOnRight = ItemCards[cardIndex].GetComponentInChildren<Button>();
                buttonNav.selectOnUp = getSelectOnUp(NPCCards, cardIndex, LaunchButton);
                buttonNav.selectOnDown = getSelectOnDown(NPCCards, cardIndex, LaunchButton);
                NPCCards[cardIndex].GetComponent<NPCEquipCardController>().EquipButton.GetComponent<Button>().navigation = buttonNav;
            }
            else
            {
                Navigation buttonNav = NPCCards[cardIndex].GetComponent<NPCEquipCardController>().EquipButton.GetComponent<Button>().navigation;
                buttonNav.selectOnRight = ItemCards[NPCCards.Count].GetComponentInChildren<Button>();
                buttonNav.selectOnUp = getSelectOnUp(NPCCards, cardIndex, LaunchButton);
                buttonNav.selectOnDown = getSelectOnDown(NPCCards, cardIndex, LaunchButton);
                NPCCards[cardIndex].GetComponent<NPCEquipCardController>().EquipButton.GetComponent<Button>().navigation = buttonNav;
            }
        }

        for (int cardIndex = 0; cardIndex < ItemCards.Count; cardIndex++)
        {
            if (cardIndex < NPCCards.Count)
            {
                Navigation buttonNav = ItemCards[cardIndex].GetComponentInChildren<Button>().navigation;
                buttonNav.selectOnLeft = NPCCards[cardIndex].GetComponentInChildren<Button>();
                buttonNav.selectOnUp = getSelectOnUp(ItemCards, cardIndex, LaunchButton);
                buttonNav.selectOnDown = getSelectOnDown(ItemCards, cardIndex, LaunchButton);
                ItemCards[cardIndex].GetComponentInChildren<Button>().navigation = buttonNav;
            }
            else
            {
                Navigation buttonNav = ItemCards[cardIndex].GetComponentInChildren<Button>().navigation;
                buttonNav.selectOnLeft = NPCCards[NPCCards.Count].GetComponentInChildren<Button>();
                buttonNav.selectOnUp = getSelectOnUp(ItemCards, cardIndex, LaunchButton);
                buttonNav.selectOnDown = getSelectOnDown(ItemCards, cardIndex, LaunchButton);
                ItemCards[cardIndex].GetComponentInChildren<Button>().navigation = buttonNav;
            }
        }

    }

    Button getSelectOnUp(List<GameObject> buttonStack, int currentIndex, GameObject DefaultButton)
    {
        //given the index, give the button above it in order, if it's the first, set it to the default button
        if(currentIndex == 0)
        {
            if(DefaultButton.GetComponent<Button>() != null)
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
