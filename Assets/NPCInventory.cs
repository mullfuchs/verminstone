using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventory : MonoBehaviour {
    
    public GameObject ObjectHeldInHands;
    public GameObject ObjectOnBack;

    private Transform handTransform;
    private Transform backTransform;
    private GameObject ActiveHandObject;
    private GameObject ActiveBackObject;

	// Use this for initialization
	void Start () {
        handTransform = FindDeepChild(gameObject.transform, "hand_R");
        backTransform = FindDeepChild(gameObject.transform, "neck");
        if(ObjectHeldInHands != null)
        {
            ActiveHandObject = Instantiate(ObjectHeldInHands, handTransform.position, handTransform.rotation, handTransform);
        }
        if (ObjectOnBack != null)
        {
            ActiveBackObject = Instantiate(ObjectOnBack, backTransform.position, backTransform.rotation, backTransform);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    Transform getBodyTransform(string part)
    {
        Transform hand;
        hand = transform.Find(part);
        if(hand == null)
        {
            print("couldn't find part " + part);
            return gameObject.transform;
        }
        else
        {
            print("found part");
            return hand;
        }
    }

    public void EquipHandItem(GameObject item)
    {
        Destroy(ActiveHandObject);
        ActiveHandObject = Instantiate(item, handTransform.position, handTransform.rotation, handTransform);

        ObjectHeldInHands = item;
    }

    public void EquipBackItem(GameObject item)
    {
        Destroy(ActiveBackObject);
        ActiveBackObject = Instantiate(item, backTransform.position, backTransform.rotation, backTransform);
        ObjectOnBack = item;
    }

	public GameObject getHandObject(){
		if (ActiveHandObject) {
			return ActiveHandObject;
		} else {
			return null;
		}
	}

    public Transform FindDeepChild(Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = FindDeepChild(child, aName);
            if (result != null)
                return result;
        }
        return null;
    }

}
