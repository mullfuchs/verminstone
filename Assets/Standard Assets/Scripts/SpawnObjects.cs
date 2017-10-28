using UnityEngine;
using System.Collections;

public class SpawnObjects : MonoBehaviour {
	public GameObject enemy;
    public GameObject SwarmEnemy;

	public float spawnDelay = 0.5f;

	private bool canSpawn = true;
	
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

    public void SpawnEnemy(GameObject objectToSpawn, GameObject[] TargetsForThatEnemy, int numberToSpawn)
    {
        StartCoroutine(spawnASetOfEnemies(0.5f, numberToSpawn, objectToSpawn, TargetsForThatEnemy));
    }

    private IEnumerator spawnASetOfEnemies(float delay, int amount, GameObject objectToSpawn, GameObject[] targets)
    {
        int count = targets.Length;
        print("targets: " + count);
        while(count > 0)
        {
            StartCoroutine(spawnSingleEnemyTimed(delay, amount, SwarmEnemy, targets[count - 1]));
            count--;
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator spawnSingleEnemyTimed(float delay, int amount, GameObject objectToSpawn, GameObject target)
    {
        int count = amount;
        while(count >= 0)
        {
            GameObject bugToSpawn = Instantiate(objectToSpawn, transform.position, transform.rotation);
            bugToSpawn.GetComponent<LookAtTargetAndWalkForward>().setTarget(target);
            count--;
            yield return new WaitForSeconds(delay);
        }
    }



}
