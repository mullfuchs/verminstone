using UnityEngine;
using System.Collections;

public class SpawnObjects : MonoBehaviour {
	public GameObject enemy;
    public GameObject SwarmEnemy;

	public float spawnDelay = 0.5f;

	private bool canSpawn = true;
	private int floorLevel = 0;

	void ResetSpawn(){
		canSpawn = true;
	}

	// Update is called once per frame
	void Update () {
		//if (canSpawn) {
			//Instantiate(enemy, transform.position, transform.rotation);
			//canSpawn = false;
			//Invoke("ResetSpawn",spawnDelay);
		//}
	}

	public void setFloorLevel(int level){
		floorLevel = level;
	}

	public void SpawnEnemy(GameObject objectToSpawn, GameObject[] TargetsForThatEnemy, int numberToSpawn, int floor)
    {
		floorLevel = floor;
		StartCoroutine(spawnASetOfEnemies(0.5f, numberToSpawn, objectToSpawn, TargetsForThatEnemy));
    }

    private IEnumerator spawnASetOfEnemies(float delay, int amount, GameObject objectToSpawn, GameObject[] targets)
    {
		int count = amount;
        print("targets: " + count);
		while(amount > 0)
        {
			StartCoroutine(spawnSingleEnemyTimed(delay, amount, objectToSpawn, targets[Random.Range(0, targets.Length)]));
			amount--;
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator spawnSingleEnemyTimed(float delay, int amount, GameObject objectToSpawn, GameObject target)
    {
       	GameObject bugToSpawn = Instantiate(objectToSpawn, transform.position, transform.rotation);
		bugToSpawn.GetComponent<AIBugController> ().setUpBug (target, gameObject, floorLevel);
        yield return new WaitForSeconds(delay);
    }



}
