using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Floor : MonoBehaviour {

	int[,] map;
	List<PlaceObjects.FloorObject> gameObjectList;
	List<GameObject> spawnedObjectList;

	public Vector3 AscendPosition;
	public Vector3 DescendPosition;

	public GameObject AscendObject;
	public GameObject DescendObject;

	public Floor(int[,] _map, List<PlaceObjects.FloorObject> _objects){
		map = _map;
		gameObjectList = _objects;
		AscendPosition = _objects.ElementAt (_objects.Count - 2).getPosition();
		DescendPosition = _objects.ElementAt (_objects.Count - 1).getPosition();
	}

	public int[,] getMap(){
		return map;
	}

	public List<PlaceObjects.FloorObject> getObjectList(){
		return gameObjectList;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
