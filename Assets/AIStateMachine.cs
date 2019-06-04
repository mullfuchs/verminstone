﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets;


public class AIStateMachine : MonoBehaviour {

	public enum AIState { Follow, GetStone, ReturningToCart, Scared, Angry, Idle};

	public GameObject ChannelerIFollow;

	public AIState currentState = AIState.Follow;

	private GameObject CurrentTarget;
	private GameObject DefaultTarget;
	public GameObject MineCartTarget;

	private GameObject FollowUpTarget;

    private GameObject EnemyAttackingMe;
	private GameObject AttackHitBox;
	private bool CanAttack = true;

    Queue targets = new Queue();

	private float vStoneAmount = 0.0f; //hard coding value for test purposes

    private float defaultStoppingDist = 3.0f;
    private float itemStoppingDist = 1.0f;
    private bool GoToNextTargetWhenCurrentTargetReached = false;

    private VStoneEconomyObject VStoneEcoInstance;

		// Use this for initialization
	void Start () {
		//DefaultTarget = this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target.gameObject;
		vStoneAmount = 0.0f;
		AttackHitBox = transform.Find ("AttackHitBox").gameObject;
		AttackHitBox.SetActive (false);
        updateStoppingDistance(defaultStoppingDist);
        VStoneEcoInstance = GameObject.Find("CampEventController").GetComponent<VStoneEconomyObject>();
    }

	void Awake(){
		//DefaultTarget = this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target.gameObject;
		ChannelerIFollow = GameObject.Find ("Player");
		DefaultTarget = ChannelerIFollow;
	}

	public void GetStone(GameObject stone){
		setTarget (stone);
		currentState = AIState.GetStone;
	}

	public void Follow(GameObject follow){
		setTarget (follow);
		currentState = AIState.Follow;
	}

	public void ReturnToMineCart(GameObject MineCartObject){
		setTarget (MineCartObject);
		currentState = AIState.ReturningToCart;
	}

	public void SetNPCTarget(GameObject target){
		setTarget (target);
		if (FollowUpTarget != null) {
			FollowUpTarget = null;
		}
	}
		
    public void ResetNPCVariables()
    {
		//print ("Clearing npc targets");
        targets.Clear();
		currentState = AIState.Follow;
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			ResetNPCVariables ();
		}


		if (currentState == AIState.Angry) {
			updateStoppingDistance (0);
			if (EnemyAttackingMe != null) {

				float distToEnemy = Vector3.Distance(EnemyAttackingMe.transform.position, transform.position);
				if (distToEnemy <= 1.5f && CanAttack) {
					print ("performing attack");
					PerformAttack ();
				}
			}
            else
            {
				//find an enemy?
				if (targets.Count > 0) {
					EnemyAttackingMe = (GameObject)targets.Dequeue ();	
					setTarget (EnemyAttackingMe);
				} else {
					print ("no longer attacking, returning to follow");
					ResetNPCVariables ();
					//currentState = AIState.Follow;
				}
            }
		}

        if (currentState == AIState.Follow)
        {
            CheckIfTheresATargetInMyQueue();
            CheckIfIHaveNoTarget();
        }
        
		if (currentState == AIState.Scared) {
			if (EnemyAttackingMe == null) {
				print ("no longer being attacked, returning to normal");
				currentState = AIState.Follow;
			}
		}

