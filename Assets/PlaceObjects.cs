using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : MonoBehaviour {

	public GameObject objectToAdd;
	public GameObject spawnPointObject;
	public GameObject AscendObjet;
	public GameObject DescendObject;
	public bool finishedSpawning = false;
	//
	//List<Vector3> objectCoords = new List<Vector3>();

	public List<GameObject> currentFloorObjects = new List<GameObject>();

	GameObject currentEntryPoint;
	GameObject currentExitPoint;

	// Use this for initialization
	void Start () {
		//objectCoords = gameObject.GetComponent<GenerateMap> ().GetRandomPointsInRooms (30);
		//PopulateMap ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//todo: refactor this
	public List<FloorObject> PopulateMapList(List<Vector3> objectCoords, List<Vector3>SpawnPoints){
		List<FloorObject> objectList = new List<FloorObject>();
		//disabling object spawning for now
		foreach (Vector3 point in objectCoords) { 
			objectList.Add( new FloorObject(point, objectToAdd));
		}

		foreach (Vector3 point in SpawnPoints) {
			objectList.Add( new FloorObject(point, spawnPointObject));
		}

		FloorObject EntryPoint = new FloorObject(gameObject.GetComponent<GenerateMap> ().GetFloorEntryPoint (), AscendObjet);
		FloorObject ExitPoint = new FloorObject(gameObject.GetComponent<GenerateMap> ().GetFloorExitPoint (), DescendObject);
		objectList.Add (EntryPoint);
		objectList.Add (ExitPoint);
		return objectList;
	}

//	public List<FloorObject> AddEntranceAndExits(List<FloorObject> _objects){
//		FloorObject EntryPoint = new FloorObject(gameObject.GetComponent<GenerateMap> ().GetFloorEntryPoint (), AscendObjet);
//		FloorObject ExitPoint = new FloorObject(gameObject.GetComponent<GenerateMap> ().GetFloorExitPoint (), AscendObjet);
//		_objects.Add (EntryPoint);
//		_objects.Add (ExitPoint);
//		return _objects;
//	}


	public class FloorObject{
		Vector3 positionOnFloor;
		GameObject gameObjectType;
		public FloorObject(Vector3 _position, GameObject _gameObject){
			positionOnFloor = _position;
			gameObjectType = _gameObject;
		}

		public GameObject CreateObject(){
			return Instantiate (gameObjectType, positionOnFloor, Quaternion.identity);
		}

		public Vector3 getPosition(){
			return positionOnFloor;
		}

	}

	public void PopulateMapWithObjects(List<FloorObject> _objects){
		List<GameObject> objectList = new List<GameObject> ();
		foreach (FloorObject obj in _objects) {
			currentFloorObjects.Add( obj.CreateObject ());
		}
		finishedSpawning = true;
	}

	public void ClearObjectsOnFloor(){
		foreach (GameObject obj in currentFloorObjects) {
			Destroy (obj);
		}
		currentFloorObjects.Clear ();
	}

}
