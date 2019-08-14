using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamHandler : MonoBehaviour {

    private List<GameObject> NPCList;
    private List<GameObject> CarrierList;
    private List<GameObject> MinerList;

    private GameObject[] ListOfSpawnersOnFloor;

    public GameObject SwarmEnemy;
    public GameObject MediumEnemy;
    public GameObject BossEnemy;

	public int floorLevel = 1;

    private NPCTeamHandler playerTeamHandler;

	public FloorSpawnRates[] SpawnRatesForFloors;

	/*
	 * so this here needs to better control spawning
	 * I basically wanna control how many bugs are spawned, and which are spawned, at what level
	 * because right now it sucks
	 * ANYWAY. I should have like a table that this imports
	 * and it looks at the floor and it looks at the table and goes "ah, send X Y and Z to the enemies"
	 * it could be a class, that holds amounts per level, and it's stored in an array, which I define?
	 * I could parse it from a json that would be less work, it would mean not going into testMap every
	 * time I wanna fix it or balance it.
	*/

    // Use this for initialization
    void Start () {
        NPCList = new List<GameObject>();
        CarrierList = new List<GameObject>();
        MinerList = new List<GameObject>();

        playerTeamHandler = GameObject.Find("Player").GetComponent<NPCTeamHandler>();

        ListOfSpawnersOnFloor = GameObject.FindGameObjectsWithTag("Spawner");


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void sendSwarmToAttackWhenVStoneMined(GameObject rockMined)
    {
        print("Sending A Swarm");
        GameObject closestsSpawner = findClosestSpawner(rockMined.transform.position);
		GameObject randSpawner = findARandomSpawner ();

		List<GameObject> MinerList = playerTeamHandler.GetCurrentMiners();

        //closestsSpawner.GetComponent<SpawnObjects>().SpawnEnemy(SwarmEnemy, MinerList.ToArray(), floorLevel, floorLevel);
		//randSpawner.GetComponent<SpawnObjects> ().SpawnEnemy (SwarmEnemy, MinerList.ToArray (), floorLevel, floorLevel);


		if (floorLevel > SpawnRatesForFloors.Length) {
			MakeSwarmFromSpawnRate (closestsSpawner, MinerList.ToArray(), SpawnRatesForFloors [SpawnRatesForFloors.Length]);
		} else {
			MakeSwarmFromSpawnRate (closestsSpawner, MinerList.ToArray(), SpawnRatesForFloors [floorLevel]);
		}


    }

	private void MakeSwarmFromSpawnRate(GameObject spawner, GameObject[] targets, FloorSpawnRates spawnRate){
		for(int i = 0; i < spawnRate.EnemySpawnRates.Length; i++){
			spawner.GetComponent<SpawnObjects> ().SpawnEnemy (spawnRate.EnemySpawnRates [i].enemyToSpawn, targets, 
				spawnRate.EnemySpawnRates [i].numberOfEnemyToSpawn, 0);
		}
	}

    private GameObject findClosestSpawner(Vector3 targetPosition)
    {
        ListOfSpawnersOnFloor = GameObject.FindGameObjectsWithTag("Spawner");
        //return ListOfSpawnersOnFloor[0];
        GameObject closestSpawn = ListOfSpawnersOnFloor[0];
        
        float distanceToSpawn = Vector3.Distance(closestSpawn.transform.position, targetPosition);

        foreach (GameObject b in ListOfSpawnersOnFloor)
        {
            float tempDist = Vector3.Distance(b.transform.position, targetPosition);
            if (tempDist < distanceToSpawn)
            {
                closestSpawn = b;
                distanceToSpawn = tempDist;
            }
        }
        
       return closestSpawn;
    }

	private GameObject findARandomSpawner()
	{
		ListOfSpawnersOnFloor = GameObject.FindGameObjectsWithTag ("Spawner");
		GameObject randomSpawn = ListOfSpawnersOnFloor [Random.Range(0, ListOfSpawnersOnFloor.Length)];

		return randomSpawn;
	}


	public class FloorSpawnRates{
		public EnemySpawnRate[] EnemySpawnRates;
	}

	public class EnemySpawnRate{
		public GameObject enemyToSpawn;
		public int numberOfEnemyToSpawn;
		public float probabilityOfSpawning;
	}
}
	