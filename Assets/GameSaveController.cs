using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

public class GameSaveController : MonoBehaviour {

    private Save currentlyLoadedSave;

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
			profile.statRecord = stats.statObject;

			gameSave.NPCProfiles.Add (profile);
		}

		GameObject player = GameObject.Find ("Player");
        GameObject campEventController = GameObject.Find("CampEventController");

		gameSave.PlayerPosition = player.transform.position;
		gameSave.PlayerHealth = player.GetComponent<health> ().healthPoints;
        //gameSave.DaysElapsed = GameObject.Find ("CampEventController").GetComponent<CampEventController> ().day;
        gameSave.DaysElapsed = campEventController.GetComponent<CampEventController>().day;

        //weapon/upgrade levels
        gameSave.WeaponLevel = campEventController.GetComponent<CampInventoryController>().weaponLevel;
        gameSave.ArmorLevel = campEventController.GetComponent<CampInventoryController>().armorLevel;
        gameSave.BagLevel = campEventController.GetComponent<CampInventoryController>().bagLevel;
        gameSave.HelmetLevel = campEventController.GetComponent<CampInventoryController>().helmetLevel;
        gameSave.PickaxeLevel = campEventController.GetComponent<CampInventoryController>().pickaxeLevel;

        gameSave.CompletedQuests = GameObject.Find ("GameQuestObjects").GetComponent<CampQuestController> ().GetCompletedQuestList ();
        gameSave.QuestVariableReference = GameObject.Find("GameQuestObjects").GetComponent<CampQuestController>().GatherQuestVariablesForSave();
        gameSave.DialogViariableReferece = GameObject.Find("Dialogue").GetComponent<ExampleVariableStorage>().GetDialogVariables();

        currentlyLoadedSave = gameSave;

		return gameSave;
	}

	void LoadGameData(Save data){

        CampPopulationController campPopController = GameObject.Find("CampEventController").GetComponent<CampPopulationController>();
        campPopController.LoadPlayerFromSave(data.PlayerPosition, data.PlayerHealth);

        //set the day
        gameObject.transform.GetComponent<CampEventController>().day = data.DaysElapsed;
		gameObject.transform.GetComponent<CampEventController> ().StartDay ();

		foreach (NPCProfile profile in data.NPCProfiles) {
			campPopController.LoadNPCFromSave (profile.NPCName, profile.NPCHealth, profile.NPCPosition, profile.NPCDaysTalkedTo, profile.NPCDialogIndex, profile.statRecord);
		}

		GameObject.Find ("GameQuestObjects").GetComponent<CampQuestController> ().LoadCompletedQuests ( data.CompletedQuests );
        GameObject.Find("GameQuestObjects").GetComponent<CampQuestController>().RestoreQuestVariables(data.QuestVariableReference);

        GameObject.Find("Dialogue").GetComponent<ExampleVariableStorage>().LoadVariablesFromSaveFile(data.DialogViariableReferece);

        //load the upgrade levels
        CampInventoryController campInventoryController = GameObject.Find("CampEventController").GetComponent<CampInventoryController>();
        campInventoryController.weaponLevel = data.WeaponLevel;
        campInventoryController.armorLevel = data.ArmorLevel;
        campInventoryController.bagLevel = data.BagLevel;
        campInventoryController.helmetLevel = data.HelmetLevel;
        campInventoryController.pickaxeLevel = data.PickaxeLevel;

        currentlyLoadedSave = data;
	}

    public void LoadQuestObjects()
    {
        if(currentlyLoadedSave != null)
        {
            GameObject.Find("GameQuestObjects").GetComponent<CampQuestController>().LoadCompletedQuests(currentlyLoadedSave.CompletedQuests);
            GameObject.Find("GameQuestObjects").GetComponent<CampQuestController>().RestoreQuestVariables(currentlyLoadedSave.QuestVariableReference);

            GameObject.Find("Dialogue").GetComponent<ExampleVariableStorage>().LoadVariablesFromSaveFile(currentlyLoadedSave.DialogViariableReferece);

        }
    }
}
