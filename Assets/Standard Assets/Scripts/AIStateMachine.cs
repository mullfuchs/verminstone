using UnityEngine;
using System.Collections;
using UnityStandardAssets;


public class AIStateMachine : MonoBehaviour {

	public enum AIState { Follow, GetStone, ReturningToCart };

	public GameObject ChannelerIFollow;

	public AIState currentState = AIState.Follow;

	private GameObject CurrentTarget;
	private GameObject DefaultTarget;
	public GameObject MineCartTarget;

	private GameObject FollowUpTarget;

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
		CheckIfIHaveNoTarget ();
        CheckIfTheresATargetInMyQueue();
		if (FollowUpTarget != null) {
			if (Vector3.Distance (gameObject.transform.position, getTarget().position) <= 1.5f) {
				setTarget (FollowUpTarget);
			}	
		}

	}

	void setTarget(GameObject target){
		this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target = target.transform;
	}

	Transform getTarget(){
		return this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target;
	}

	void CheckIfIHaveNoTarget(){
		if (this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target == null 
			&& gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().isActiveAndEnabled)
        {
                
            if(targets.Count > 0)
            {
                print("NPC going to target in queue");
                setTarget((GameObject)targets.Dequeue());
            }
            else
            {
                print("NPC following player");
                setTarget(ChannelerIFollow);
            }
                
		}
	}

    void CheckIfTheresATargetInMyQueue()
    {
        if (targets.Count > 0)
        {
            print("NPC going to target in queue");
            setTarget((GameObject)targets.Dequeue());
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
		return this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target.gameObject;
	}


	public void SetFollowupTarget(GameObject Target){
		FollowUpTarget = Target;
	}

	public float GetVerminStoneAmount(){
		return vStoneAmount;
	}

    public void AddTargetForNPC(GameObject target)
    {
        print("added target for npc");
        targets.Enqueue(target);
    }

}

