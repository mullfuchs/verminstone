using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampInventoryController : MonoBehaviour {

    public GameObject[] items;

    public GameObject ShopKeeperNPC;

    public int weaponLevel = 0;
    public int armorLevel = 0;
    public int bagLevel = 0;
    public int helmetLevel = 0;
    public int pickaxeLevel = 0;

    public GameObject[] weapons;
    public GameObject[] armors;
    public GameObject[] bags;
    public GameObject[] helmets;
    public GameObject[] pickaxes;

    public List<InventoryItem> allItems;

    //if a thing has zero, it doesn't show up
    //ui will display it?

    // Use this for initialization
    void Start() {
        ShopKeeperNPC = GameObject.Find("ShopKeeper");
    }

    private void Awake()
    {
        ShopKeeperNPC = GameObject.Find("ShopKeeper");
    }

    // Update is called once per frame
    void Update() {

    }

    public GameObject[] getInventoryList()
    {
        GameObject[] InventoryList = new GameObject[] { weapons[weaponLevel], armors[armorLevel], bags[bagLevel], helmets[helmetLevel], pickaxes[pickaxeLevel] };
        return InventoryList;
    }

    public void EnableShopKeeper(bool isActive)
    {
        if(ShopKeeperNPC == null)
        {
            ShopKeeperNPC = GameObject.Find("ShopKeeper");
        }

        if (ShopKeeperNPC != null)
        {
            ShopKeeperNPC.SetActive(isActive);
        }
    }

    [Yarn.Unity.YarnCommand("upgradeSwordLevel")]
    public void UpgradeWeaponLevel()
    {
        if(weaponLevel + 1 <= weapons.Length)
        {
            weaponLevel += 1;
        }
    }

    [Yarn.Unity.YarnCommand("upgradeArmorLevel")]
    public void UpgradeArmorLevel()
    {
        if (armorLevel + 1 <= armors.Length)
        {
            armorLevel += 1;
        }
    }

    [Yarn.Unity.YarnCommand("upgradeBagLevel")]
    public void UpgradeBagLevel()
    {
        if (bagLevel + 1 <= bags.Length)
        {
            bagLevel += 1;
        }
    }

    [Yarn.Unity.YarnCommand("upgradeHelmetLevel")]
    public void UpgradeHelmetLevel()
    {
        if(helmetLevel + 1 <= helmets.Length)
        {
            helmetLevel += 1;
        }
    }


    [Yarn.Unity.YarnCommand("upgradePickaxeLevel")]
    public void UpgradePickaxeLevel()
    {
        if (pickaxeLevel + 1 <= pickaxes.Length)
        {
            pickaxeLevel += 1;
        }
    }


    [System.Serializable]
    public struct InventoryItem{
        GameObject ItemInInventory;
        int InventoryAmount;
    }
}
