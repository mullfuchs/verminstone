using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampQuestController : MonoBehaviour {
	//so how do quests work?
	//one way is to have a quest gameobject, that holds whatever we want the quest to do
	//and when it's enabled in on awake it does whatever tasks it needs to do
	//and so to make a quest happen a call gets made, and the quest is popped up
	//if there's performance issues, consider changing this to instatiate objects and have a positional reference.

	//consider changing the quest object to a hash table

	public QuestReference[] QuestObjects;

	private List<string> CompletedQuests = new List<string>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Yarn.Unity.YarnCommand("startQuest")]
	public void StartQuest(string QuestName){
		foreach (QuestReference q in QuestObjects) {
			if (QuestName == q.QuestName) {
				print ("Starting Quest: " + q.QuestName);
				q.QuestObject.SetActive (true);
				CompletedQuests.Add (q.QuestName);
			}
		}
	}

	public void LoadCompletedQuests (List<string> quests){
		foreach (string quest in quests) {
			foreach (QuestReference q in QuestObjects) {
				if (quest == q.QuestName) {
					print ("Loading Quest Object: " + q.QuestName);
					q.QuestObject.SetActive (true);
					CompletedQuests.Add (q.QuestName);
				}
			}
		}
	}

	public List<string> GetCompletedQuestList(){
		return CompletedQuests;
	}

	IEnumerator BasicEscapeQuest()
	{

		yield return new WaitForSeconds(3);

	}
}

[System.Serializable] 
public class QuestReference{
	public string QuestName;
	public GameObject QuestObject;
}