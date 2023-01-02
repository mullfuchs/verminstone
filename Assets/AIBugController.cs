using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBugController : MonoBehaviour {
	public float walkSpeed;
	public GameObject target;

	public float probabilityOfRunningAway;

	private GameObject AttackHitBox;
	private GameObject OriginObject;

    private AudioSource audioSource;

    public AudioClip spawnSound;
    public AudioClip hitSomethingSound;
    public AudioClip dieSound;

	private bool CanAttack = true;
	private bool isAttacking = true;

	private int FrameTimeout = 0;

	public bool patrol = false;
	// Update is called once per frame
	void Start (){
        audioSource = GetComponent<AudioSource>();

		AttackHitBox = transform.Find("AttackHitBox").gameObject;
		AttackHitBox.SetActive(false);

        audioSource.PlayOneShot(spawnSound);
		if (!patrol) {
			if (Random.Range (0, 2) <= 0 && target == null) {
				target = GameObject.FindGameObjectWithTag ("Player");
			} else {
				target = GameObject.FindGameObjectWithTag ("WorkerNPC");
			}

			this.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target = target.transform;
		} else {
			isAttacking = false;
		}

	}

	void Update () {
		if (isAttacking) {
			if (target != null) {
				float distToEnemy = Vector3.Distance (target.transform.position, transform.position);
				if (distToEnemy < 2.5f && CanAttack) {
					PerformAttack ();
				}
			} else if(!patrol) {
				setTarget (GameObject.FindGameObjectWithTag ("Player"));
			}
		} else {
			//setTarget (OriginObject);
		}

		FrameTimeout++;
		if (FrameTimeout >= 10) {
			FrameTimeout = 0;
			if (target != null) {
				CheckForCloserTarget (target);
			}
		}

	}

	public void setUpBug(GameObject target, GameObject origin, int floorLevel){
		setTarget (target);
		setOriginObject (origin);
		//health right now is floor level x 10
		float healthpoints = floorLevel * 30;
		gameObject.GetComponent<health>().healthPoints = healthpoints;
		//run away probability is set by health, probably? I think?
		//probabilityOfRunningAway = ;
		//attack amount is a lot, probably
		AttackHitBox = transform.Find("AttackHitBox").gameObject;
		AttackHitBox.SetActive(true);
		AttackHitBox.GetComponent<DealDamageToObjects>().baseDamageAmount = floorLevel * 6;
		AttackHitBox.SetActive (false);
		//run speed is set last, if we have a lot of hp and attack, then they'll be slower.
		walkSpeed = (healthpoints * 2 + floorLevel) / healthpoints;
		print("bug stats: hp " + healthpoints + ", damage " + (floorLevel * 2) + ", walk speed " + walkSpeed);  
	}

	public void setUpPatrolBug(Transform target, int floorLevel){
		setTargetTransform (target);
		patrol = true;
		float healthpoints = floorLevel * 50;
		gameObject.GetComponent<health> ().healthPoints = healthpoints;
		AttackHitBox = transform.Find("AttackHitBox").gameObject;
		AttackHitBox.SetActive(true);
		AttackHitBox.GetComponent<DealDamageToObjects>().baseDamageAmount = floorLevel * 2;
		AttackHitBox.SetActive (false);
		walkSpeed = (healthpoints * 2 + floorLevel) / healthpoints;
		print("patrol bug stats: hp " + healthpoints + ", damage " + (floorLevel * 2) + ", walk speed " + walkSpeed);  
	}

	public void setTarget(GameObject newTarget)
	{
		this.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = newTarget.transform;
	}

	public void setTargetTransform(Transform newTarget)
	{
		this.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target = newTarget;
	}

	public void setOriginObject(GameObject origin)
	{
		OriginObject = origin;
	}

	public void PerformAttack()
	{
		print ("Bug performing a hit attack");
		AttackHitBox.SetActive(true);
		CanAttack = false;
		Invoke("HideHitBox", 0.5f);
		Invoke("ResetAttack", 1.5f);
        audioSource.PlayOneShot(hitSomethingSound);
    }

	public void HideHitBox()
	{
		AttackHitBox.SetActive(false);
	}

	public void ResetAttack()
	{
		CanAttack = true;
	}

	void OnCollisionEnter(Collision obj){
		if (obj.gameObject.tag == "projectile") {
			if (Random.Range (0, 1) <= probabilityOfRunningAway && probabilityOfRunningAway > 0) {
				isAttacking = false;
			} 
		}

		if (obj.gameObject.tag == "WorkerNPC") {
			isAttacking = true;
			setTarget (obj.gameObject);
		}
	}

	void OnTriggerEnter(Collider obj){
		
	}

	void CheckForCloserTarget(GameObject target){
		Collider[] targets = Physics.OverlapSphere (gameObject.transform.position, 3.0f);
		float TargetDist = Vector3.Distance (target.transform.position, gameObject.transform.position);
		foreach (Collider c in targets) {
			if (c.gameObject.tag == "WorkerNPC" || c.gameObject.tag == "Player") {
				float checkDist = Vector3.Distance (c.gameObject.transform.position, gameObject.transform.position);
				if (c.gameObject != target && checkDist < TargetDist) {
					setTarget (c.gameObject);
					print ("bug found a closer target");
					return;
				}
			}
		}
	}

}
