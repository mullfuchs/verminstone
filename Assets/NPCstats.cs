﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCstats : MonoBehaviour {

	public string name;
	public string mood;
	public float health = 50;
	public float stamina = 25;
	public float maxHealth = 100;
	public float maxStamina = 100;

	// Use this for initialization
	void Start () {
		//name = "blank";
		mood = "sad";
		//name = "blank";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
