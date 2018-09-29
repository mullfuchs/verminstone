using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBugController : MonoBehaviour {
	public float walkSpeed;
	public GameObject target;

	public float probabilityOfRunningAway;

	private GameObject AttackHitBox;
	private GameObject OriginObject;



	private bool CanAttack = true;
	private bool isAttacking = true;
	// Update is called once per frame
	void Start (){
		
		AttackHitBox = transform.Find("AttackHitBox").gameObject;
		AttackHitBox.SetActive(false);

		if (Random.Range (0, 2) <= 0 && target == null) {
			target = GameObject.FindGameObjectWithTag ("Player");
		} 
		else {
			target = GameObject.FindGameObjectWithTag ("WorkerNPC");
		}

		this.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target = target.transform;
	}

	void Update () {
		if (isAttacking) {
			if (target != null) {
				float distToEnemy = Vector3.Distance (target.transform.position, transform.position);
				if (distToEnemy <= 1.5f && CanAttack) {
					PerformAttack ();
				}
			} else {
				setTarget (GameObject.FindGameObjectWithTag ("Player"));
			}
		} else {
			setTarget (OriginObject);
		}

	}

	public void setTarget(GameObject newTarget)
	{
		this.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = newTarget.transform;
	}

	public void setOriginObject(GameObject origin)
	{
		OriginObject = origin;
	}

	public void PerformAttack()
	{
		AttackHitBox.SetActive(true);
		CanAttack = false;
		Invoke("HideHitBox", 0.5f);
		Invoke("ResetAttack", 1.5f);
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
			isAttacking = false;
		}
	}

	void OnTriggerEnter(Collider obj){
		
	}

}
