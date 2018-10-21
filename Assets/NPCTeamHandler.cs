using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets;

public class NPCTeamHandler : MonoBehaviour {

	public GameObject[] NPCMiners;
	public GameObject[] NPCCarriers = new GameObject[5];

	Queue ActiveStones = new Queue();
	Queue MinedStones = new Queue();

	Queue MinerQueue = new Queue();
	Queue CarrierQueue = new Queue();

    private Queue AvailabileTargets = new Queue();

    private List<GameObject> NPCList;

    private List<GameObject> CurrentMiners;
    private List<GameObject> CurrentCarriers;
    private List<GameObject> CurrentArmedNPCs;

    public GameObject UIObject;
    private UIController UIcontroller;

    private float KilogramsofVstoneCollected = 0;

    // Use this for initialization
    void Start () {
        UIObject = GameObject.Find("Canvas");
		//NPCMiners = GameObject.FindGameObjectsWithTag ("Miner");
		//NPCCarriers = GameObject.FindGameObjectsWithTag ("Carrier");

        NPCMiners = GameObject.FindGameObjectsWithTag("WorkerNPC");

        CurrentMiners = new List<GameObject>();
        CurrentArmedNPCs = new List<GameObject>();

        CurrentMiners = GetAllNPCSwithMineTools();
        CurrentCarriers = GetAllNPCSwithBagTools();
        CurrentArmedNPCs = GetAllNPCSwithWeapons();

		foreach (GameObject g in NPCMiners) {
			//print ("added miner");
			MinerQueue.Enqueue(g);
		}
		foreach (GameObject g in NPCCarriers) {
			//print ("added carrier");
			CarrierQueue.Enqueue(g);
		}

        UIcontroller = UIObject.GetComponent<UIController>();

	}
	
	// Update is called once per frame
	void Update () {
        if (NPCMiners.Length == 0)
        {
            NPCMiners = GameObject.FindGameObjectsWithTag("WorkerNPC");
        }

		if (ActiveStones.Count > 0) {
			//print ("Sending miners to mine rock");
            // get all miners with pickaxes

            // 
			SendNPCToMineRock((GameObject)MinerQueue.Dequeue(), (GameObject)ActiveStones.Peek());
		}
		
		if (MinedStones.Count > 0 && CarrierQueue.Count > 0) {
			//print ("Sending carrier to pick up rock");
			SendNPCToPickUpRock((GameObject)CarrierQueue.Dequeue(), (GameObject)MinedStones.Dequeue());
		}

        if (Input.GetButton("Order_Attack"))
        {
            print("Ordering NPCs to attack");
            OrderNPCsToAttackNearestNPC();
        }
	}

	void CheckToSeeIfARockCanBePickedUpOrMined(){

	}

	public void AddStoneToBeMined(GameObject rock){
        //ActiveStones.Enqueue (rock);
        //print("sending miners to rock");
        SendAllMinersToMineRock(rock);
	}

	public void AddStoneToBePickedUp(GameObject rock){
		MinedStones.Enqueue (rock);
	}

	public void AddNPCToMinerQueue(GameObject NPC){
		MinerQueue.Enqueue (NPC);
		CheckToSeeIfARockCanBePickedUpOrMined ();
	}

	public void AddNPCToCarrierQueue(GameObject NPC){
		CarrierQueue.Enqueue (NPC);
		CheckToSeeIfARockCanBePickedUpOrMined ();
	}

	public void MineVerminStone(GameObject rock){
		AddStoneToBeMined (rock);
	}

	public void SendNPCToMineRock(GameObject NPCFollower, GameObject rock){
		NPCFollower.GetComponent<AIStateMachine>().GetStone(rock);
	}

	public void SendNPCToPickUpRock(GameObject NPCCarrier, GameObject rock){
		NPCCarrier.GetComponent<AIStateMachine> ().GetStone (rock);
	}

	public void RefreshNPCMinerList(){
		NPCMiners = GameObject.FindGameObjectsWithTag("WorkerNPC");
	}

    public void SendAllMinersToMineRock(GameObject rock)
    {
		CurrentMiners = GetAllNPCSwithMineTools();
        foreach (GameObject Miner in CurrentMiners)
        {
           //print("sending a miner to the rock");
            Miner.GetComponent<AIStateMachine>().AddTargetForNPC(rock);
        }
    }

