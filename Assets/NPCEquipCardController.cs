using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCEquipCardController : MonoBehaviour {
    
    private GameObject associatedNPC;

    public GameObject nameText;
    public GameObject HandEquipButton;
    public GameObject BackEquipButton;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void assignNPCtoCard(GameObject npc)
    {
        associatedNPC = npc;
        UpdateEquipCard();
    }

    private void UpdateEquipCard()
    {
        NPCstats stats = associatedNPC.GetComponent<NPCstats>();
        nameText.GetComponent<Text>().text = stats.name;

    }
}
