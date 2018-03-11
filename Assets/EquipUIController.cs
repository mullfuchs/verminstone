using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUIController : MonoBehaviour {

    public GameObject ItemCardPrefab;

    public GameObject ItemCardParent;

    public GameObject NPCCardPrefab;

    public GameObject NPCCardParent;

    private GameObject[] NPCs;

    private GameObject[] Items;

    // Use this for initialization
    void Start()
    {
        NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        Items = GameObject.Find("CampEventController").GetComponent<CampInventoryController>().items;
        //Items = 
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void CreateAndDisplayNPCcards()
    {
        NPCCardParent.SetActive(true);
        foreach (GameObject g in NPCs)
        {
            GameObject uiCard = Instantiate(NPCCardPrefab, NPCCardParent.transform, false);
            uiCard.SetActive(true);
            uiCard.GetComponent<NPCEquipCardController>().assignNPCtoCard(g);
        }
    }

    public void CreateAndDisplayItemCards()
    {
        ItemCardParent.SetActive(true);
        foreach (GameObject g in Items)
        {
            GameObject uiCard = Instantiate(ItemCardPrefab, ItemCardParent.transform, false);
            uiCard.SetActive(true);
            uiCard.GetComponent<ItemEquipCardController>().assignItemtoCard(g);
        }
    }

}
