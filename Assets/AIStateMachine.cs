using UnityEngine;
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

    public Queue targets = new Queue();

	private float vStoneAmount = 0.0f; //hard coding value for test purposes

    private float defaultStoppingDist = 3.0f;
    private float itemStoppingDist = 0.5f;
    private bool GoToNextTargetWhenCurrentTargetReached = false;

    private VStoneEconomyObject VStoneEcoInstance;
    private CampNarrativeController narrativeController;

    private bool canCarryVstone = true;

    public AudioClip mineRockHitSound;
    public AudioClip swordHitSound;
    public AudioClip stonePickupSound;
    private AudioSource m_AudioSource;

    // Use this for initialization
    void Start () {
		//DefaultTarget = this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target.gameObject;
		vStoneAmount = 0.0f;
		AttackHitBox = transform.Find ("AttackHitBox").gameObject;
		AttackHitBox.SetActive (false);
        updateStoppingDistance(defaultStoppingDist);
        m_AudioSource = GetComponent<AudioSource>();
        VStoneEcoInstance = GameObject.Find("CampEventController").GetComponent<VStoneEconomyObject>();
        narrativeController = GameObject.Find("CampEventController").GetComponent<CampNarrativeController>();

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
        targets.Clear();
        currentState = AIState.Follow;
    }

    public void RecallFightingEnemies()
    {
        GameObject handobj = gameObject.GetComponent<NPCInventory>().ObjectHeldInHands;
        if (handobj != null && (handobj.GetComponent<EquippableItem>().itemName == "Pickaxe" || handobj.tag == "Weapon"))
        {
            targets.Clear();
        }

        currentState = AIState.Follow;
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R) || Input.GetButton("Recall")) {
            RecallFightingEnemies();
		}


		if (currentState == AIState.Angry) {
			updateStoppingDistance (0);
			if (EnemyAttackingMe != null) {

				float distToEnemy = Vector3.Distance(EnemyAttackingMe.transform.position, transform.position);
				if (distToEnemy <= 4.5f && CanAttack) {
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
            if (backobject != null && backobject.tag == "BagTool" && canCarryVstone)
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
			if (gameObject.GetComponent<NPCInventory> ().ObjectHeldInHands.tag == "MineTool") {
				setTarget(other.gameObject);
				EnemyAttackingMe = other.gameObject;
				currentState = AIState.Angry;		
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
                canCarryVstone = true;
			}
		}

        if (other.tag == "VStoneFragment" /* && other.gameObject == getTargetObject() */)
        {
            if (gameObject.GetComponent<NPCInventory>().ObjectOnBack.tag == "BagTool")
            {
                print("picking up stone");
                //check bag capacity vs what I have
                float vStoneFragmentAmount = 5.0f;
                int cap = gameObject.GetComponent<NPCInventory>().ObjectOnBack.GetComponent<Vstonebag>().vStoneCapacity;
                if (cap < (int)vStoneAmount + vStoneFragmentAmount)
                {
                    canCarryVstone = false;
                    targets.Clear();
                    print("bag full, can't carry anymore");
                    //it's full, empty target queue of additional vstone including this one and set a flag
                    //give vstone to other npcs? 
                    while(targets.Count > 0)
                    {
                        GameObject target = (GameObject)targets.Peek();
                        if(target.tag == "VStoneFragment")
                        {
                            ChannelerIFollow.GetComponent<NPCTeamHandler>().AddTargetForNPCs(target);
                        }

                        targets.Dequeue();
                        
                    }
                }
                gameObject.GetComponent<NPCInventory>().ObjectOnBack.GetComponent<Vstonebag>().currentVStoneAmount += vStoneFragmentAmount;
                vStoneAmount += vStoneFragmentAmount;
                m_AudioSource.PlayOneShot(stonePickupSound);
                ChannelerIFollow.GetComponent<NPCTeamHandler>().addCollectedVStone(vStoneFragmentAmount);
                if (targets.Count > 0)
                {
                    targets.Dequeue();
                }
                Destroy(other.gameObject);
                CheckIfTheresATargetInMyQueue();
            }
        }

		if (other.tag == "projectile") {
			GameObject projectileOrigin = other.gameObject.GetComponent<MoveForward> ().originObject;
			if (projectileOrigin != null) {
				//setTarget (ChannelerIFollow);
				//EnemyAttackingMe = projectileOrigin;
				//currentState = AIState.Scared;
			}
		}

        if (other.tag == "BagTool" && other.gameObject == getTargetObject())
        {
            ChannelerIFollow.GetComponent<NPCTeamHandler>().addCollectedVStone(other.gameObject.GetComponent<Vstonebag>().currentVStoneAmount);
            vStoneAmount += other.gameObject.GetComponent<Vstonebag>().currentVStoneAmount;
            gameObject.GetComponent<NPCInventory>().EquipBackItem(other.gameObject);
            Destroy(other.gameObject);
            gameObject.GetComponent<NPCInventory>().DropBackItem();
            GameObject.Find("Canvas").GetComponent<UIController>().ResetNPCCards();
            targets.Dequeue();
        }

		if (other.gameObject.name == "EquipmentReturn") {
			if (narrativeController.timeOfDay == CampNarrativeController.timePeriod.Evening) { //only return equipment if it's night time
				gameObject.GetComponent<NPCInventory> ().DestroyAllEquippedObjects ();
                targets.Clear(); //clearing targets to solve weird pathing problem
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
       if (other.gameObject.tag == "VerminStone") {
           if (gameObject.GetComponent<NPCInventory> ().ObjectHeldInHands.tag == "MineTool") {
               setTarget(other.gameObject);
               EnemyAttackingMe = other.gameObject;
               currentState = AIState.Angry;		
           }
       }

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
        m_AudioSource.PlayOneShot(stonePickupSound);
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
		if (this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target != null) {
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

    public void SetVstoneAmount(float amount)
    {
        vStoneAmount = amount;
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

    public void handleDeath(GameObject target)
    {
        //tell NPCTeamHandler it's died
		print("npc died :(");
        if(target != null && target.tag == "BagTool")
        {
            //if target is a bag object, add that to the npc team handler queue or whatever
            ChannelerIFollow.GetComponent<NPCTeamHandler>().makeNPCPickUpBag(target, vStoneAmount);
        }
		ChannelerIFollow.GetComponent<NPCTeamHandler>().RefreshNPCMinerList();


    }

	public void handleRessurection()
	{
		ChannelerIFollow.GetComponent<NPCTeamHandler>().RefreshNPCMinerList();
	}

	public void PerformAttack(){
		AttackHitBox.SetActive (true);
		CanAttack = false;
        if(gameObject.GetComponent<NPCInventory>().ObjectHeldInHands.tag == "MineTool")
        {
            m_AudioSource.PlayOneShot(mineRockHitSound);
            gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().PlayMineAnimation(true);
        }
        else
        {
            m_AudioSource.PlayOneShot(swordHitSound);
            gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().PlayAttackAnimation(true);
        }
        
		Invoke ("HideHitBox", 0.5f);
		Invoke ("ResetAttack", 1.5f);
	}

	public void HideHitBox(){
		AttackHitBox.SetActive (false);
	}

	public void ResetAttack(){
		CanAttack = true;
        gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().PlayAttackAnimation(false);
        gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().PlayMineAnimation(false);
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

    public void HardSendNPCToObject(GameObject target)
    {
        targets.Clear();
        targets.Enqueue(target);
        updateStoppingDistance(itemStoppingDist);
    }

}

