using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampInventoryController : MonoBehaviour {

    public GameObject[] items;

    public InventoryItem[] weapons;
    public InventoryItem[] armors;
    public InventoryItem[] bags;
    public InventoryItem[] helmets;
    public InventoryItem[] pickaxes;

    public List<InventoryItem> allItems;

    //if a thing has zero, it doesn't show up
    //ui will display it?

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }


    [System.Serializable]
    public struct InventoryItem{
        GameObject ItemInInventory;
        int InventoryAmount;
    }
}
