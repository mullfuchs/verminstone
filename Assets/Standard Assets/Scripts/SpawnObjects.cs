using UnityEngine;
using System.Collections;

public class SpawnObjects : MonoBehaviour {
	public GameObject enemy;

	public float spawnDelay = 0.5f;

	private bool canSpawn = true;
	
	void ResetSpawn(){
		canSpawn = true;
	}

	// Update is called once per frame
	void Update () {
		if (canSpawn) {
			Instantiate(enemy, transform.position, transform.rotation);
			canSpawn = false;
			Invoke("ResetSpawn",spawnDelay);
		}
	}
}
