﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEquipCardController : MonoBehaviour {

    private GameObject associatedItem;

    public GameObject nameText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void assignItemtoCard(GameObject item)
    {
        associatedItem = item;
        
        updateCardUI();
    }

    private void updateCardUI()
    {
        EquippableItem itemStats = associatedItem.GetComponent<EquippableItem>();
        nameText.GetComponent<Text>().text = itemStats.itemName;
    }

    public void setThisItemToEquip()
    {
        GameObject.Find("Canvas").GetComponent<EquipUIController>().SetCurrentItemAndEnableButtons(associatedItem);
    }

}
