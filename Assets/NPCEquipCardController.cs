using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCEquipCardController : MonoBehaviour {
    
    private GameObject associatedNPC;

    private GameObject canvasOBJ;

    public GameObject nameText;
    public GameObject HandEquipButton;
    public GameObject BackEquipButton;

    // Use this for initialization
    void Start () {
        Button handequipbtn = HandEquipButton.GetComponent<Button>();
        Button backequipbtn = BackEquipButton.GetComponent<Button>();

        handequipbtn.onClick.AddListener(EquipHandObject);
        backequipbtn.onClick.AddListener(EquipBackObject);

        canvasOBJ = GameObject.Find("Canvas");
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

    private void EquipHandObject()
    {
        canvasOBJ.GetComponent<EquipUIController>().equipHandItemToNPC(associatedNPC);
    }

    private void EquipBackObject()
    {
        canvasOBJ.GetComponent<EquipUIController>().equipBackItemToNPC(associatedNPC);
    }

    public GameObject getAssociatedNPC()
    {
        if(associatedNPC != null)
        {
            return associatedNPC;
        }
        else
        {
            return null;
        }
    }
}
