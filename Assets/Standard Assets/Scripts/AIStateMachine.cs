using UnityEngine;
using System.Collections;
using UnityStandardAssets;


public class AIStateMachine : MonoBehaviour {

	public enum AIState { Follow, GetStone, ReturningToCart, Scared, Angry};

	public GameObject ChannelerIFollow;

	public AIState currentState = AIState.Follow;

	private GameObject CurrentTarget;
	private GameObject DefaultTarget;
	public GameObject MineCartTarget;

	private GameObject FollowUpTarget;

    private GameObject EnemyAttackingMe;

    Queue targets = new Queue();

	private float vStoneAmount = 6.9f; //hard coding value for test purposes

		// Use this for initialization
	void Start () {
		//DefaultTarget = this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target.gameObject;
		vStoneAmount = 0.0f;
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
		

	// Update is called once per frame
	void Update () {
        if(currentState == AIState.Angry || currentState == AIState.Scared)
        {
            if(EnemyAttackingMe == null)
            {
                print("NPC Returning to default state");
                currentState = AIState.Follow;
            }
        }
        


        if (currentState == AIState.Follow)
        {
            CheckIfTheresATargetInMyQueue();
            CheckIfIHaveNoTarget();
        }
        


		if (FollowUpTarget != null) {
			if (Vector3.Distance (gameObject.transform.position, getTarget().position) <= 1.5f) {
				setTarget (FollowUpTarget);
			}	
		}

	}

	void setTarget(GameObject target){
        if(target != null)
        {
            this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = target.transform;
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
                print("adding fragment to an NPC target list, count:" + targets.Count);
            }
        }

        if (targets.Count > 0)
        {
            setTarget((GameObject)targets.Peek());
            //print("setting target from target queue");
        }
        else
        {
            setTarget(ChannelerIFollow);
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

		if (other.tag == "MineCart"){
			if(this.tag == "Carrier"){
				DropVerminStone();
				Follow(DefaultTarget);
				AddSelfToAvailibleCarriers();
			}
		}

        if (other.tag == "VStoneFragment" && other.gameObject == getTargetObject())
        {
            if (gameObject.GetComponent<NPCInventory>().ObjectOnBack.tag == "BagTool")
            {
                print("picking up stone");
                vStoneAmount += 5.0f;
                targets.Dequeue();
                Destroy(other.gameObject);
                CheckIfTheresATargetInMyQueue();
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
        if (other.gameObject.tag == "Bug" && currentState == AIState.Angry)
        {
           other.gameObject.GetComponent<health>().AddDamage(1);
            print("NPC Did damage of 1");
        }
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
		//vStone.transform.SetParent (this.transform);
		//vStone.transform.localPosition = new Vector3 (0f, 3.61f, 1.77f);
		//this.GetComponent<IKControl>().ikActive = true;
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
        GameObject obj = this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target.gameObject;
        if(obj != null)
        {
            return obj;
        }
        else
        {
            return null;
        }
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
        print("adding target to an NPC target list, count:" + targets.Count);
    }

}

