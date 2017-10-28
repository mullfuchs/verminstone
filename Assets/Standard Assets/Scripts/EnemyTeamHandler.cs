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
    public GameObject HeavyPatrolEnemy;

    private NPCTeamHandler playerTeamHandler;

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
        List<GameObject> MinerList = playerTeamHandler.GetCurrentMiners();

        closestsSpawner.GetComponent<SpawnObjects>().SpawnEnemy(SwarmEnemy, MinerList.ToArray(), 2);


    }

    private GameObject findClosestSpawner(Vector3 targetPosition)
    {
        ListOfSpawnersOnFloor = GameObject.FindGameObjectsWithTag("Spawner");
        return ListOfSpawnersOnFloor[0];
        //GameObject closestSpawn = ListOfSpawnersOnFloor[0];
        /*
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
        */
       // return closestSpawn;
    }


}
