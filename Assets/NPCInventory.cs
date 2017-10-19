using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventory : MonoBehaviour {

    public List<GameObject> _NPCInventory;

    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject Back;

	// Use this for initialization
	void Start () {
        _NPCInventory = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
