using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUIController : MonoBehaviour {

    public GameObject CardPrefab;

    public GameObject cardParent;

    private GameObject[] NPCs;

    // Use this for initialization
    void Start()
    {
        NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void CreateAndDisplayNPCcards()
    {
        cardParent.SetActive(true);
        foreach (GameObject g in NPCs)
        {
            GameObject uiCard = Instantiate(CardPrefab, cardParent.transform, false);
            uiCard.SetActive(true);
            uiCard.GetComponent<NPCFoodCardController>().assignNPCtoCard(g);
        }
    }

}