    List<GameObject> GetAllNPCSwithMineTools()
    {
		RefreshNPCMinerList ();
        List<GameObject> MinerList = new List<GameObject>();
        foreach (GameObject g in NPCMiners)
        {
            if(g.GetComponent<NPCInventory>().ObjectHeldInHands != null)
            {
                if (g.GetComponent<NPCInventory>().ObjectHeldInHands.tag == "MineTool")
                {
                    MinerList.Add(g);
                }
            }

        }
        return MinerList;
    }

    List<GameObject> GetAllNPCSwithBagTools()
    {
		RefreshNPCMinerList ();
        List<GameObject> CarrierList = new List<GameObject>();
        foreach(GameObject g in NPCMiners)
        {
            if(g.GetComponent<NPCInventory>().ObjectOnBack != null)
            {
                if (g.GetComponent<NPCInventory>().ObjectOnBack.tag == "BagTool")
                {
                    CarrierList.Add(g);
                }
            }
        }
        return CarrierList;
    }

    List<GameObject> GetAllNPCSwithWeapons()
    {
        List<GameObject> NPCList = new List<GameObject>();
        foreach (GameObject g in NPCMiners)
        {
            if (g.GetComponent<NPCInventory>().ObjectHeldInHands != null)
            {
				if (g.GetComponent<NPCInventory>().ObjectHeldInHands.tag == "Weapon" )
                {
                    NPCList.Add(g);
                }
            }

        }
        return NPCList;
    }

    public void AddTargetForNPCs(GameObject target)
    {
        AvailabileTargets.Enqueue(target);
    }

    public void DistributeFragmentsToCarrierNPCs()
    {

    }

    public GameObject GetATargetIfOneIsAvailable()
    {
       // print("Available targets " + (AvailabileTargets.Count));
        if (AvailabileTargets.Count > 0)
        {
            return (GameObject)AvailabileTargets.Dequeue();
        }
        else
        {
            return null;
        }
    }

    public int checkTargetCount()
    {
        return AvailabileTargets.Count;
    }

    void OrderNPCsToAttackNearestNPC()
    {
        //find bug closest to me
        // closestBug = null;
        float bugDistToPlayer = 0.0f;
        GameObject[] bugs = GameObject.FindGameObjectsWithTag("Bug");
		if (bugs.Length > 0) { 
			GameObject closestBug = bugs[0];
			print("found bugs:" + bugs.Length);

			if(bugs.Length == 0)
			{
				print("found no bugs");
				return;
			}		
		
		     
	        bugDistToPlayer = Vector3.Distance(closestBug.transform.position, gameObject.transform.position);

	        foreach (GameObject b in bugs)
	        {
	            float bugDist = Vector3.Distance(b.transform.position, gameObject.transform.position);
	            if (bugDist < bugDistToPlayer)
	            {
	                closestBug = b;
	                bugDistToPlayer = bugDist;
	            }
	        }
	        

	        foreach (GameObject g in CurrentArmedNPCs)
	        {
				g.GetComponent<AIStateMachine> ().ResetNPCVariables ();
				foreach (GameObject b in bugs) {
					if (b.activeSelf) {
						g.GetComponent<AIStateMachine> ().AddTargetForNPC (b);	
					}
				}
	            print("sending NPCS to attack a bug");
	            g.GetComponent<AIStateMachine>().AttackEnemy(closestBug);
	        }
		}
    }

    public List<GameObject> GetCurrentMiners()
    {
		return GetAllNPCSwithMineTools();
    }

    public List<GameObject> GetCurrentCarriers()
    {
        return GetAllNPCSwithBagTools();
    }

    public void handleNPCDeath()
    {
        //refresh lists
        //redistribute targets
    }

    public void distributeTargetsToNPCList()
    {

    }

    public void addCollectedVStone(float amount)
    {
        KilogramsofVstoneCollected += amount;
        UIcontroller.updateText(UIcontroller.VStoneAmountText, KilogramsofVstoneCollected.ToString());
    }

    public float getVStoneCollected()
    {
        return KilogramsofVstoneCollected;
    }

	public void rebuildNPCLists(){
		CurrentMiners = GetAllNPCSwithMineTools();
		CurrentCarriers = GetAllNPCSwithBagTools();
		CurrentArmedNPCs = GetAllNPCSwithWeapons();
		resetNPCTargets ();
	}

	public void resetNPCTargets(){
		NPCMiners = GameObject.FindGameObjectsWithTag("WorkerNPC");

		foreach (GameObject npc in NPCMiners)
		{
			npc.GetComponent<AIStateMachine>().ResetNPCVariables();
		}
	}

}
