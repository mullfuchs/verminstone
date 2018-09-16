using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveManager : MonoBehaviour {

	public int numberOfFloorsToMake = 5;

	int currentFloor = 0;

	public GameObject mapGenObject;

	public GameObject player;

	public GameObject navMeshFloor;

	public GameObject NPCHolder;

    public bool DemoCaveMode = false;

    List<Floor> FloorList = new List<Floor>();

	Floor CurrentFloor;


	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		mapGenObject.GetComponent<GenerateMap> ().initLevel ();
		GenerateDungeon ();
		LoadFloor (currentFloor, true);

	}

	void GenerateDungeon() {
		for (int i = 0; i < numberOfFloorsToMake; i++) {
			int[,] tempFloor = mapGenObject.GetComponent<GenerateMap> ().MakeFloor ();
			List<Vector3> tempPositionList = mapGenObject.GetComponent<GenerateMap>().GetRandomPointsInRooms (2);
			List<Vector3> spawnPointsList = mapGenObject.GetComponent<GenerateMap> ().GetRandomPointsInRooms (1);
			List<PlaceObjects.FloorObject> tempObjList = mapGenObject.GetComponent<PlaceObjects> ().PopulateMapList (tempPositionList, spawnPointsList);
			FloorList.Add( new Floor(tempFloor, tempObjList) );
		}
	}

	void LoadFloor(int floorNumber, bool isDescending){
		Floor FloorToLoad = FloorList.ElementAt (floorNumber);
		CurrentFloor = FloorToLoad;
		mapGenObject.GetComponent<GenerateMap> ().RenderMap (FloorToLoad.getMap ());
		makeNavMesh ();
		mapGenObject.GetComponent<PlaceObjects> ().finishedSpawning = false;
		StartCoroutine(InstantiateObjectsForFloor(CurrentFloor));
		if (isDescending) {
			player.transform.position = CurrentFloor.AscendPosition;
			moveNPCTeamToPoint (NPCHolder, CurrentFloor.AscendPosition);
		} else {
			player.transform.position = CurrentFloor.DescendPosition;
			moveNPCTeamToPoint (NPCHolder, CurrentFloor.DescendPosition);

		}

		//moveNPCTeamToPoint(new Vector3(100, 100, 100));
		//GameObject AscendObject = CurrentFloor.AscendObject;//GameObject.Find("PassageUp(Clone)");

	}

	IEnumerator InstantiateObjectsForFloor(Floor _currentFloor){

		mapGenObject.GetComponent<PlaceObjects> ().PopulateMapWithObjects (_currentFloor.getObjectList());
		//while (mapGenObject.GetComponent<PlaceObjects> ().finishedSpawning) {
		yield return new WaitForSeconds(1.0f);
		//}

	}

 

	public void DescendToLowerFloor(){
		if (currentFloor + 1 <= numberOfFloorsToMake) {
			currentFloor += 1;
			mapGenObject.GetComponent<PlaceObjects> ().ClearObjectsOnFloor ();
			CurrentFloor = null;
			LoadFloor (currentFloor, true);

		}
	}

	public void AscendToUpperFloor(){
		if (currentFloor - 1 >= 0) {
			currentFloor -= 1;
			mapGenObject.GetComponent<PlaceObjects> ().ClearObjectsOnFloor ();
			CurrentFloor = null;
			LoadFloor (currentFloor, false);
		} else {
			
			Debug.Log ("resurfaceing");
            if (DemoCaveMode)
            {
                GameObject.Find("DemoGameHandler").GetComponent<HandleGameState>().AscendAndShowResults();
            }
            else
            {
                SceneManager.LoadScene("Camp", LoadSceneMode.Single);

				GameObject.Find ("CampEventController").GetComponent<CampEventController> ().ExitCaveSequence ();
				//StartCoroutine (WaitASec ());
            }
		}
	}

	void makeNavMesh(){
		navMeshFloor.GetComponent<UnityEngine.AI.NavMeshSurface> ().BuildNavMesh ();
	}


		
	void moveNPCTeamToPoint(GameObject Holder, Vector3 Location){
		//GameObject[] Carriers = player.GetComponent<NPCTeamHandler> ().NPCCarriers;
		GameObject[] Miners = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		Holder.GetComponent<PlayerAndNPCSpawner> ().setPoint (Location);
		//Debug.Log ("carrier count" + Carriers.Length);
		Debug.Log ("miner count" + Miners.Length);
		for (int i = 0; i < Miners.Count(); i++) {

			Debug.Log ("Enqued");

			Holder.GetComponent<PlayerAndNPCSpawner> ().addNPC(Miners [i]);

		}

	}

	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator WaitASec(){ 
		yield return new WaitForSeconds (2);
	}
}
