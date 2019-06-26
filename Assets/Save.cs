using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
	public List<string> NPCNames = new List<string>();
	public List<int> NPCDialogIndex = new List<int> ();
	public List<Vector3> NPCPositions = new List<Vector3>();
	public List<float> NPCHealth = new List<float>();
	public List<int> NPCDaysTalkedTo = new List<int> ();

	public Vector3 PlayerPosition;
	public float PlayerHealth;

	public int DaysElapsed;
}
