using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventory : MonoBehaviour {
    
    public GameObject ObjectHeldInHands;
    public GameObject ObjectOnBack;
	public GameObject ObjectOnHead;

    private Transform handTransform;
    private Transform backTransform;
	private Transform headTransform;

    private GameObject ActiveHandObject;
    private GameObject ActiveBackObject;
	private GameObject ActiveHeadObject;

	// Use this for initialization
	void Start () {
        handTransform = FindDeepChild(gameObject.transform, "hand_R");
        backTransform = FindDeepChild(gameObject.transform, "neck");
		headTransform = FindDeepChild (gameObject.transform, "head");

        if(ObjectHeldInHands != null)
        {
            ActiveHandObject = Instantiate(ObjectHeldInHands, handTransform.position, handTransform.rotation, handTransform);
			ActiveHandObject.GetComponent<Rigidbody> ().isKinematic = true;
			ActiveHandObject.GetComponent<Rigidbody> ().useGravity = false;
			ActiveHandObject.GetComponent<BoxCollider> ().enabled = false;
        }
        if (ObjectOnBack != null)
        {
            ActiveBackObject = Instantiate(ObjectOnBack, backTransform.position, backTransform.rotation, backTransform);
			ActiveBackObject.GetComponent<Rigidbody> ().isKinematic = true;
			ActiveBackObject.GetComponent<Rigidbody> ().useGravity = false;
			ActiveBackObject.GetComponent<BoxCollider> ().enabled = false;
        }
		if (ObjectOnHead != null) 
		{
			ActiveHeadObject = AttachObject (ObjectOnHead, headTransform);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	GameObject AttachObject(GameObject item, Transform objectTransform){
		GameObject activeSlotObject = Instantiate (item, objectTransform.position, objectTransform.rotation, objectTransform);
		if (activeSlotObject.GetComponent<Rigidbody> () != null) {
			activeSlotObject.GetComponent<Rigidbody> ().isKinematic = true;
			activeSlotObject.GetComponent<Rigidbody> ().useGravity = false;
		}
		if (activeSlotObject.GetComponent<BoxCollider> () != null) {
			activeSlotObject.GetComponent<BoxCollider> ().enabled = false;
		}
		return activeSlotObject;
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

	public void EquipHeadItem(GameObject item)
	{
		Destroy(ActiveHeadObject);
		ActiveHeadObject = Instantiate(item, headTransform.position, headTransform.rotation, headTransform);
		ActiveHeadObject.GetComponent<Rigidbody> ().isKinematic = true;
		ActiveHeadObject.GetComponent<Rigidbody> ().useGravity = false;
		ActiveHeadObject.GetComponent<BoxCollider> ().enabled = false;
		ObjectOnHead = item;
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

    public GameObject getBackObject()
    {
        if (ActiveBackObject)
        {
            return ActiveBackObject;
        }
        else
        {
            return null;
        }
    }


	public void DestroyAllEquippedObjects()
	{
		
		if (ActiveHandObject) {
			Destroy (ActiveHandObject);
		}
		if (ActiveBackObject) {
			Destroy (ActiveBackObject);
		}
		if (ActiveHeadObject) {
			print ("removing helmet");
			Destroy (ActiveHeadObject);
		}
			
		ObjectHeldInHands = null;
		ObjectOnHead = null;
		ObjectOnBack = null;
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
