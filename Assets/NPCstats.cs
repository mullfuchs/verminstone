﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCstats : MonoBehaviour {

	public string NPCName;
	public string mood;
	public float health = 50;
	public float maxHealth = 100;
	public float stamina = 25;
	public float maxStamina = 100;
	public float endurance = 1;
	public float recovery = 1;
	public float bravery = 1;
	public float runSpeed = 4;
	public GameObject ragDollObject;
	public bool hasBeenTalkedToToday = false;
	public int daysTalkedTo = 1;
	public Sprite[] DialogPortraits;
	public int NPCScriptIndex;

	/*
	 * NPC systems
	 * need a way to make different profiles so what do npcs do?
	 * Health: they got a lot or a little 
	 * Defense: how hardy they are to hits 
	 * Attack: how much they attack 
	 * Endurance: how much they keep stamina
	 * Stamina Amount: How long can they go
	 * can I think of more?
	 * Recovery: how fast they recover Stamina
	 * Bravery: do they stay on task under duress?
	 * Run speed: how fast they run
	 * 
	 * where is this defined? maybe in scripts? so do I parse the json script?
	 * do I make a separate class that holds all these things?
	 * yeah and make it serializable so I can just throw it into a fucking save file
	 * 
	 * csv that's loaded at start?
	 * 
	 * these stats are also going to have to be saved and restored on load too
	*/

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
