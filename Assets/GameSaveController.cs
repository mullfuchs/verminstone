using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

public class GameSaveController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void Awake(){
        GameObject startGameObj = GameObject.Find("StartGameController");
        if (startGameObj != null && startGameObj.GetComponent<StartGameController>().loadGameFromSave == true)
        {
            LoadGame();
            //this is also being looked at in the game event controller so watch it
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			SaveGame ();		
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			LoadGame ();
		}
	}

	public void SaveGame(){
		Save saveObject = CreateGameSave ();

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
		bf.Serialize(file, saveObject);
		file.Close();

		print ("Saved game?");
	}

	public void LoadGame(){
		if (File.Exists (Application.persistentDataPath + "/gamesave.save")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
			Save save = (Save)bf.Deserialize(file);
			file.Close();
			print("Game file loaded?");
			LoadGameData (save);
		}
		else{
			print ("Could not find save file");	
		}


		//set day

		//maybe get camp population controller and have it load the player?
		//then have it load the NPCs?

	}
		

	public Save CreateGameSave(){
		Save gameSave = new Save ();

		//get all the npcs
		GameObject[] npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		for (int i = 0; i < npcs.Length; i++) {
			NPCstats stats = npcs [i].GetComponent<NPCstats> ();

			NPCProfile profile = new NPCProfile();
			profile.NPCName = stats.NPCName;
			profile.NPCHealth = stats.health;
            profile.NPCPosition = npcs[i].transform.position;
			profile.NPCDaysTalkedTo = stats.daysTalkedTo;
			profile.NPCDialogIndex = stats.NPCScriptIndex;

			gameSave.NPCProfiles.Add (profile);
		}

		GameObject player = GameObject.Find ("Player");

		gameSave.PlayerPosition = player.transform.position;
		gameSave.PlayerHealth = player.GetComponent<health> ().healthPoints;

		gameSave.DaysElapsed = GameObject.Find ("CampEventController").GetComponent<CampEventController> ().day;

		return gameSave;
	}

	void LoadGameData(Save data){

        CampPopulationController campPopController = GameObject.Find("CampEventController").GetComponent<CampPopulationController>();
        campPopController.LoadPlayerFromSave(data.PlayerPosition, data.PlayerHealth);

        //set the day
        gameObject.transform.GetComponent<CampEventController>().day = data.DaysElapsed;

		foreach (NPCProfile profile in data.NPCProfiles) {
			campPopController.LoadNPCFromSave (profile.NPCName, profile.NPCHealth, profile.NPCPosition, profile.NPCDaysTalkedTo, profile.NPCDialogIndex);
		}


	}


}
