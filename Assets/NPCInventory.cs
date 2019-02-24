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
		ActiveHandObject.GetComponent<Rigidbody> ().isKinematic = true;
		ActiveHandObject.GetComponent<Rigidbody> ().useGravity = false;
		ActiveHandObject.GetComponent<BoxCollider> ().enabled = false;
        ObjectHeldInHands = item;
    }

    public void EquipBackItem(GameObject item)
    {
        Destroy(ActiveBackObject);
        ActiveBackObject = Instantiate(item, backTransform.position, backTransform.rotation, backTransform);
		ActiveBackObject.GetComponent<Rigidbody> ().isKinematic = true;
		ActiveBackObject.GetComponent<Rigidbody> ().useGravity = false;
		ActiveBackObject.GetComponent<BoxCollider> ().enabled = false;
        ObjectOnBack = item;
    }

	public void DropBackItem()
	{
		ActiveBackObject.transform.parent = null;
		ActiveBackObject.GetComponent<Rigidbody> ().isKinematic = false;
		ActiveBackObject.GetComponent<Rigidbody> ().useGravity = true;
        ActiveBackObject.GetComponent<BoxCollider>().enabled = true;
        ObjectOnBack = null;
	}

	public void DropHandItem()
	{
		ActiveHandObject.transform.parent = null;
		ActiveHandObject.GetComponent<Rigidbody> ().isKinematic = false;
		ActiveHandObject.GetComponent<Rigidbody> ().useGravity = true;
		ActiveHandObject.GetComponent<BoxCollider> ().enabled = true;
        ObjectHeldInHands = null;
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
