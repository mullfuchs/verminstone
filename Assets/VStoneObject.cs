using UnityEngine;
using System.Collections;

public class VStoneObject : MonoBehaviour {

	public bool HasBeenTouched = false;
	public bool HasBeenMined = false;
	public bool IsBeingMined = false;
    private bool SentForHelp = false;
	private int minersInRadius = 0;
	public int healthPoints = 500;
	public float energy = 0.5f;
	private float timer = 1.0f;
	private float timerOGval = 0;
	public GameObject VStoneFragmentObject;
    private GameObject playerObject;
    private GameObject EnemyTeamHandler;
	public int FragmentsToMake = 5;
    public GameObject ShatterEffectObject;

	public float ShardSpawnRadius = 0.5f;

    public AudioClip destructionSound;

	// Use this for initialization
	void Start () {
		timerOGval = timer;
        playerObject = GameObject.Find("Player");
        EnemyTeamHandler = GameObject.Find("EnemyNPCHandler");
	}
	
	// Update is called once per frame
	void Update () {
		if (IsBeingMined) {
			timer -= Time.deltaTime;
			if (timer <= 0.0f) {
				timer = timerOGval;
		//		MineStone ();
			}
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Player" && !HasBeenTouched) {
			HasBeenTouched = true;
			this.GetComponent<ParticleSystem>().Stop();
		}

		if (other.gameObject.tag == "WorkerHitBox") {
			healthPoints -= other.transform.parent.GetComponent<NPCstats> ().attack;
			if (healthPoints <= 0) {
				DestroyStoneAndCreateRocksToPickUp ();		
			}

			if(SentForHelp == false)
			{
				EnemyTeamHandler.GetComponent<EnemyTeamHandler>().sendSwarmToAttackWhenVStoneMined(gameObject);
				SentForHelp = true;
			}
		}
	}

	void OnTriggerEnter(Collider other){


		if (other.tag == "WorkerNPC" && HasBeenTouched) {
			print ("stone being mined");
			if (IsBeingMined == false) {
				IsBeingMined = true;
			}
			minersInRadius++;


		}

//		if (other.tag == "Miner" && HasBeenTouched) {
//			HasBeenMined = true;
//		}
//
//		if (other.tag == "Carrier" && HasBeenMined) {
//			this.GetComponent<SphereCollider>().enabled = false;
//		}

	}
	void OnTriggerExit(Collider other){
		if (other.tag == "Player") {
			energy = 0;
		}

		if (other.tag == "NPCWorker") {
			minersInRadius--;
			if (minersInRadius <= 0) {
				IsBeingMined = false;
			}
		}
	}

	void MineStone(){
		if (minersInRadius >= 1) {
			healthPoints -= minersInRadius * 10;
			if (healthPoints <= 0) {
				DestroyStoneAndCreateRocksToPickUp ();		
			}
		}
	}

    public void DamageStone(float damage)
    {
        healthPoints -= (int)damage;

        if (SentForHelp == false)
        {
            EnemyTeamHandler.GetComponent<EnemyTeamHandler>().sendSwarmToAttackWhenVStoneMined(gameObject);
            SentForHelp = true;
        }

        if (healthPoints <= 0)
        {
            DestroyStoneAndCreateRocksToPickUp();
        }
    }

	void DestroyStoneAndCreateRocksToPickUp(){
		float radiusOffset = 360 / FragmentsToMake;
		for (int i = 0; i < FragmentsToMake; i++) {
			Vector3 newPosition = gameObject.transform.position + new Vector3(ShardSpawnRadius * Mathf.Cos(radiusOffset * i), 0.5f ,ShardSpawnRadius * Mathf.Sin(radiusOffset * i) );
			GameObject fragment = Instantiate (VStoneFragmentObject, newPosition, Quaternion.identity);
			fragment.GetComponent<Rigidbody> ().AddForce (new Vector3 (Random.Range(1.0f, 5.0f) , 0.5f, Random.Range(1.0f, 5.0f) ), ForceMode.Impulse);
			playerObject.GetComponent<NPCTeamHandler>().AddTargetForNPCs( fragment );
		}
		//try to remove it from the list
		if (GameObject.Find ("CaveManager") != null) {
			GameObject.Find("CaveManager").GetComponent<CaveManager>().RemoveObjectFromFloor(gameObject);
		}
        AudioSource.PlayClipAtPoint(destructionSound, this.transform.position);
        if(ShatterEffectObject != null)
        {
            Instantiate(ShatterEffectObject, gameObject.transform.position, gameObject.transform.rotation);
        }
        Destroy(gameObject);
	}
}
