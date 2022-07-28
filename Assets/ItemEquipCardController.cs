using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEquipCardController : MonoBehaviour {

    private GameObject associatedItem;

    public GameObject nameText;

    public GameObject statText;

    public Image icon;
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
        nameText.GetComponent<Text>().text = "Level " + itemStats.itemLevel + " " + itemStats.itemName;
        statText.GetComponent<Text>().text = GetStatBonusString(associatedItem);
        icon.sprite = itemStats.icon;
    }

    public void setThisItemToEquip()
    {
        EquipUIController equipUI = GameObject.Find("Canvas").GetComponent<EquipUIController>();
		if(equipUI != null && associatedItem != null)
        {
            equipUI.SetCurrentItemAndEnableButtons(associatedItem);
        }

    }

    private string GetStatBonusString(GameObject item)
    {
        if (item.GetComponent<DefenseController>() != null)
        {
            return "Def +" + item.GetComponent<DefenseController>().defensePoints;
        }

        if (item.GetComponent<WeaponController>() != null)
        {
            return "Atk +" + item.GetComponent<WeaponController>().damage;
        }

        return "";
    }

}