		if (GoToNextTargetWhenCurrentTargetReached == true) {
			if (Vector3.Distance (gameObject.transform.position, getTarget().position) <= defaultStoppingDist + 0.2f) {
				if (targets.Count != 0) {
					setTarget ((GameObject)targets.Dequeue ());
					GoToNextTargetWhenCurrentTargetReached = false;
				}
			}	
		}
	

	}

	void setTarget(GameObject target){
        if(target != null)
        {
            this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = target.transform;
			this.GetComponent<IKControl> ().lookObj = target.transform;	
        }
	}

	Transform getTarget(){
		return this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target;
	}

	void CheckIfIHaveNoTarget(){
		if (this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target == null 
			&& gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().isActiveAndEnabled)
        {
            targets.Dequeue();
            CheckIfTheresATargetInMyQueue();
            //setTarget(ChannelerIFollow); uncomment this first
        }
	}

    void CheckIfTheresATargetInMyQueue()
    {

        if (ChannelerIFollow.GetComponent<NPCTeamHandler>().checkTargetCount() > 0)
        {
            GameObject backobject = gameObject.GetComponent<NPCInventory>().ObjectOnBack;
            if (backobject != null && backobject.tag == "BagTool")
            {
                targets.Enqueue(ChannelerIFollow.GetComponent<NPCTeamHandler>().GetATargetIfOneIsAvailable());
                //setTarget((GameObject)targets.Dequeue());
                //print("adding fragment to an NPC target list, count:" + targets.Count);
            }
        }

        if (targets.Count > 0)
        {
            setTarget((GameObject)targets.Peek());
            updateStoppingDistance(itemStoppingDist);
            //print("setting target from target queue");
        }
        else
        {
            setTarget(ChannelerIFollow);
            updateStoppingDistance(defaultStoppingDist);
        }

    }
		


	void OnTriggerEnter(Collider other){
		if (other.tag == "VerminStone") {
				if(this.tag == "Miner" && other.GetComponent<VStoneObject>().HasBeenTouched){
					//StartCoroutine( MineVerminStone(other.gameObject));
				}
				if(this.tag == "Carrier" && other.GetComponent<VStoneObject>().HasBeenMined){
					//PickUpVerminStone(other.gameObject);
					//Follow (DefaultTarget);
					//ReturnToMineCart(MineCartTarget);
				}
			}
			

        if (other.tag == "StoneDropOff")
        {
            if(vStoneAmount >= 0.0f)
            {
				targets.Dequeue ();
				CheckIfTheresATargetInMyQueue ();
                VStoneEcoInstance.AddVStoneToDailyTotal(vStoneAmount);
                vStoneAmount = 0;
            }
        }

		if (other.tag == "StoneDropoffDebug") {
			if(gameObject.GetComponent<NPCInventory>().ObjectOnBack.tag == "BagTool" && vStoneAmount >= 0.0f)
			{
				VStoneEcoInstance.AddVStoneToDailyTotal(vStoneAmount);
				vStoneAmount = 0;
			}
		}

        if (other.tag == "VStoneFragment" && other.gameObject == getTargetObject())
        {
            if (gameObject.GetComponent<NPCInventory>().ObjectOnBack.tag == "BagTool")
            {
                //print("picking up stone");
                
                vStoneAmount += 5.0f;
                ChannelerIFollow.GetComponent<NPCTeamHandler>().addCollectedVStone(5.0f);
                targets.Dequeue();
                Destroy(other.gameObject);
                CheckIfTheresATargetInMyQueue();
            }
        }

		if (other.tag == "projectile") {
			GameObject projectileOrigin = other.gameObject.GetComponent<MoveForward> ().originObject;
			if (projectileOrigin != null) {
				setTarget (ChannelerIFollow);
				EnemyAttackingMe = projectileOrigin;
				currentState = AIState.Scared;
			}
		}

        if(other.tag == "Bug")
        {

            if (gameObject.GetComponent<NPCInventory>().ObjectHeldInHands != null)
            {
                if (gameObject.GetComponent<NPCInventory>().ObjectHeldInHands.tag == "MineTool" || gameObject.GetComponent<NPCInventory>().ObjectHeldInHands.tag == "Weapon")
                {
                    setTarget(other.gameObject);
                    EnemyAttackingMe = other.gameObject;
                    currentState = AIState.Angry;
                    print("NPC is angry");
                }
            }
            else
            {
                setTarget(ChannelerIFollow);
                EnemyAttackingMe = other.gameObject;
                currentState = AIState.Scared;
               	print("NPC is scared");
            }
        }

    }

    void OnCollisionEnter(Collision other)
    {
        /*
        if (other.gameObject.tag == "Bug" && currentState == AIState.Angry)
        {
           other.gameObject.GetComponent<health>().AddDamage(1);
            print("NPC Did damage of 1");
        }
        */
    }

	void DropVerminStone(){
		foreach (Transform child in transform) {
			if(child.CompareTag("VerminStone")){
				Destroy(child.gameObject);
				child.GetComponent<SphereCollider>().enabled = false;
				child.parent = null;
			}
		}
		this.GetComponent<IKControl> ().ikActive = false;
	}

	void PickUpVerminStone(GameObject vStone){
		vStoneAmount += 0.6f;
		Destroy (vStone);
	}

	IEnumerator MineVerminStone(GameObject rock){
		yield return new WaitForSeconds(2);
		Follow(DefaultTarget);
		ChannelerIFollow.GetComponent<NPCTeamHandler>().AddStoneToBePickedUp(rock);
		AddSelfToAvailibleMiners ();
	}

	void AddSelfToAvailibleMiners(){
		ChannelerIFollow.GetComponent<NPCTeamHandler> ().AddNPCToMinerQueue (gameObject);
	}

	void AddSelfToAvailibleCarriers(){
		ChannelerIFollow.GetComponent<NPCTeamHandler> ().AddNPCToCarrierQueue (gameObject);
	}
		
	GameObject getTargetObject(){
		if (this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> () != null) {
			GameObject obj = this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target.gameObject;
			if(obj != null)
			{
				return obj;
			}
			else
			{
				return null;
			}
		}
		return null;
        //return this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target.gameObject;
	}


	public void SetFollowupTarget(GameObject Target){
		FollowUpTarget = Target;
	}

	public float GetVerminStoneAmount(){
		return vStoneAmount;
	}

    public void AddTargetForNPC(GameObject target)
    {
        targets.Enqueue(target);
        //print("adding target to an NPC target list, count:" + targets.Count);
    }


    public void AttackEnemy(GameObject enemy)
    {
        currentState = AIState.Angry;
        EnemyAttackingMe = enemy;
        setTarget(enemy);
		print ("number of targets im attacking " + targets.Count );
    }

    public void handleDeath()
    {
        //tell NPCTeamHandler it's died
		print("npc died :(");
		ChannelerIFollow.GetComponent<NPCTeamHandler>().RefreshNPCMinerList();
    }

	public void PerformAttack(){
		AttackHitBox.SetActive (true);
		CanAttack = false;
		Invoke ("HideHitBox", 0.5f);
		Invoke ("ResetAttack", 1.5f);
	}

	public void HideHitBox(){
		AttackHitBox.SetActive (false);
	}

	public void ResetAttack(){
		CanAttack = true;
	}

    void updateStoppingDistance(float dist)
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = dist;
    }

    public void SetGoToNextTargetFlag()
    {
        GoToNextTargetWhenCurrentTargetReached = true;
    }

    public void SendNPCToObject(GameObject target)
    {
        targets.Enqueue(target);
        SetGoToNextTargetFlag();
        updateStoppingDistance(itemStoppingDist);
    }

}

