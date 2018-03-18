﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRoofOnEnter : MonoBehaviour {

    private GameObject player;
    public GameObject roofObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter()
    {
        roofObject.SetActive(false);
    }

    void OnTriggerExit()
    {
        roofObject.SetActive(true);
    }

    
}