using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
	public List<NPCProfile> NPCProfiles = new List<NPCProfile>();

	public Vector3 PlayerPosition;
	public float PlayerHealth;

	public int DaysElapsed;
}

[System.Serializable]
public class NPCProfile{
	public string NPCName;
	public int NPCDialogIndex;
	public Vector3 NPCPosition;
	public float NPCHealth;
	public int NPCDaysTalkedTo;
}
	