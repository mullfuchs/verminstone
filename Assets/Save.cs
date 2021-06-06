using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
	public List<NPCProfile> NPCProfiles = new List<NPCProfile>();

	public SerializableVector3 PlayerPosition;
	public float PlayerHealth;
	public int DaysElapsed;
	public List<string> CompletedQuests = new List<string>();
    public QuestVariables QuestVariableReference = new QuestVariables();
    public ExampleVariableStorage.DefaultVariable[] DialogViariableReferece;
    public int WeaponLevel;
    public int ArmorLevel;
    public int BagLevel;
    public int HelmetLevel;
    public int PickaxeLevel;
}

[System.Serializable]
public class NPCProfile{
	public string NPCName;
	public int NPCDialogIndex;
    public SerializableVector3 NPCPosition;
	public float NPCHealth;
	public int NPCDaysTalkedTo;
	public NPCStatRecord statRecord;
}
	