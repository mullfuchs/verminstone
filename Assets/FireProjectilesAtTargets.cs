using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectilesAtTargets : MonoBehaviour {

	public GameObject Projectile;

	public GameObject FiringPoint;

	private GameObject currentTarget = null;

	public float shootDelay = 2.0f;
	public float damage = 0.5f;
	public float projectileSpeed = 20.0f;
	public float fireSpread = 0.9f; //TODO, implement
	public float cost = 0.8f;
	public float secondsAlive = 5.0f;

	private bool canShoot = true;

	public float attackRange = 5.0f;

	private Transform shootTransform;

	void ResetShot(){
		canShoot = true;
	}

	// Use this for initialization
	void Start () {
		if (FiringPoint == null) {
			shootTransform = transform;
			shootTransform.position = transform.position;
			shootTransform.rotation = transform.rotation;
				
		} else {
			shootTransform = FiringPoint.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (canShoot) {
			lookForANewTarget ();
		}
	}

	void lookForANewTarget(){
		Collider[] allOverlappingColliders = Physics.OverlapSphere(transform.position, attackRange);

		List<GameObject> NPCsToTarget = new List<GameObject> ();
		for (int i = 0; i < allOverlappingColliders.Length; i++) {
			if (allOverlappingColliders [i].gameObject.tag == "WorkerNPC" || allOverlappingColliders [i].tag == "Player") {
				NPCsToTarget.Add (allOverlappingColliders [i].gameObject);
			}
		}
		//print ("why isn't this working, found " + NPCsToTarget.Count + "targets");
		GameObject target = null;
		float distance = 100.0f;
	
		foreach (GameObject item in NPCsToTarget) {
			float tempdist = Vector3.Distance (transform.position, item.transform.position);
			if (tempdist < distance) {
				target = item;
				distance = tempdist;
			}
		}
			
		ShootProjectile(target);
	}

	void ShootProjectile(GameObject target)
	{
		if (target != null) {
			Vector3 difference = target.transform.position - transform.position;
			float rotationZ = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
			shootTransform.rotation = Quaternion.Euler(0.0f, rotationZ, 0.0f);

			GameObject projectile = Instantiate(Projectile, shootTransform.position, shootTransform.rotation) as GameObject;

			Physics.IgnoreCollision (projectile.GetComponent<Collider> (), GetComponent<Collider> ());
			projectile.GetComponent<MoveForward> ().originObject = gameObject;
		}
		canShoot = false;
		Invoke("ResetShot", shootDelay);

	}

}
